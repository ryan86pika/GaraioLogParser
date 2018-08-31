using GaraioLogParser;
using GaraioLogParser.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;
using WebGaraioLogParser.Models;
using WebGaraioLogParser.Utils;

namespace WebGaraioLogParser.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload  
        public ActionResult Index() => View();

        [HttpGet]
        public ActionResult UploadFile() => View();

        [HttpPost]
        public ActionResult GetDataIntoJSON(string baseFileName)
        {
            var reason = string.Empty;
            if (ModelState.IsValid)
            {
                try
                {
                    // if file doesn't exists.. doesn't return a file path name
                    if (baseFileName != string.Empty)
                    {
                        IISLogParser parser = null;
                        string result = null;

                        if (baseFileName.EndsWith(".log"))
                        {
                            using (FileStream file = new FileStream(baseFileName, FileMode.Open))
                            {
                                parser = new IISLogParser(baseFileName);

                                //result = ConvertList2DataTable(parser.ParseW3CLog());
                                result = ConvertList2Json(parser.ParseW3CLog());

                                return this.Json(new ExtractDataResult { Success = true, Message = result });
                            }
                        }
                        else reason = "This file format is not supported";
                    }
                    else reason = "Please Upload Your file";
                }
                catch
                {
                    reason = "File parsing failed!!";
                }
            }

            ModelState.AddModelError("File", reason);
            return new HttpStatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized, reason);
        }

        [HttpPost]
        public ActionResult MergeFilesUploadedIntoSingleFile()
        {
            string jsonMsg = string.Empty;
            string baseFileName = string.Empty;

            foreach (string file in Request.Files)
            {
                var FileDataContent = Request.Files[file];
                if (FileDataContent != null && FileDataContent.ContentLength > 0)
                {
                    // take the input stream, and save it to a temp folder using the original file.part name posted
                    var stream = FileDataContent.InputStream;
                    var fileName = Path.GetFileName(FileDataContent.FileName);
                    var UploadPath = Server.MapPath(FileUtils.UPLOADED_FILE_PATH);
                    Directory.CreateDirectory(UploadPath);
                    string path = Path.Combine(UploadPath, fileName);
                    try
                    {
                        if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

                        using (var fileStream = System.IO.File.Create(path)) stream.CopyTo(fileStream);

                        // Once the file part is saved, see if we have enough to merge it
                        if (FileUtils.MergeFile(path))
                        {
                            if (baseFileName == string.Empty)
                                baseFileName = path.Substring(0, path.IndexOf(FileUtils.PART_TOKEN));

                            else if (baseFileName != string.Empty && baseFileName != path.Substring(0, path.IndexOf(FileUtils.PART_TOKEN))) throw new FileLoadException(Resource.LoadMergedFile);
                        }
                        else
                        {
                            string reason = "File not completed yet!";
                            return this.Json(new MergeFileResult { Success = true, Chunk = ExtractChunkNumber(path), MaxChunks = ExtractMaxChunkNumber(path), Message = reason });
                        }
                    }
                    catch(Exception ex)
                    {
                        string reason = "Error during uploading file! " + ex.Message;
                        return new HttpStatusCodeResult((int)System.Net.HttpStatusCode.Unauthorized, reason);
                    }
                }
            }

            return this.Json(new MergeFileResult { Success = true, BaseFileName = baseFileName });
        }

        private string ExtractChunkNumber(string path)
        {
            string patternChunk = path.Substring(path.IndexOf(FileUtils.PART_TOKEN) + FileUtils.PART_TOKEN.Length);
            return patternChunk.Substring(0, patternChunk.IndexOf("."));
        }

        private string ExtractMaxChunkNumber(string path)
        {
            string patternChunk = path.Substring(path.IndexOf(FileUtils.PART_TOKEN) + FileUtils.PART_TOKEN.Length);
            return patternChunk.Substring(patternChunk.IndexOf(".") + 1);
        }

        private DataTable ConvertList2DataTable(List<IPDataResult> inputList)
        {
            // New table.
            DataTable table = new DataTable();

            // Set columns names.
            string[] columnsName = new string[] { "Client IP", "FQDN", "N. Calls" };

            // Add columns.
            for (int i = 0; i < columnsName.Length; i++)
                table.Columns.Add(columnsName[i]);

            // Add rows.
            foreach (var element in inputList)
                table.Rows.Add(ConvertElement2StringArray(element));

            return table;
        }

        private string ConvertList2Json(List<IPDataResult> inputList) => JsonConvert.SerializeObject(inputList);

        private string[] ConvertElement2StringArray(IPDataResult element) => new string[] { element.ClientIp, string.Join("; ", element.FQDNs), element.NCalls.ToString() };
    }
}
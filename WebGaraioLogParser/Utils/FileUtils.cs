using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebGaraioLogParser.Utils
{

    public class FileUtils
    {
        private struct SortedFile
        {
            public long FileOrder { get; set; }
            public String FileName { get; set; }
        }

        public static string PART_TOKEN => ".part_";

        public static string UPLOADED_FILE_PATH => "~/App_Data/uploads";

        /// <summary>
        /// original name + ".part_N.X" (N = file part number, X = total files)
        /// Objective = enumerate files in folder, look for all matching parts of split file. If found, merge and return true.
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static bool MergeFile(string FileName)
        {
            var rslt = false;

            var baseFileName = FileName.Substring(0, FileName.IndexOf(PART_TOKEN));
            var trailingTokens = FileName.Substring(FileName.IndexOf(PART_TOKEN) + PART_TOKEN.Length);

            var searchPattern = String.Format("{0}{1}*", Path.GetFileName(baseFileName), PART_TOKEN);
            var FilesList = Directory.GetFiles(Path.GetDirectoryName(FileName), searchPattern);

            long.TryParse(trailingTokens.Substring(0, trailingTokens.IndexOf(".")), out long fileIndex);
            long.TryParse(trailingTokens.Substring(trailingTokens.IndexOf(".") + 1), out long fileCount);

            if (FilesList.Count() == fileCount && !FileMergeManager.Instance.InUse(baseFileName))
            {
                FileMergeManager.Instance.AddFile(baseFileName);

                if (File.Exists(baseFileName)) File.Delete(baseFileName);

                var MergeList = new List<SortedFile>();
                foreach (string file in FilesList)
                {
                    var sFile = new SortedFile() { FileName = file };
                    baseFileName = file.Substring(0, file.IndexOf(PART_TOKEN));
                    trailingTokens = file.Substring(file.IndexOf(PART_TOKEN) + PART_TOKEN.Length);
                    long.TryParse(trailingTokens.Substring(0, trailingTokens.IndexOf(".")), out fileIndex);
                    sFile.FileOrder = fileIndex;
                    MergeList.Add(sFile);
                }

                var MergeOrder = MergeList.OrderBy(s => s.FileOrder).ToList();
                using (var FS = new FileStream(baseFileName, FileMode.Create))
                {
                    foreach (var chunk in MergeOrder)
                    {
                        try
                        {
                            using (var fileChunk = new FileStream(chunk.FileName, FileMode.Open))
                            {
                                fileChunk.CopyTo(FS);
                            }
                        }
                        catch { }
                        File.Delete(chunk.FileName);
                    }
                }

                rslt = true;
                FileMergeManager.Instance.RemoveFile(baseFileName);
            }
            return rslt;
        }
    }
}
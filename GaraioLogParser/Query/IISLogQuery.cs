using GaraioLogParser.Model.IISRecord;
using GaraioLogParser.Query.Model;
using GaraioLogParser.Resources;
using System;
using System.IO;

namespace GaraioLogParser.Query
{
    public class IISLogQuery
    {
        public const string SOFTWARE = "#Software:";
        public const string VERSION = "#Version:";
        public const string DATE = "#Date:";
        public const string FIELD = "#Fields:";

        public static IISLogRecordSet Execute(string filename, params string[] filters)
        {
            string myLine;

            IISLogRecord record = null;
            var recordSet = new IISLogRecordSet();

            var filterPositions = new int[filters.Length];

            using (var myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var myStreamReader = new StreamReader(myFileStream))
                {
                    W3CLogFileHeader header = null;

                    while ((myLine = myStreamReader.ReadLine()) != null)
                    {
                        if (myLine.Contains(SOFTWARE))
                        {
                            header = new W3CLogFileHeader();
                            header.ParseSoftwareRow(myLine);
                        }
                        else
                        {
                            if (myLine.Contains(VERSION)) header.ParseVersionRow(myLine);
                            else
                            {
                                if (myLine.Contains(DATE)) header.ParseDateRow(myLine);
                                else
                                {
                                    if (myLine.Contains(FIELD))
                                    {
                                        header.ParseFieldDefinitionRow(myLine);

                                        for (var i = 0; i < filters.Length; i++)
                                        {
                                            if (!header.ContainFilter(filters[i])) throw new FormatException(Resource.WrongFilterRequestedException);
                                            filterPositions[i] = header.GetIndexByField(filters[i]);
                                        }
                                    }
                                    else
                                    {
                                        if (!header.ValidateHeader()) throw new FormatException(Resource.W3CIISFormatException);

                                        record = new IISLogRecord(filters.Length);
                                        for (var i = 0; i < filterPositions.Length; i++) record.AddValue(i, (myLine.Split(' '))[filterPositions[i]]);

                                        recordSet.Add(record);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return recordSet;
        }
    }
}

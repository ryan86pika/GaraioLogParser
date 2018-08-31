using System;
using System.Collections.Generic;
using System.Linq;

namespace GaraioLogParser.Query.Model
{
    public class W3CLogFileHeader
    {
        private string _software;
        private string _version;
        private DateTime _date;
        private List<String> _fields;

        private bool? _validate;

        public W3CLogFileHeader()
        {
            _software = string.Empty;
            _version = string.Empty;
            _date = DateTime.MinValue;
            _fields = new List<String>();
            _validate = null;
        }

        public void ParseSoftwareRow(string sw) => _software = sw.Replace(IISLogQuery.SOFTWARE, string.Empty).TrimStart().TrimEnd();

        public void ParseVersionRow(string v) => _version = v.Replace(IISLogQuery.VERSION, string.Empty).TrimStart().TrimEnd();

        public void ParseDateRow(string d) => _date = DateTime.ParseExact(d.Replace(IISLogQuery.DATE, string.Empty).TrimStart().TrimEnd(), 
                                                                            "yyyy-MM-dd HH:mm:ss",
                                                                            System.Globalization.CultureInfo.InvariantCulture);

        public void ParseFieldDefinitionRow(string fields) => _fields = fields.Replace(IISLogQuery.FIELD, string.Empty).TrimStart().TrimEnd().Split(' ').ToList();

        public bool ContainFilter(string field) => _fields.Contains(field);

        public int GetIndexByField(string field) => _fields.IndexOf(field);

        public bool ValidateHeader()
        {
            if (_validate == null) return !string.IsNullOrEmpty(_software) && !string.IsNullOrEmpty(_version) && !_date.Equals(DateTime.MinValue) && _fields.Count > 0;
            return _validate.Value;
        }
    }
}

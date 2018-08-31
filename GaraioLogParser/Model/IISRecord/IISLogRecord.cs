using GaraioLogParser.Resources;
using System;
using System.Linq;
using Resources = GaraioLogParser.Resources;

namespace GaraioLogParser.Model.IISRecord
{
    public class IISLogRecord : ILogRecord, IEquatable<IISLogRecord>
    {
        private string[] _records;
        private long _counter;

        public IISLogRecord(int nElem)
        {
            _records = new string[nElem];
            _counter = 1;
        }

        public virtual string GetValue(int pos)
        {
            if (pos < _records.Length) return _records[pos];
            else if (pos == _records.Length) return _counter.ToString();
            return string.Empty;
        }

        public void AddValue(int index, string value)
        {
            if (index >= _records.Length) throw new IndexOutOfRangeException(Resource.RecordIndexOutOfRangeException);
            else if (value.Equals("-")) value = string.Empty;
            _records[index] = value;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            foreach (string s in _records) hash = hash * 23 + s.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj) => obj is IISLogRecord && Equals((IISLogRecord)obj);

        public bool Equals(IISLogRecord p) => _records.Length == p._records.Length && _records.SequenceEqual(p._records);

        public void IncrementCounter() => _counter++;
    }
}

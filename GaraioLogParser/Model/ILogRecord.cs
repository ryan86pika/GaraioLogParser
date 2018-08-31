using System;
using System.Collections.Generic;
using System.Linq;

namespace GaraioLogParser.Model
{
    public interface ILogRecord
    {
        void AddValue(int index, string value);
        string GetValue(int pos);
        void IncrementCounter();
    }
}

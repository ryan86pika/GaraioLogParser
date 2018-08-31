using System;
using System.Collections.Generic;
using System.Linq;

namespace GaraioLogParser.Model
{
    public interface ILogRecordSet
    {
        ILogRecord Element { get; }
        ILogRecordSet NextElement { get; }
        bool IsLastElement { get; }

        bool Add(ILogRecord newItem);
    }
}

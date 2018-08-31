using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGaraioLogParser.Models
{
    public class ExtractDataResult
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
    }
}
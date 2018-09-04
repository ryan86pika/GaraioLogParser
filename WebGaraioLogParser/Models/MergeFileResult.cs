using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebGaraioLogParser.Models
{
    public class MergeFileResult
    {
        public bool Success { get; set; } = false;
        public string BaseFileName { get; set; } = string.Empty;
        public long Chunk { get; set; } = 0;
        public long MaxChunks { get; set; } = 0;
        public string Message { get; set; } = string.Empty;
    }
}
using System.Collections.Generic;

namespace WebGaraioLogParser.Utils
{
    public class FileMergeManager
    {
        private List<string> _mergeFileList;

        private static FileMergeManager _instance;

        private FileMergeManager()
        {
            _mergeFileList = new List<string>();
        }

        public static FileMergeManager Instance => _instance = _instance ?? new FileMergeManager();

        public void AddFile(string BaseFileName) => _mergeFileList.Add(BaseFileName);

        public bool InUse(string BaseFileName) => _mergeFileList.Contains(BaseFileName);

        public bool RemoveFile(string BaseFileName) => _mergeFileList.Remove(BaseFileName);
    }
}
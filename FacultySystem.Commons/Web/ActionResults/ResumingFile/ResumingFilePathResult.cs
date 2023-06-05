using System;
using System.IO;

namespace ContentManagementSystem.Commons.Web.ActionResults.ResumingFile
{
    public class ResumingFilePathResult : ResumingActionResultBase
    {
        private FileInfo MediaFile { get; set; }

        public ResumingFilePathResult(string fileName, string contentType, string fileDispositionType = "attachment")
            : base(contentType, fileDispositionType, fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException();

            MediaFile = new FileInfo(fileName);
            LastModified = MediaFile.LastWriteTime;
            FileContents = MediaFile.OpenRead();
        }
    }
}

using System;
using System.IO;

namespace ContentManagementSystem.Commons.Web.ActionResults.ResumingFile
{
    public class ResumingFileStreamResult : ResumingActionResultBase
    {
        public ResumingFileStreamResult(Stream fileStream, string contentType)
            : base(contentType)
        {
            if (fileStream == null)
                throw new ArgumentNullException("fileStream");

            FileContents = fileStream;
        }
    }
}

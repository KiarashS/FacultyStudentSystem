using System;
using System.IO;

namespace ContentManagementSystem.Commons.Web.ActionResults.ResumingFile
{
    public class ResumingFileContentResult : ResumingActionResultBase
    {
        public ResumingFileContentResult(byte[] fileContents, string contentType)
            : base(contentType)
        {
            if (fileContents == null)
                throw new ArgumentNullException("fileContents");

            FileContents = new MemoryStream(fileContents);
        }
    }
}

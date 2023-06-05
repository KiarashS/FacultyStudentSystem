
using ContentManagementSystem.Models.Utils;
using System;

namespace ContentManagementSystem.Models.ViewModels
{
    public class ExternalResearchRecordViewModel
    {
        private string fileText;

        public long Id { get; set; }
        public int UserId { get; set; }
        public string Doi { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Journal { get; set; }
        public string Volume { get; set; }
        public string Issue { get; set; }
        public string Pages { get; set; }
        public int? Year { get; set; }
        public int? TimesCited { get; set; }
        public string Link { get; set; }
        public string Filename { get; set; }
        public string Abstract { get; set; }
        public string Description { get; set; }
        public int? Order { get; set; }
        public DateTime? ExternalResearchLastUpdateTime { get; set; }

        public string FileText
        {
            get
            {
                if (fileText != null)
                {
                    return fileText;
                }

                if (string.IsNullOrEmpty(Filename) || UserId == 0)
                {
                    fileText = null;
                    return fileText;
                }

                fileText = RijndaelManagedEncryption.EncryptRijndael(UserId.ToString() + ";#;" + Filename);
                return fileText;
            }
            set
            {
                fileText = value;
            }
        }

        public string GetRelativeExternalResearchLastUpdateTime
        {
            get
            {
                if (ExternalResearchLastUpdateTime == null)
                {
                    return string.Empty;
                }

                return (ExternalResearchLastUpdateTime ?? DateTime.UtcNow).UtcToLocalDateTime().CalculateRelativeTime();
            }
        }
    }
}

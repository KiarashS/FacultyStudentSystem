
using ContentManagementSystem.Models.Utils;

namespace ContentManagementSystem.Models.ViewModels
{
    public class InternalSeminarRecordViewModel
    {
        private string fileText;

        public long Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Conference { get; set; }
        public string Date { get; set; }
        public string Link { get; set; }
        public string Filename { get; set; }
        public string Description { get; set; }
        public string Abstract { get; set; }
        public int? Order { get; set; }

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
    }
}

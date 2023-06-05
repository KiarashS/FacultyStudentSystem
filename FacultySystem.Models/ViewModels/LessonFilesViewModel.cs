using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class LessonFilesViewModel
    {
        private string fileText;
        private string coverText;
        public LessonFilesViewModel()
        {
            CreateDate = DateTime.UtcNow;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string CoverFilename { get; set; }
        public string Filename { get; set; }
        public string FileLink { get; set; }
        public FileType FileType { get; set; }
        public bool DeleteFile { get; set; }
        public bool DeleteCover { get; set; }
        public long LessonId { get; set; }
        public int UserId { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        public string FileTypeText
        {
            get
            {
                return EnumExtensions.GetDescription(FileType);
            }
        }

        public string CreateDateText
        {
            get
            {
                var persianDate = "-";
                PersianCalendar pc = new PersianCalendar();
                var localDate = ((DateTime)CreateDate).UtcToLocalDateTime();
                persianDate = string.Format("{0}/{1}/{2}", pc.GetYear(localDate), pc.GetMonth(localDate), pc.GetDayOfMonth(localDate));
                return persianDate;
            }
        }

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

        public string CoverText
        {
            get
            {
                if (coverText != null)
                {
                    return coverText;
                }

                if (string.IsNullOrEmpty(CoverFilename) || UserId == 0)
                {
                    coverText = null;
                    return coverText;
                }

                coverText = RijndaelManagedEncryption.EncryptRijndael(UserId.ToString() + ";#;" + CoverFilename);
                return coverText;
            }
            set
            {
                coverText = value;
            }
        }

        public Dictionary<string, string> FilesLinkText
        {
            get
            {
                if (string.IsNullOrEmpty(FileLink))
                {
                    return null;
                }

                var links = FileLink.Split(',');
                var linksText = new Dictionary<string, string>();
                var number = 1;

                foreach (var link in links)
                {
                    var validLink = link.ToLowerInvariant().Trim();

                    if (!validLink.StartsWith("http://") && !validLink.StartsWith("https://") && !validLink.StartsWith("ftp://"))
                    {
                        validLink = "http://" + validLink;
                    }

                    linksText.Add($"لینک {number}", validLink);
                    number ++;
                }

                return linksText;
            }
        }
    }
}

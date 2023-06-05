using System;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.Utils;
using System.Globalization;

namespace ContentManagementSystem.Models.ViewModels
{
    public class GalleryItemViewModel
    {
        private string fileText;
        //private string coverText;
        public GalleryItemViewModel()
        {
            CreateDate = DateTime.UtcNow;
        }

        public long Id { get; set; }
        public int UserId { get; set; }
        public long GalleryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string MediaFilename { get; set; }
        //public string MediaLink { get; set; }
        //public string MediaDate { get; set; }
        public MediaType MediaType { get; set; }
        //public bool DeleteFile { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        public string MediaTypeText
        {
            get
            {
                return EnumExtensions.GetDescription(MediaType);
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

                if (string.IsNullOrEmpty(MediaFilename) || UserId == 0)
                {
                    fileText = null;
                    return fileText;
                }

                fileText = RijndaelManagedEncryption.EncryptRijndael(UserId.ToString() + ";#;" + GalleryId.ToString() + @"\" + MediaFilename);
                return fileText;
            }
            set
            {
                fileText = value;
            }
        }
    }
}

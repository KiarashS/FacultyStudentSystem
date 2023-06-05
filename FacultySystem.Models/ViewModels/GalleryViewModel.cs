using ContentManagementSystem.Models.Utils;
using System;
using System.Globalization;

namespace ContentManagementSystem.Models.ViewModels
{
    public class GalleryViewModel
    {
        public GalleryViewModel()
        {
            CreateDate = DateTime.UtcNow;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        //public string CoverFilename { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

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
    }
}

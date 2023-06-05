using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class LessonImportantDateViewModel
    {
        public LessonImportantDateViewModel()
        {
            CreateDate = DateTime.UtcNow;
        }

        public long Id { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public DateDay DateDay { get; set; }
        public long LessonId { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        public string DateDayText
        {
            get
            {
                return EnumExtensions.GetDescription(DateDay);
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
    }
}

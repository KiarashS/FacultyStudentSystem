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
    public class PracticeClassInfoViewModel
    {
        public PracticeClassInfoViewModel()
        {
            CreateDate = DateTime.UtcNow;
        }

        public long Id { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string TeacherName { get; set; }
        public PracticeClassDay PracticeClassDay { get; set; }
        public long LessonId { get; set; }
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

        public string PracticeClassDayText
        {
            get
            {
                return EnumExtensions.GetDescription(PracticeClassDay);
            }
        }
    }
}
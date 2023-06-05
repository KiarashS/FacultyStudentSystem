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
    public class LessonViewModel
    {
        public LessonViewModel()
        {
            CreateDate = DateTime.UtcNow;
        }

        public long Id { get; set; }
        public string LessonCode { get; set; }
        public int? GroupNumber { get; set; }
        public string LessonName { get; set; }
        public string Description { get; set; }
        public string ScoringDescription { get; set; }
        public string ProjectDescription { get; set; }
        public LessonGrade LessonGrade { get; set; }
        public string Field { get; set; }
        public string Trend { get; set; }
        public string AcademicYear { get; set; }
        public string Semester { get; set; }
        public LessonType LessonType { get; set; }
        public LessonState LessonState { get; set; }
        public UnitState UnitState { get; set; }
        public float? UnitNumber { get; set; }
        public DateTime CreateDate { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }
        public string Reference { get; set; }

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

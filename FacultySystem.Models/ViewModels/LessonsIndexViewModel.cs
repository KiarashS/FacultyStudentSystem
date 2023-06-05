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
    public class LessonsIndexViewModel
    {
        public int LessonsCount { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public IList<UserLessonsViewModel> UserLessons { get; set; }

        public string Fullname
        {
            get
            {
                if (!string.IsNullOrEmpty(Firstname) && !string.IsNullOrEmpty(Lastname))
                {
                    return $"{Firstname} {Lastname}";
                }

                if (!string.IsNullOrEmpty(Firstname))
                {
                    return Firstname;
                }

                if (!string.IsNullOrEmpty(Lastname))
                {
                    return Lastname;
                }

                return null;
            }
        }
    }

    public class UserLessonsViewModel
    {
        public long LessonId { get; set; }
        public string LessonName { get; set; }
        public string Description { get; set; }
        public LessonGrade LessonGrade { get; set; }
        public string Field { get; set; }
        public string Trend { get; set; }
        public string AcademicYear { get; set; }
        public string Semester { get; set; }
        public string Link { get; set; }
        public DateTime CreateDate { get; set; }

        public string LessonGradeText
        {
            get
            {
                return EnumExtensions.GetDescription(LessonGrade);
            }
        }

        public string UrlSlug
        {
            get
            {
                return Utils.Extensions.GenerateSlug(LessonName);
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

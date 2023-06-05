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
    public class LessonIndexViewModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public long LessonId { get; set; }
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
        public bool HasLessonClassInfo { get; set; }
        public bool HasPracticeClassInfo { get; set; }
        public bool HasLessonNews { get; set; }
        public bool HasLessonFile { get; set; }
        public bool HasImportantDate { get; set; }
        public bool HasLessonPractice { get; set; }
        public bool HasLessonScore { get; set; }
        public string Reference { get; set; }

        public string LessonTypeText
        {
            get
            {
                return EnumExtensions.GetDescription(LessonType);
            }
        }

        public string LessonStateText
        {
            get
            {
                return EnumExtensions.GetDescription(LessonState);
            }
        }

        public string UnitStateText
        {
            get
            {
                return EnumExtensions.GetDescription(UnitState);
            }
        }

        public string LessonGradeText
        {
            get
            {
                return EnumExtensions.GetDescription(LessonGrade);
            }
        }

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

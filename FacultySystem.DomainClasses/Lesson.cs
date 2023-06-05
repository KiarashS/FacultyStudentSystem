using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class Lesson : DomainClassBase
    {
        public Lesson()
        {
            CreateDate = DateTime.UtcNow;
        }

        [Key]
        public long Id { get; set; }
        [ForeignKey("ProfessorDetails")]
        public int ProfessorId { get; set; }
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

        #region Navigations
        public virtual Professor ProfessorDetails { get; set; }
        public virtual ICollection<LessonNews> LessonNews { get; set; }
        public virtual ICollection<LessonPractices> LessonPractices { get; set; }
        public virtual ICollection<LessonScores> LessonScores { get; set; }
        public virtual ICollection<LessonImportantDate> ImportantDates { get; set; }
        public virtual ICollection<LessonFiles> LessonFiles { get; set; }
        public virtual ICollection<LessonClassInfo> LessonClassInfos { get; set; }
        public virtual ICollection<PracticeClassInfo> PracticeClassInfos { get; set; }
        #endregion
    }

    public enum LessonGrade : byte
    {
        [Description("--")]
        NotDefined = 1,
        [Description("کاردانی")]
        Kardani = 2,
        [Description("کارشناسی")]
        Karshenasi = 3,
        [Description("پزشکی عمومی")]
        PezeshkiOmoomi = 4,
        [Description("کارشناسی ارشد")]
        KarshenasiArshad = 5,
        [Description("دکترا")]
        Doctora = 6,
        [Description("دکترای تخصصی پزشکی")]
        DrTakhassosiPezeshki = 7,
        [Description("دکترای حرفه ای")]
        DoctorayeHerfeei = 8,
        [Description("دکترای فوق تخصصی بالینی")]
        DrTakhassosiBalini = 9,
        [Description("دکترای تکمیلی تخصصی (فلوشیپ)")]
        Fellowship = 10,
        [Description("دکترای تخصصی دندانپزشکی")]
        DrTakhassosiDandanpezeshki = 11,
        [Description("دکترای تخصصی (PhD) داروسازی")]
        DrTakhassosiDaroosazi = 12,
        [Description("دکترای تخصصی (PhD)")]
        DrTakhassosi = 13,
        [Description("دستیاری تخصصی (علوم پایه پزشکی، داروسازی و دندانپزشکی)")]
        DastyariTakhassosi = 14,
        [Description("دستیاری تخصصی بالینی")]
        DastyariTakhassosiBalini = 15,
        [Description("پسادکترا")]
        PasaDoctora = 16,
        [Description("دوره MPH")]
        Mph = 17,
        [Description("دانشوری")]
        Daneshvari = 18
    }

    public enum LessonType : byte
    {
        [Description("--")]
        NotDefined = 1,
        [Description("تخصصی")]
        Takhassosi = 2,
        [Description("عمومی")]
        Omoomi = 3
    }

    public enum LessonState : byte
    {
        [Description("--")]
        NotDefined = 1,
        [Description("اجباری")]
        Ejbari = 2,
        [Description("اختیاری")]
        Ekhtiari = 3
    }

    public enum UnitState : byte
    {
        [Description("--")]
        NotDefined = 1,
        [Description("نظری")]
        Nazari = 2,
        [Description("عملی")]
        Amali = 3
    }
}

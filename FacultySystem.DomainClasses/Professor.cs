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
    public class Professor : DomainClassBase
    {
        public Professor()
        {
            IsApproved = true;
            IsActiveBio = false;
            IsActiveFreePage = false;
            ShowHIndexSection = true;
            IsActiveWeeklyProgram = true;
            ShowScopusDocumentsCitationChart = true;
            ShowGoogleDocumentsCitationChart = true;
            Sex = Sex.NotDefined;
            MaritalStatus = MaritalStatus.NotDefined;
            //AcademicRank = AcademicRank.NotDefined;
            //EducationalDegree = EducationalDegree.NotDefined;
            //EducationalGroup = EducationalGroup.NotDefined;
            //College = College.NotDefined;
        }

        [Key]
        [ForeignKey("UserDetails")]
        public int UserId { get; set; }
        public string PageId { get; set; }
        public string CommonAuthorPaperName { get; set; }
        public string SecondaryEmails { get; set; }
        public Sex Sex { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public int AcademicRankId { get; set; }
        public int EducationalDegreeId { get; set; }
        public int EducationalGroupId { get; set; }
        public int CollegeId { get; set; }
        public string Mobile { get; set; }
        public string Location { get; set; }
        public string ResearchFields { get; set; }
        public string Interests { get; set; }
        public string PersonalWebPage { get; set; }
        public string PersianResumeFileName { get; set; }
        public string EnglishResumeFileName { get; set; }
        public string ScopusId { get; set; }
        public string OrcidId { get; set; }
        public string ResearchGateId { get; set; }
        public string GoogleScholarId { get; set; }
        public string ResearcherId { get; set; }
        public string PubMedId { get; set; }
        public string MedLibId { get; set; }
        public string Bio { get; set; }
        public string BirthPlace { get; set; }
        public bool IsBanned { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public bool IsApproved { get; set; } // ?!
        public bool IsSoftDelete { get; set; }
        public bool IsActiveBio { get; set; }
        public DateTime? BannedDate { get; set; }
        public DateTime? SoftDeleteDate { get; set; }
        public string BannedReason { get; set; }
        public string Avatar { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? ScopusHIndex { get; set; }
        public int? GoogleHIndex { get; set; }
        //public int? ScopusCitations { get; set; }
        public int? ScopusDocuments { get; set; }
        //public int? ScopusTotalDocumentsCited { get; set; } // تعداد کل مقالاتی که مقالات  این نویسنده در آنها سایت شده
        public int? GoogleCitations { get; set; }
        public string OtherNamesFormat { get; set; } // اسامی دیگر در مقالات
        //public bool IsActiveDashboardAnnouncement { get; set; }
        //public string DashboardAnnouncement { get; set; }
        public string FreePage { get; set; }
        public bool IsActiveFreePage { get; set; }
        public bool ShowScopusDocumentsCitationChart { get; set; }
        public bool ShowGoogleDocumentsCitationChart { get; set; }
        public bool ShowHIndexSection { get; set; }
        public DateTime? FreePageLastUpdateTime { get; set; }
        public string WeeklyProgramStartDate { get; set; }
        public string WeeklyProgramEndDate { get; set; }
        public string WeeklyProgramDescription { get; set; }
        public bool IsActiveWeeklyProgram { get; set; }
        public DateTime? HIndexAndDocumentCitationLastUpdateTime { get; set; }
        public DateTime? ExternalResearchLastUpdateTime { get; set; }

        #region Navigations
        public virtual User UserDetails { get; set; }
        [ForeignKey("EducationalDegreeId")]
        public virtual EducationalDegree EducationalDegree { get; set; }
        [ForeignKey("CollegeId")]
        public virtual College College { get; set; }
        [ForeignKey("EducationalGroupId")]
        public virtual EducationalGroup EducationalGroup { get; set; }
        [ForeignKey("AcademicRankId")]
        public virtual AcademicRank AcademicRank { get; set; }
        //public virtual ICollection<ActivityLog> ActivityLogs { get; set; }
        public virtual ICollection<WeeklyProgram> WeeklyPrograms { get; set; } // برنامه هفتگی
        public virtual ICollection<AdminMessage> AdminMessages { get; set; } // پیام های ارسالی به مدیر سیستم
        public virtual ICollection<DocumentCitation> DocumentCitations { get; set; } // تعداد ارجاع و مقالات در هر سال
        public virtual ICollection<Gallery> Galleries { get; set; } // گالری ها
        public virtual ICollection<GalleryItem> GalleryItems { get; set; } // فایل های گالری
        public virtual ICollection<Lesson> Lessons { get; set; } // دروس ارائه شده
        public virtual ICollection<LessonNews> LessonNews { get; set; } // اخبار درس
        public virtual ICollection<LessonPractices> LessonPractices { get; set; } // تمرینات درس
        public virtual ICollection<LessonScores> LessonScores { get; set; } // نمرات درس
        public virtual ICollection<LessonImportantDate> ImportantDates { get; set; } // تاریخ های مهم درس
        public virtual ICollection<LessonFiles> LessonFiles { get; set; } // فایل های درس
        public virtual ICollection<LessonClassInfo> LessonClassInfos { get; set; } // اطلاعات تشکیل کلاس درسی
        public virtual ICollection<PracticeClassInfo> PracticeClassInfos { get; set; } // اطلاعات کلاس حل تمرین
        public virtual ICollection<SectionOrder> SectionOrders { get; set; } // ترتیب نمایش بخش ها
        public virtual ICollection<FreeField> FreeFields { get; set; } // فیلدهای آزاد
        public virtual ICollection<Address> Addresses { get; set; } // آدرس ها
        public virtual ICollection<TrainingRecord> TrainingRecords { get; set; } // سوابق آموزشی 
        public virtual ICollection<ProfessorMembership> Memberships { get; set; } // عضویت ها 
        public virtual ICollection<ResearchRecord> ResearchRecords { get; set; } // سوابق پژوهشی
        public virtual ICollection<StudingRecord> StudingRecords { get; set; } // سوابق تحصیلی
        public virtual ICollection<AdministrationRecord> AdministrationRecords { get; set; } // سوابق اجرایی و مدیریتی
        public virtual ICollection<InternalResearchRecord> InternalResearchRecords { get; set; } // چاپ مقالات در مجلات معتبر داخلی
        public virtual ICollection<ExternalResearchRecord> ExternalResearchRecords { get; set; } // چاپ مقالات در مجلات معتبر خارجی
        public virtual ICollection<InternalSeminarRecord> InternalSeminarRecords { get; set; } // ارائه مقالات در سمینارها و کنگره های داخلی
        public virtual ICollection<ExternalSeminarRecord> ExternalSeminarRecords { get; set; } // ارائه مقالات در سمینارها و کنگره های خارجی
        public virtual ICollection<Thesis> Thesis { get; set; } // پایان نامه‌ها
        public virtual ICollection<CourseAndWorkshop> CourseAndWorkshops { get; set; } // دوره های آموزشی و کارگاه‌ها
        public virtual ICollection<Honor> Honors { get; set; } // جوایز و افتخارات
        public virtual ICollection<Publication> Publications { get; set; } // تدوین ها و تالیفات
        public virtual ICollection<Language> Languages { get; set; } // زبان ها 

        #endregion
    }


    public enum Sex : byte
    {
        [Description("--")]
        NotDefined = 1,
        [Description("مرد")]
        Male = 2,
        [Description("زن")]
        Female = 3
    }

    public enum MaritalStatus : byte
    {
        [Description("--")]
        NotDefined = 1,
        [Description("مجرد")]
        Single = 2,
        [Description("متاهل")]
        Married = 3
    }

    //public enum AcademicRank : byte
    //{
    //    [Description("--")]
    //    NotDefined = 1,
    //    [Description("مربی")]
    //    Instructor = 2,
    //    [Description("استادیار")]
    //    AssistantProfessor = 3,
    //    [Description("دانشیار")]
    //    AssociateProfessor = 4,
    //    [Description("استاد")]
    //    Professor = 5,
    //    [Description("استاد تمام")]
    //    FullProfessor = 6
    //}

    //public enum EducationalDegree : byte
    //{
    //    [Description("--")]
    //    NotDefined = 1,
    //    [Description("دکترای تخصصی")]
    //    PHD = 2
    //}

    //public enum EducationalGroup : byte
    //{
    //    [Description("--")]
    //    NotDefined = 1,
    //    [Description("جراحی")]
    //    Surgery = 2
    //}

    //public enum College : byte
    //{
    //    [Description("--")]
    //    NotDefined = 1,
    //    [Description("فنی و مهندسی")]
    //    Engineering = 2,
    //    [Description("معماری")]
    //    Architecture = 3,
    //    [Description("ادبیات")]
    //    Literature = 4,
    //}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class LessonFiles : DomainClassBase
    {
        public LessonFiles()
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
        [ForeignKey("ProfessorDetails")]
        public int ProfessorId { get; set; }
        [ForeignKey("LessonDetails")]
        public long LessonId { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        #region Navigations
        public virtual Professor ProfessorDetails { get; set; }
        public virtual Lesson LessonDetails { get; set; }
        #endregion
    }

    public enum FileType : byte
    {
        //[Description("--")]
        //NotDefined = 1,
        [Description("اسلاید")]
        Slide = 1,
        [Description("کتاب")]
        Ebook = 2,
        [Description("نمونه سوال")]
        SampleQuestion = 3,
        [Description("نرم افزار")]
        Application = 4,
        [Description("نرم افزار کمک آموزشی")]
        HelperApplication = 5,
        [Description("ویدئو")]
        Video = 6,
        [Description("فایل صوتی")]
        Audio = 7,
        [Description("تصویر")]
        Image = 8,
        [Description("مقاله")]
        Article = 9,
        [Description("پایان نامه")]
        Thesis = 10,
        [Description("استاندارد")]
        Standard = 11,
        [Description("لینک/سایت")]
        Link = 12,
        [Description("فایل کمکی")]
        UtilityFile = 13,
        [Description("فایل متنی")]
        TextFile = 14,
        [Description("فایل راهنما")]
        HelperFile = 15,
        [Description("غیره")]
        Etcetera = 16,
    }
}

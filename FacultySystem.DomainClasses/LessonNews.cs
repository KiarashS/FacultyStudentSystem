using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class LessonNews: DomainClassBase
    {
        public LessonNews()
        {
            CreateDate = DateTime.UtcNow;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public NewsColor NewsColor { get; set; }
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

    public enum NewsColor: byte
    {
        [Description("آبی")]
        Primary = 1,
        [Description("سبز")]
        Success = 2,
        [Description("طوسی")]
        Default = 3,
        [Description("نارنجی")]
        Warning = 4,
        [Description("قرمز")]
        Danger = 5
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class LessonPractices: DomainClassBase
    {
        public LessonPractices()
        {
            CreateDate = DateTime.UtcNow;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Filename { get; set; }
        public DateTime CreateDate { get; set; }
        public string DeliverDate { get; set; }
        public string FileLink { get; set; }
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
}

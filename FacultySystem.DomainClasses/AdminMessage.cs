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
    public class AdminMessage : DomainClassBase
    {
        public AdminMessage()
        {
            CreateDate = DateTime.UtcNow;
            State = AdminMessageState.Posted;
        }

        [Key]
        public long Id { get; set; }
        [ForeignKey("ProfessorDetails")]
        public int ProfessorId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ReplyContent { get; set; }
        public AdminMessageState State { get; set; }
        public DateTime CreateDate { get; set; }

        #region Navigations
        public virtual Professor ProfessorDetails { get; set; }
        #endregion
    }

    public enum AdminMessageState: byte
    {
        [Description("ارسال شده")]
        Posted = 1,
        [Description("انجام شده")]
        Done = 2,
        [Description("بسته شده")]
        Terminated = 3
    }
}

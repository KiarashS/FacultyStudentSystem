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
    public class ActivityLog : DomainClassBase
    {
        public ActivityLog()
        {
            ActionDate = DateTime.UtcNow;
            ActionLevel = ActionLevel.Low;
        }

        [Key]
        public long Id { get; set; }
        //[ForeignKey("ProfessorDetails")]
        //public int UserId { get; set; }
        public string SourceAddress { get; set; } // IP Address
        public string ActionBy { get; set; } // Username
        public string ActionType { get; set; }
        public string Message { get; set; }
        public ActionLevel ActionLevel { get; set; }
        public DateTime ActionDate { get; set; }
        //public string OriginalValues { get; set; }
        //public string NewValues { get; set; }
        public string Url { get; set; }
        //public bool IsActionSucceeded { get; set; }

        //#region Navigations
        //public virtual Professor ProfessorDetails { get; set; }
        //#endregion
    }

    public enum ActionLevel : byte
    {
        [Description("کم")]
        Low = 1,
        [Description("متوسط")]
        Medium = 2,
        [Description("زیاد")]
        High = 3
    }
}

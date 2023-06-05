using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class StudingRecord : DomainClassBase
    {
        public long Id { get; set; }
        public int ProfessorId { get; set; }
        public string Grade { get; set; }
        public string Field { get; set; }
        public string Trend { get; set; }
        public string University { get; set; }
        public int? StartTime { get; set; }
        public int? EndTime { get; set; }
        public string ThesisTitle { get; set; }
        public string ThesisSupervisors { get; set; }
        public string ThesisAdvisors { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        [ForeignKey("ProfessorId")]
        public virtual Professor ProfessorProfile { get; set; }
    }
}

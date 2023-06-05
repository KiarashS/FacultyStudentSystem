using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class TrainingRecord: DomainClassBase
    {
        public long Id { get; set; }
        public int ProfessorId { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public int? Time { get; set; } // Year
        public int? FromTime { get; set; } // Year
        public int? ToTime { get; set; } // Year
        public string Teacher { get; set; }
        public string Participant { get; set; }
        public string Secretary { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        [ForeignKey("ProfessorId")]
        public virtual Professor ProfessorProfile { get; set; }
    }
}

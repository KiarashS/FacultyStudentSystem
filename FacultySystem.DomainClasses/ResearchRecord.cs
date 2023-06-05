using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class ResearchRecord : DomainClassBase
    {
        public long Id { get; set; }
        public int ProfessorId { get; set; }
        public string Title { get; set; }
        public string Place { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        [ForeignKey("ProfessorId")]
        public virtual Professor ProfessorProfile { get; set; }
    }
}

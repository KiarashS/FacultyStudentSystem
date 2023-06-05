using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class Honor : DomainClassBase
    {
        public long Id { get; set; }
        public int ProfessorId { get; set; }
        public string Title { get; set; }
        public int? Time { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        [ForeignKey("ProfessorId")]
        public virtual Professor ProfessorProfile { get; set; }
    }
}

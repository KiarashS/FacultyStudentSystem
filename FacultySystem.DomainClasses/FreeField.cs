using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class FreeField : DomainClassBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int? Order { get; set; }
        public int ProfessorId { get; set; }

        [ForeignKey("ProfessorId")]
        public virtual Professor Professor { get; set; }
    }
}

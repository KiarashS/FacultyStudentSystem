using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class AcademicRank : DomainClassBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Order { get; set; }

        public virtual ICollection<Professor> Professors { get; set; }
    }
}

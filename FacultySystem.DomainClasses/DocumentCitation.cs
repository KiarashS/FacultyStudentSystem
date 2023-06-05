using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class DocumentCitation: DomainClassBase
    {
        public long Id { get; set; }
        public int Citation { get; set; }
        public int Year { get; set; }
        public int? Document { get; set; }
        public int ProfessorId { get; set; }
        public DocSource Source { get; set; }

        [ForeignKey("ProfessorId")]
        public virtual Professor ProfessorProfile { get; set; }
    }

    public enum DocSource: byte
    {
        [Description("اسکاپوس")]
        Scopus = 1,
        [Description("گوگل اسکولار")]
        Google = 2
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class InternalSeminarRecord : DomainClassBase
    {
        public long Id { get; set; }
        public int ProfessorId { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string Conference { get; set; }
        public string Date { get; set; }
        public string Link { get; set; }
        public string Filename { get; set; }
        public string Description { get; set; }
        public string Abstract { get; set; }
        public int? Order { get; set; }

        [ForeignKey("ProfessorId")]
        public virtual Professor ProfessorProfile { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class Address : DomainClassBase
    {
        [Key]
        public int AddressId { get; set; }
        [ForeignKey("ProfessorDetails")]
        public int ProfessorId { get; set; }
        public string PostalAddress { get; set; }
        public string PostalCode { get; set; }
        public string Tell { get; set; }
        public string Fax { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        #region Navigations
        public virtual Professor ProfessorDetails { get; set; }
        #endregion
    }
}

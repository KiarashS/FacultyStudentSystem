using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class AddressListViewModel
    {
        public int AddressId { get; set; }
        public string PostalAddress { get; set; }
        public string PostalCode { get; set; }
        public string Tel { get; set; }
        public string Fax { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }
    }
}

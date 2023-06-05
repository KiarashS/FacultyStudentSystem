using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class ProfileValidationViewModel
    {
        public string PageId { get; set; }
        public bool IsValidPageId {get;set;}
        public bool IsSoftDelete { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}

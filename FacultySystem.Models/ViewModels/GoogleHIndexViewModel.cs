using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentManagementSystem.DomainClasses;

namespace ContentManagementSystem.Models.ViewModels
{
    public class GoogleHIndexViewModel
    {
        public int? GoogleHIndex { get; set; }
        public int? GoogleCitations { get; set; }
        public string Value { get; set; }
        public int? Order { get; set; }
        public IList<DocumentCitation> DocumentsCitation { get; set; }
    }
}

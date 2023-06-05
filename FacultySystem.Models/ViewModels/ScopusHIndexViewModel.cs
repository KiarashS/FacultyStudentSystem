using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentManagementSystem.DomainClasses;

namespace ContentManagementSystem.Models.ViewModels
{
    public class ScopusHIndexViewModel
    {
        public int? ScopusHIndex { get; set; }
        public int? ScopusCitations { get; set; }
        public int? ScopusDocuments { get; set; }
        public int? ScopusTotalDocumentsCited { get; set; }
        public string OtherNamesFormat { get; set; }
        public IList<DocumentCitation> DocumentsCitation { get; set; }
    }
}

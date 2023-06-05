using ContentManagementSystem.DomainClasses;
using System.ComponentModel;

namespace ContentManagementSystem.Models.ViewModels
{
    public class DocumentCitationViewModel
    {
        public long Id { get; set; }
        public int Citation { get; set; }
        public int Year { get; set; }
        public int? Document { get; set; }
        public DocSource Source { get; set; }
    }
}

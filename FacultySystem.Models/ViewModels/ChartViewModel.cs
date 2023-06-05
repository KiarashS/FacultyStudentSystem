using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class ScopusChartViewModel
    {
        public ScopusChartViewModel()
        {
            Years = new List<int>();
            Citations = new List<int>();
            Documents = new List<int?>();
        }

        //public string Fullname { get; set; }
        public IList<int> Years { get; set; }
        public IList<int> Citations { get; set; }
        public IList<int?> Documents { get; set; }
    }

    public class GoogleChartViewModel
    {
        public GoogleChartViewModel()
        {
            Years = new List<int>();
            Citations = new List<int>();
        }

        //public string Fullname { get; set; }
        public IList<int> Years { get; set; }
        public IList<int> Citations { get; set; }
    }
}

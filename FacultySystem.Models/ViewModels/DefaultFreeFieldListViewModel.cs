using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class DefaultFreeFieldListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int? Order { get; set; }

        public bool AddToUsers { get; set; }
    }
}

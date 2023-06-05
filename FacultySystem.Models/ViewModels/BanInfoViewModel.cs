using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class BanInfoViewModel
    {
        public bool IsBanned { get; set; }
        public string BannedReason { get; set; }
        public DateTime? BannedDate { get; set; }
        public bool IsSoftDelete { get; set; }
    }
}

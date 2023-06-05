using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class LogonViewModel
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string[] Roles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class UserTopMenuViewModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Fullname { get { return $"{Firstname} {Lastname}"; } }
        //public string UserImageName { get; set; }
        //public string UserImageLocation { get { return $@"App_Data\UsersImage\{UserImageName}"; } }
        public string PageId { get; set; }
    }
}

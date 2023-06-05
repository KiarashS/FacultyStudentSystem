using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Web.Controllers
{
    public partial class BaseController : Controller
    {
        private string UserName()
        {
            return User.Identity.IsAuthenticated ? User.Identity.Name.Split(new[] { ",#;" }, StringSplitOptions.None)[0] : null;
        }

        private int UserId()
        {
            return User.Identity.IsAuthenticated ? int.Parse(User.Identity.Name.Split(new[] { ",#;" }, StringSplitOptions.None)[1]) : 0;
        }

        public string CurrentUserName
        {
            get { return UserName(); }
        }

        public int CurrentUserId
        {
            get { return UserId(); }
        }
    }
}
using System;
using System.Configuration;
using System.Web.Mvc;

namespace ContentManagementSystem.Web.Controllers
{
    public partial class ByController : Controller
    {
        public virtual ActionResult Index()
        {
            var showBy = Convert.ToBoolean(ConfigurationManager.AppSettings["ShowBy"]);
            if (showBy)
            {
                var result = "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><meta content=\"width=device-width\" name=\"viewport\"><title>Designed and Developed by Kiarash Soleimanzadeh</title><style>div{direction:rtl;text-align:center;font-family:tahoma,arial;width:400px;margin:50px auto}</style></head><body><div><div style=\"padding:20px 10px; border: 2px solid #eee;background-color:#fcfcfc;\">Designed and Developed by <a href=\"http://www.kiarash.pro\" target=\"_blank\" title=\"kiarash.s@hotmail.com\">Kiarash Soleimanzadeh</a></div><a href=\"/\">بازگشت به سامانه</a></div></body></html>";
                return Content(result);
            }

            return new EmptyResult();
        }
    }
}

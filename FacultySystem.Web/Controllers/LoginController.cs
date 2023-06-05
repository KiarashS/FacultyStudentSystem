using ContentManagementSystem.Commons.ActionResults;
using ContentManagementSystem.Commons.Web;
using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.Commons.Web.Captcha;
using ContentManagementSystem.Commons.Web.Filters;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using ContentManagementSystem.Web.Utils;
using Postal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ContentManagementSystem.Web.Controllers
{
    public partial class LoginController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IProfessorService _professorService;
        private readonly IActivityLogService _logs;

        public LoginController(IUnitOfWork uow, IUserService userService, IProfessorService professorService, IActivityLogService logs)
        {
            _uow = uow;
            _userService = userService;
            _professorService = professorService;
            _logs = logs;
        }

        [NoBrowserCache]
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
        public virtual ActionResult Logon(string email, string password, string captchaInputText, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return new CamelCaseJsonResult { Data = new { Type = "danger", Title = ModelState.Where(m => m.Key.ToLowerInvariant() == "captchainputtext").Select(m => m.Value.Errors.First().ErrorMessage).First(), Message = "جهت بارگذاری مجدد تصویر امنیتی بر روی آن کلیک نمائید." } };
            }

            if (User.Identity.IsAuthenticated)
            {
                return new CamelCaseJsonResult { Data = new { Type = "success", Title = "شما وارد سامانه شده اید.", Message = "بازگشت به صفحه اصلی ...", RedirectUrl = "/dashboard" } };
            }

            email = email.Trim().ToLowerInvariant();
            var userHashedPassword = _userService.GetPassword(email);

            if (string.IsNullOrEmpty(userHashedPassword) || userHashedPassword.Length == 0)
            {
                _logs.CreateActivityLog(new ActivityLogViewModel
                {
                    ActionBy = CurrentUserName,
                    ActionType = "logon",
                    Message = $"ورود ناموفق به سامانه با ایمیل ناموجود \"{email}\"",
                    SourceAddress = Request.UserHostAddress,
                    Url = Request.RawUrl
                });
                _uow.SaveAllChanges();
                return new CamelCaseJsonResult { Data = new { Type = "danger", Title = "کاربری با این آدرس ایمیل در سامانه موجود نمی باشد.", Message = "لطفاً مجدداً تلاش نمایید." } };
            }

            if (PasswordHash.ValidatePassword(password, userHashedPassword))
            {
                var userInfo = _userService.GetInfoForLogon(email);

                if (userInfo.Roles.Contains(ConstantsUtil.ProfessorRole) && !userInfo.Roles.Contains(ConstantsUtil.AdminRole))
                {
                    var banInfo = _professorService.GetBanInfo(userInfo.UserId);

                    if (banInfo != null && banInfo.IsSoftDelete)
                    {
                        return new CamelCaseJsonResult
                        {
                            //Data = new { Type = "danger", Title = "متاسفانه اکانت شما حذف شده است!", Message = "جهت پیگیری با مدیر سامانه ارتباط برقرار نمائید." }
                            Data = new { Type = "danger", Title = "کاربری با این آدرس ایمیل در سامانه موجود نمی باشد.", Message = "لطفاً مجدداً تلاش نمایید." }
                        };
                    }

                    if (banInfo != null && banInfo.IsBanned && !string.IsNullOrEmpty(banInfo.BannedReason))
                    {
                        return new CamelCaseJsonResult { Data = new { Type = "danger", Title = "متاسفانه اکانت شما غیر فعال شده است!", Message = $"دلیل: {banInfo.BannedReason}" } };
                    }

                    if (banInfo != null && banInfo.IsBanned)
                    {
                        return new CamelCaseJsonResult
                        {
                            Data = new {Type = "danger", Title = "متاسفانه اکانت شما غیر فعال شده است!", Message = ""}
                        };
                    }
                }

                var token = email + ",#;" + userInfo.UserId.ToString();
                //var userRoles = _userService.GetRolesForUser(email);
                //FormsAuthentication.SetAuthCookie(email + ",#;" + userId.ToString() , false);

                HttpContext currentContext = System.Web.HttpContext.Current;
                var formsCookieStr = string.Empty;
                var userRoles = userInfo.Roles.StringJoin(",");
                var ticket = new FormsAuthenticationTicket(
                         3,                                                   // version
                         token,                                               // email, userId
                         DateTime.UtcNow.AddHours(3.5),                       // issue time
                         DateTime.UtcNow.AddHours(3.5).AddMinutes(30),        // expires
                         false,                                               // persistent
                         userRoles                                            // user data
                   );

                formsCookieStr = FormsAuthentication.Encrypt(ticket);
                var formsCookie = new HttpCookie(FormsAuthentication.FormsCookieName, formsCookieStr);
                currentContext.Response.Cookies.Add(formsCookie);

                var isFirstLogin = _userService.IsFirstLogin(userInfo.UserId);
                _userService.UpdateLastLogonTimeAndIp(email, Request.UserHostAddress);
                _uow.SaveAllChanges();

                if (isFirstLogin)
                {
                    HttpContext.CacheInsert(email + ";IsFirstLogin", isFirstLogin, 360);
                }

                if (ShouldRedirect(returnUrl))
                {
                    return new CamelCaseJsonResult { Data = new { Type = "success", Title = "ورود موفقیت آمیز به سامانه ...", Message = "لطفاً چند لحظه صبر نمائید ...", RedirectUrl = returnUrl } };
                }

                return new CamelCaseJsonResult { Data = new { Type = "success", Title = "ورود موفقیت آمیز به سامانه ...", Message = "لطفاً چند لحظه صبر نمائید ...", RedirectUrl = "/dashboard" } };
            }

            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "logon",
                Message = $"ورود ناموفق به سامانه به دلیل رمز ناصحیح با ایمیل \"{email}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();
            return new CamelCaseJsonResult { Data = new { Type = "error", Title = "آدرس ایمیل و یا رمز عبور شما اشتباه می باشد.", Message = "لطفاً مجدداً تلاش نمایید و در صورت فراموشی رمز عبورتان، بر روی فراموشی رمز عبور کلیک نمائید." } };
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //[AjaxOnly]
        //[ValidateCaptcha]
        //[Demo(errorType: "error")]
        //public virtual async Task<ActionResult> ForgottenPassword(string email, string captchaInputText)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return new CamelCaseJsonResult { Data = new { Type = "error", Title = ModelState.Where(m => m.Key.ToLowerInvariant() == "captchainputtext").Select(m => m.Value.Errors.First().ErrorMessage).First(), Message = "جهت بارگذاری مجدد تصویر امنیتی بر روی آن کلیک نمائید." } };
        //    }

        //    var forgottenValue = email + ";" + DateTime.Now.ToString(CultureInfo.InvariantCulture);
        //    if (HttpContext.CacheRead<string>(email + ";ForgottenPassword") != null)
        //    {
        //        return new CamelCaseJsonResult { Data = new { Type = "success", Title = "پیامی مبنی بر تائید درخواست رمز عبور به ایمیل شما ارسال شده است.", Message = "لطفاً به ایمیل خود مراجعه کرده و درخواست خود را تائید فرمایید." } };
        //    }

        //    var senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
        //    var tokenLink = Url.Action(MVC.Login.ActionNames.ForgottenPasswordVerify, MVC.Login.Name, new { token = RijndaelManagedEncryption.EncryptRijndael(forgottenValue) }, this.Request.Url.Scheme);
        //    dynamic mailer = new Email("ForgottenPassword");
        //    mailer.From = senderEmail;
        //    mailer.To = email;
        //    mailer.Subject = "بازیابی رمز عبور";
        //    mailer.TokenLink = tokenLink;
        //    await mailer.SendAsync();
        //    HttpContext.CacheInsert(email + ";ForgottenPassword", forgottenValue, 120);

        //    _logs.CreateActivityLog(new ActivityLogViewModel
        //    {
        //        ActionBy = CurrentUserName,
        //        ActionType = "forgot password",
        //        Message = $"درخواست رمز عبور جدید برای کاربری با ایمیل \"{email}\"",
        //        SourceAddress = Request.UserHostAddress,
        //        Url = Request.RawUrl
        //    });
        //    _uow.SaveAllChanges();
        //    return new CamelCaseJsonResult { Data = new { Type = "success", Title = "لطفاً به ایمیل خود مراجعه کرده و درخواست بازیابی رمز عبورتان را تائید فرمایید.", Message = "توجه فرمائید که ایمیل بازیابی رمز عبور تنها تا 2 ساعت فعال می باشد." } };
        //}

        //[OnlyGuest(Order = 0)]
        //[Demo(errorType: "error")]
        //public virtual async Task<ActionResult> ForgottenPasswordVerify(string token)
        //{
        //    if (string.IsNullOrWhiteSpace(token))
        //        return Redirect("/");

        //    var plainToken = RijndaelManagedEncryption.DecryptRijndael(token);
        //    var keys = plainToken.Split(';');
        //    var forgotEmail = keys[0];
        //    var forgotDate = keys[1];

        //    var cachedToken = HttpContext.CacheRead<string>(forgotEmail + ";ForgottenPassword");
        //    if (cachedToken == null || cachedToken != plainToken)
        //    {
        //        if (HttpContext.CacheRead<string>(forgotEmail + ";ForgottenPassword") != null)
        //            HttpContext.InvalidateCache(forgotEmail + ";ForgottenPassword");

        //        ViewBag.Type = "info";
        //        ViewBag.Title = "لینک بازیابی نامعتبر است، لطفاً مجدداً اقدام نمائید.";
        //        ViewBag.Message = "ابتدا می بایست درخواست بازیابی رمز عبور خود را ارسال نمائید.";

        //        return View("Index");
        //    }

        //    var date = new DateTime();
        //    if (!DateTime.TryParse(forgotDate, out date))
        //    {
        //        ViewBag.Type = "info";
        //        ViewBag.Title = "ایمیل بازیابی نامعتبر است.";
        //        ViewBag.Message = "لطفاً مجدداً اقدام نمائید.";

        //        return View("Index");
        //    }

        //    var dateDiff = (DateTime.Now - date).TotalMinutes;

        //    if (dateDiff > 120)
        //    {
        //        if (HttpContext.CacheRead<string>(forgotEmail + ";ForgottenPassword") != null)
        //            HttpContext.InvalidateCache(forgotEmail + ";ForgottenPassword");

        //        ViewBag.Type = "info";
        //        ViewBag.Title = "لینک بازیابی منقضی شده است، لطفاً مجدداً اقدام نمائید.";
        //        ViewBag.Message = "توجه فرمائید که ایمیل بازیابی رمز عبور تنها تا 2 ساعت فعال می باشد.";

        //        return View("Index");
        //    }

        //    var newPassword = SafePassword.CreatePassword(8);
        //    var newHashedPassword = PasswordHash.CreateHash(newPassword);
        //    _userService.UpdatePasswordByEmail(forgotEmail, newHashedPassword);
        //    _uow.SaveAllChanges();

        //    var senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
        //    dynamic mailer = new Email("NewPassword");
        //    mailer.From = senderEmail;
        //    mailer.To = forgotEmail;
        //    mailer.Subject = "رمز عبور جدید شما";
        //    mailer.NewPassword = newPassword;
        //    await mailer.SendAsync();

        //    if (HttpContext.CacheRead<string>(forgotEmail + ";ForgottenPassword") != null)
        //        HttpContext.InvalidateCache(forgotEmail + ";ForgottenPassword");


        //    ViewBag.Type = "success";
        //    ViewBag.Title = "ایمیلی حاوی رمز عبور جدید به ایمیل شما ارسال شد.";

        //    return View("Index");
        //}

        //[HttpPost]
        //[AjaxOnly]
        //[OnlyGuest(Order = 0)]
        //public virtual ActionResult CheckEmail(string email)
        //{
        //    var isExistUser = _userService.IsExistUser(email);
        //    return Content(isExistUser.ToString().ToLower());
        //    //return Content((true).ToString().ToLower());
        //}

        public virtual ActionResult Logout()
        {
            if (!User.Identity.IsAuthenticated)
                return Redirect("/");

            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        private bool ShouldRedirect(string returnUrl)
        {
            return !string.IsNullOrWhiteSpace(returnUrl) &&
                                Url.IsLocalUrl(returnUrl) &&
                                returnUrl.Length > 1 &&
                                returnUrl.StartsWith("/") &&
                                !returnUrl.StartsWith("//") &&
                                !returnUrl.StartsWith("/\\");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContentManagementSystem.Commons.ActionResults;
using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.Commons.Web.ModelBinder;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using ContentManagementSystem.Web.Utils;
using System.Web.Security;
using ContentManagementSystem.Commons.Web;
using Postal;
using System.Configuration;

namespace ContentManagementSystem.Web.Areas.Dashboard.Controllers
{
    [SiteAuthorize(Roles = ConstantsUtil.AdminRole)]
    public partial class AdminController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IActivityLogService _logs;

        public AdminController(IUnitOfWork uow, IUserService userService, IActivityLogService logs)
        {
            _uow = uow;
            _userService = userService;
            _logs = logs;
        }

        public virtual ActionResult Details()
        {
            var details = _userService.GetAdminDashboardDetails(CurrentUserId);
            return View(details);
        }

        [HttpPost]
        [AjaxOnly]
        [Demo]
        public virtual ActionResult Details(DetailsViewModel info)
        {
            if (CurrentUserName != info.Email && !_userService.IsUniqueEmail(CurrentUserId, info.Email))
            {
                return new CamelCaseJsonResult { Data = new { Type = "danger", Title = "این ایمیل توسط فرد دیگری در سامانه ثبت شده است." } };
            }

            if (!_userService.IsUniqueEmail(CurrentUserId, CurrentUserName))
            {
                return new CamelCaseJsonResult { Data = new { Type = "error", Title = "کاربری با این آدرس ایمیل موجود است." } };
            }

            _userService.UpdateDetails(CurrentUserId, info);
            _uow.SaveAllChanges();

            return new CamelCaseJsonResult { Data = new { Type = "success", Title = "اطلاعات شما با موفقیت بروز شد." } };
        }

        public virtual ActionResult ManageUsers(bool showDisableUsers = false)
        {
            if (showDisableUsers)
            {
                ViewBag.ShowDisableUsers = showDisableUsers;
            }
            return View(MVC.Dashboard.Admin.Views.ManageUsers);
        }

        public virtual ActionResult EducationalDegree()
        {
            return View(MVC.Dashboard.Admin.Views.EducationalDegree);
        }

        public virtual ActionResult College()
        {
            return View(MVC.Dashboard.Admin.Views.College);
        }

        public virtual ActionResult EducationalGroup()
        {
            return View(MVC.Dashboard.Admin.Views.EducationalGroup);
        }

        public virtual ActionResult AcademicRank()
        {
            return View(MVC.Dashboard.Admin.Views.AcademicRank);
        }

        public virtual ActionResult DefaultFreeField()
        {
            return View(MVC.Dashboard.Admin.Views.DefaultFreeField);
        }

        public virtual ActionResult ActivityLog()
        {
            return View(MVC.Dashboard.Admin.Views.ActivityLog);
        }

        public virtual ActionResult AdminMessage(bool showPendingMessages = false)
        {
            if (showPendingMessages)
            {
                ViewBag.ShowPendingMessages = showPendingMessages;
            }
            return View(MVC.Dashboard.Admin.Views.AdminMessage);
        }

        public virtual ActionResult CitationManagement()
        {
            return View(MVC.Dashboard.Admin.Views.CitationManagement);
        }

        public virtual ActionResult News()
        {
            return View(MVC.Dashboard.Admin.Views.News);
        }

        [HttpPost]
        [AjaxOnly]
        [Demo]
        public virtual async System.Threading.Tasks.Task<ActionResult> ResetPassword(int userId)
        {
            var newPassword = SafePassword.CreatePassword(8);
            var newHashedPassword = PasswordHash.CreateHash(newPassword);
            var user = _userService.UpdatePasswordById(userId, newHashedPassword);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "change password",
                Message = $"تغییر رمز عبور کاربر \"{user.Email}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            var senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
            dynamic mailer = new Email("NewPassword");
            mailer.From = senderEmail;
            mailer.To = user.Email;
            mailer.Subject = "رمز عبور جدید شما";
            mailer.NewPassword = newPassword;
            await mailer.SendAsync();

            return new CamelCaseJsonResult { Data = new { Type = "success", Title = "رمز عبور جدید با موفقیت برای کاربر مورد نظرارسال شد.", NewPassword = $"رمز عبور جدید: <span class=\"uk-text-success font-arial\" dir=\"ltr\">{newPassword}</span>" } };
        }
    }
}
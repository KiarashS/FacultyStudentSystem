using ContentManagementSystem.Commons.ActionResults;
using ContentManagementSystem.Commons.Web;
using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.Commons.Web.Captcha;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ContentManagementSystem.Web.Controllers
{
    public partial class RegistrationController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IProfessorService _professorService;
        private readonly ISectionOrderService _sectionsService;
        private readonly IFreeFieldService _freeFieldService;
        private readonly IDefaultFreeFieldService _defaultFreeFieldService;

        public RegistrationController(IUnitOfWork uow, IUserService userService, IProfessorService professorService, ISectionOrderService sectionsService,
            IFreeFieldService freeFieldService, IDefaultFreeFieldService defaultFreeFieldService)
        {
            _uow = uow;
            _userService = userService;
            _professorService = professorService;
            _sectionsService = sectionsService;
            _freeFieldService = freeFieldService;
            _defaultFreeFieldService = defaultFreeFieldService;
        }

        public virtual ActionResult Index()
        {
            var isEnableFacultyRegistration = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableFacultyRegistration"]);
            if (!isEnableFacultyRegistration || Request.IsAuthenticated)
            {
                return Redirect("/");
            }
            
            return View();
        }

        [HttpPost]
        [AjaxOnly]
        [Demo]
        [ValidateAntiForgeryToken]
        [ValidateCaptcha(ExpireTimeCaptchaCodeBySeconds = 120)]
        public virtual ActionResult Index(RegistrationViewModel info)
        {
            if (!ModelState.IsValid)
            {
                return new CamelCaseJsonResult { Data = new { Type = "danger", Title = ModelState.Where(m => m.Key.ToLowerInvariant() == "captchainputtext").Select(m => m.Value.Errors.First().ErrorMessage).First(), Message = "جهت بارگذاری مجدد تصویر امنیتی بر روی آن کلیک نمائید." } };
            }

            var isEnableFacultyRegistration = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableFacultyRegistration"]);
            if (!isEnableFacultyRegistration || Request.IsAuthenticated)
            {
                return new CamelCaseJsonResult { Data = new { Type = "danger", Title = "درخواست شما نامعتبر می باشد." } };
            }

            var newHashedPassword = PasswordHash.CreateHash(info.Password);
            var userInfo = new UserListViewModel
            {
                Firstname = info.Firstname,
                Lastname = info.Lastname,
                Email = info.Email,
                PageId = info.PageId,
                IsSoftDelete = Convert.ToBoolean(ConfigurationManager.AppSettings["RegisteredFacultyNeedActivationByAdmin"])
            };

            var user = _userService.CreateUser(userInfo, newHashedPassword, new[] { "1:professor" });
            _uow.SaveAllChanges();

            userInfo.UserId = user.Id;
            _professorService.CreateProfessor(userInfo, false); // Just in registration SoftDeleteDate field set to null
            _sectionsService.CreateSections(userInfo.UserId);
            var defaultFields = _defaultFreeFieldService.GetListFreeFields();
            if (defaultFields.Any())
            {
                _freeFieldService.CreateFreeFields(userInfo.UserId, defaultFields);
            }
            _uow.SaveAllChanges();

            CreateDirectories(userInfo.UserId);
            return new CamelCaseJsonResult { Data = new { Type = "success", Title = "عضویت شما با موفقیت انجام شد و پس از تائید مدیر سامانه می توانید وارد پروفایل خود شوید." } };
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult CheckPassword(string password)
        {
            if (password.IsSafePasword())
            {
                return new HttpStatusCodeResult(200);
            }

            return new HttpStatusCodeResult(404);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult CheckEmail(string email)
        {
            if (!_userService.IsExistUser(email) && !_professorService.ExistProfessor(email))
            {
                return new HttpStatusCodeResult(200);
            }

            return new HttpStatusCodeResult(404);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult CheckPageId(string pageId)
        {
            if (!_professorService.IsExistProfessor(pageId))
            {
                return new HttpStatusCodeResult(200);
            }

            return new HttpStatusCodeResult(404);
        }

        [NonAction]
        private void CreateDirectories(int userId)
        {
            var userFolderPath = Server.MapPath("~") + @"\App_Data\UsersFiles\" + userId.ToString();

            if (!System.IO.Directory.Exists(userFolderPath))
            {
                System.IO.Directory.CreateDirectory(userFolderPath + @"\LessonFiles\Files");
                System.IO.Directory.CreateDirectory(userFolderPath + @"\LessonFiles\Practices");
                System.IO.Directory.CreateDirectory(userFolderPath + @"\LessonFiles\Scores");
                System.IO.Directory.CreateDirectory(userFolderPath + @"\Resume");
                System.IO.Directory.CreateDirectory(userFolderPath + @"\GalleryFiles");
            }
        }
    }
}
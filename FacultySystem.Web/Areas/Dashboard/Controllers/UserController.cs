using ContentManagementSystem.Commons.Web;
using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using ContentManagementSystem.Web.Utils;
using Postal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ContentManagementSystem.Web.Areas.Dashboard.Controllers
{
    [SiteAuthorize(Roles = ConstantsUtil.AdminRole)]
    public partial class UserController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IProfessorService _professorService;
        private readonly ISectionOrderService _sectionsService;
        private readonly IFreeFieldService _freeFieldService;
        private readonly IDefaultFreeFieldService _defaultFreeFieldService;
        private readonly ILessonService _lessonService;
        private readonly IGalleryService _galleryService;
        private readonly IActivityLogService _logs;

        public UserController(IUnitOfWork uow, IUserService userService, IProfessorService professorService, 
            ISectionOrderService sectionsService, IFreeFieldService freeFieldService, 
            IDefaultFreeFieldService defaultFreeFieldService, IActivityLogService logs,
            ILessonService lessonService, IGalleryService galleryService)
        {
            _uow = uow;
            _userService = userService;
            _professorService = professorService;
            _sectionsService = sectionsService;
            _freeFieldService = freeFieldService;
            _defaultFreeFieldService = defaultFreeFieldService;
            _lessonService = lessonService;
            _galleryService = galleryService;
            _logs = logs;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List(
            string filterFirstname, string filterLastname, 
            string filterEmail, string filterPageId, 
            string filterRole = "notdefined", bool filterIsBanned = false, 
            bool filterIsSoftDelete = false, int jtStartIndex = 0, 
            int jtPageSize = 10)
        {

            var users = _userService.GetListUsers(
                filterFirstname, filterLastname, 
                filterEmail, filterPageId, 
                filterRole, filterIsBanned, 
                filterIsSoftDelete, jtStartIndex, 
                jtPageSize);

            var totalRecordCount = _userService.TotalUsersCount(
                filterFirstname, filterLastname,
                filterEmail, filterPageId,
                filterRole, filterIsBanned,
                filterIsSoftDelete);

            return Json(new { Result = "OK", TotalRecordCount = totalRecordCount, Records = users });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public async virtual Task<ActionResult> Create(UserListViewModel user, string[] userRoles)
        {
            var newPassword = SafePassword.CreatePassword(8);
            var newHashedPassword = PasswordHash.CreateHash(newPassword);

            if (userRoles.Length == 0)
            {
                return Json(new { Result = "ERROR", Message = "انتخاب حداقل یک نقش الزامی می باشد." });
            }

            if (!_userService.IsUniqueEmail(user.UserId, user.Email))
            {
                return Json(new { Result = "ERROR", Message = "کاربری با این آدرس ایمیل موجود است." });
            }

            if (userRoles.Contains("1:professor") && !_professorService.IsUniquePageId(user.UserId, user.PageId))
            {
                return Json(new { Result = "ERROR", Message = "کاربری با این شناسه موجود است." });
            }

            var newUser = _userService.CreateUser(user, newHashedPassword, userRoles);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "create",
                Message = $"کاربر \"{user.Email}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            user.UserId = newUser.Id;

            if (userRoles.Contains("1:professor"))
            {
                _professorService.CreateProfessor(user);
                _sectionsService.CreateSections(user.UserId);

                var defaultFields = _defaultFreeFieldService.GetListFreeFields();
                if (defaultFields.Any())
                {
                    _freeFieldService.CreateFreeFields(user.UserId, defaultFields);
                }

                _uow.SaveAllChanges();

                CreateDirectories(user.UserId);
            }

            var senderEmail = ConfigurationManager.AppSettings["SenderEmail"];
            dynamic mailer = new Email("CreateUser");
            mailer.From = senderEmail;
            mailer.To = user.Email.Trim().ToLowerInvariant();
            mailer.Subject = "مشخصات اکانت شما جهت ورود به صفحه شخصی";
            mailer.Fullname = user.Fullname;
            mailer.Password = newPassword;
            mailer.LogonLink = Url.ActionAbsolute(MVC.Login.Index());
            await mailer.SendAsync();

            return Json(new { Result = "OK", Record = user });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(UserListViewModel user, string[] userRoles)
        {
            if (userRoles.Length == 0)
            {
                return Json(new { Result = "ERROR", Message = "انتخاب حداقل یک نقش الزامی می باشد." });
            }

            if (!_userService.IsUniqueEmail(user.UserId, user.Email))
            {
                return Json(new { Result = "ERROR", Message = "کاربری با این آدرس ایمیل موجود است." });
            }

            if (userRoles.Contains("1:professor") && !_professorService.IsUniquePageId(user.UserId, user.PageId))
            {
                return Json(new { Result = "ERROR", Message = "کاربری با این شناسه موجود است." });
            }

            var currentRoles = _userService.GetRolesForUserAndDetach(user.UserId);
            var newRoles = userRoles.Select(r => r.Split(':')[1].Trim().ToLowerInvariant()).ToList();
            var inCurrentRolesOnly = currentRoles.Except(newRoles).ToList();
            var inNewRolesOnly = newRoles.Except(currentRoles).ToList();
            var isRolesChanged = inCurrentRolesOnly.Count() != 0 || inNewRolesOnly.Count() != 0;
            var isProfessor = newRoles.Contains(ConstantsUtil.ProfessorRole);
            //var isProfessor = _userService.IsUserInRole(user.UserId, ConstantsUtil.ProfessorRole);

            if (!currentRoles.Contains(ConstantsUtil.ProfessorRole) && isProfessor) // added new professor role
            {
                if (!_professorService.ExistProfessor(user.UserId))
                {
                    _professorService.CreateProfessor(user);
                    _sectionsService.CreateSections(user.UserId);
                    _uow.SaveAllChanges();
                    isProfessor = false; // prevent double update professor
                    CreateDirectories(user.UserId);
                }
            }

            if (currentRoles.Contains(ConstantsUtil.ProfessorRole) && !isProfessor) // deleted professor role
            {
                //METHOD ONE
                //DeleteUserFiles(user.UserId);
                ////var pageId = _professorService.GetPageId(user.UserId);
                //_lessonService.DeleteAllLesson(user.UserId);
                //_galleryService.DeleteAllGallery(user.UserId);
                //_professorService.DeleteProfessor(user.UserId);
                //_uow.SaveAllChanges();
                //DeleteDirectories(user.UserId);

                //METHOD TWO
                _professorService.SetSoftDelete(user.UserId, true);
                _uow.SaveAllChanges();
            }

            //if (currentRoles.Contains(ConstantsUtil.ProfessorRole) && isProfessor)
            //{
            //    var pageId = _professorService.GetPageId(user.UserId);
            //    if (pageId != user.PageId)
            //    {
            //        UpdateDirectories(pageId, user.PageId);
            //    }
            //}

            _userService.UpdateUser(user, userRoles, isProfessor, isRolesChanged);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "update",
                Message = $"کاربر \"{user.Email}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(int userId)
        {
            var adminId = int.Parse(ConfigurationManager.AppSettings["AdminId"]);
            if (userId == adminId)
            {
                return Json(new { Result = "ERROR", Message = "امکان حذف مدیر کل سامانه وجود ندارد." });
            }

            var existProfessor = _professorService.ExistProfessor(userId);
            if (existProfessor)
            {
                DeleteUserFiles(userId);
                DeleteDirectories(userId);
                _sectionsService.DeleteSections(userId);
                _lessonService.DeleteAllLesson(userId);
                _galleryService.DeleteAllGallery(userId);
                _professorService.DeleteProfessor(userId);
            }

            _userService.DeleteUser(userId);

            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "delete",
                Message = "کاربر",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [NonAction]
        private void DeleteUserFiles(int userId)
        {
            var avatarName = _professorService.GetAvatar(userId);
            var avatarPath = Server.MapPath("~") + @"\App_Data\Avatars\";

            if (!string.IsNullOrEmpty(avatarName) && System.IO.File.Exists(avatarPath + avatarName))
            {
                System.IO.File.Delete(avatarPath + avatarName);
            }
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

        [NonAction]
        private void DeleteDirectories(int userId)
        {
            var userFolderPath = Server.MapPath("~") + @"\App_Data\UsersFiles\" + userId.ToString();

            if (System.IO.Directory.Exists(userFolderPath))
            {
                System.IO.Directory.Delete(userFolderPath, true);
            }
        }

        //[NonAction]
        //private void UpdateDirectories(string oldPageId, string newPageId)
        //{
        //    var oldUserFolderPath = Server.MapPath("~") + @"\App_Data\UsersFiles\" + oldPageId;
        //    var newUserFolderPath = Server.MapPath("~") + @"\App_Data\UsersFiles\" + newPageId;

        //    if (System.IO.Directory.Exists(oldUserFolderPath))
        //    {
        //        System.IO.Directory.Move(oldUserFolderPath, newUserFolderPath);
        //    }
        //}

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult CheckEmail(int userId, string email, string oldEmail)
        {
            if (userId == 0) // create form
            {
                if (_userService.IsUniqueEmail(userId, email))
                {
                    return new HttpStatusCodeResult(200);
                }

                return new HttpStatusCodeResult(404);
            }

            if (oldEmail != email && !_userService.IsUniqueEmail(userId, email)) // edit form
            {
                return new HttpStatusCodeResult(404);
            }

            return new HttpStatusCodeResult(200);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult CheckPageId(int userId, string pageId, string oldPageId)
        {
            if (userId == 0) // create form
            {
                if (_professorService.IsUniquePageId(userId, pageId))
                {
                    return new HttpStatusCodeResult(200);
                }

                return new HttpStatusCodeResult(404);
            }

            if (oldPageId != pageId && !_professorService.IsUniquePageId(userId, pageId)) // edit form
            {
                return new HttpStatusCodeResult(404);
            }

            return new HttpStatusCodeResult(200);
        }
    }
}
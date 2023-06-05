using ContentManagementSystem.Commons.ActionResults;
using ContentManagementSystem.Commons.Web;
using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.Commons.Web.ModelBinder;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using Ganss.XSS;
using ImageResizer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AngleSharp.Dom;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Web.Utils;
using ContentManagementSystem.Web.Infrastructure;
using System.Threading.Tasks;
using ContentManagementSystem.IocConfig;

namespace ContentManagementSystem.Web.Areas.Dashboard.Controllers
{
    [SiteAuthorize(Roles = ConstantsUtil.AdminProfessorRoles)]
    public partial class HomeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IProfessorService _professorService;
        private readonly ICollegeService _collegeService;
        private readonly IEducationalGroupService _educationalGroupService;
        private readonly IEducationalDegreeService _educationalDegreeService;
        private readonly IAcademicRankService _academicRankService;
        private readonly ISectionOrderService _sectionOrderService;
        private readonly IActivityLogService _logs;
        private readonly IAdminMessageService _adminMessageService;
        private readonly IDocumentCitationService _citationService;

        public HomeController(IUnitOfWork uow, IUserService userService, 
            IProfessorService professorService, ICollegeService collegeService, 
            IEducationalGroupService educationalGroupService, IEducationalDegreeService educationalDegreeService, 
            IAcademicRankService academicRankService, ISectionOrderService sectionOrderService,
            IActivityLogService logs, IAdminMessageService adminMessageService,
            IDocumentCitationService citationService)
        {
            _uow = uow;
            _userService = userService;
            _professorService = professorService;
            _collegeService = collegeService;
            _educationalGroupService = educationalGroupService;
            _educationalDegreeService = educationalDegreeService;
            _academicRankService = academicRankService;
            _sectionOrderService = sectionOrderService;
            _logs = logs;
            _adminMessageService = adminMessageService;
            _citationService = citationService;
        }

        public virtual ActionResult Index()
        {
            ViewBag.IsFirstLogin = false;
            if (User.IsInRole(ConstantsUtil.ProfessorRole) && HttpContext.CacheRead<bool>(CurrentUserName + ";IsFirstLogin"))
            {
                ViewBag.IsFirstLogin = true;
                HttpContext.InvalidateCache(CurrentUserName + ";IsFirstLogin");
            }

            return View();
        }

        public virtual string Namer()
        {
            Type t = typeof(ContentManagementSystem.Commons.Web.CustomRoleProvider);
            string s = t.AssemblyQualifiedName;
            return s;
        }

        [ChildActionOnly]
        public virtual ActionResult Note()
        {
            ViewBag.Note = _userService.GetNote(CurrentUserId);

            return PartialView(MVC.Dashboard.Home.Views._NoteForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Demo]
        public virtual ActionResult UpdateNote(string noteText)
        {
            if (noteText.IsNullOrWhiteSpace() || noteText.IsNullOrEmpty())
            {
                _userService.UpdateNote(CurrentUserId, null);
            }
            else
            {
                _userService.UpdateNote(CurrentUserId, noteText);
            }
            _uow.SaveAllChanges();

            return new CamelCaseJsonResult { Data = new { Type = "success", Title = "یادداشت شما با موفقیت ذخیره شد." } };
        }

        [ChildActionOnly]
        public virtual ActionResult TopMenu()
        {
            var infoViewModel = _userService.GetTopMenuInfo(CurrentUserId);
            return PartialView(MVC.Dashboard.Home.Views._TopMenu, infoViewModel);
        }

        public virtual ActionResult ChangePassword()
        {
            return View(MVC.Dashboard.Home.Views.ChangePassword);
        }

        [HttpPost]
        [AjaxOnly]
        [Demo]
        public virtual ActionResult ChangePassword(string previousPassword, string newPassword, string confirmNewPassword)
        {
            var previousHashedPassword = _userService.GetPasswordById(CurrentUserId);
            var isValidPassword = PasswordHash.ValidatePassword(previousPassword, previousHashedPassword);

            if (!isValidPassword)
            {
                return new CamelCaseJsonResult { Data = new { Type = "danger", Title = "رمز عبور قبلی شما صحیح نمی باشد." } };
            }

            if (!newPassword.IsSafePasword())
            {
                return new CamelCaseJsonResult { Data = new { Type = "danger", Title = "رمز عبور وارد شده را به راحتی می‌توان حدس زد!" } };
            }

            //if (newPassword != confirmNewPassword)
            //{
            //    return new CamelCaseJsonResult { Data = new { Type = "danger", Title = "تکرار رمز عبور با رمز عبور جدید یکسان نمی باشند." } };
            //}

            var newHashedPassword = PasswordHash.CreateHash(newPassword);
            _userService.UpdatePasswordById(CurrentUserId, newHashedPassword);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "change password",
                Message = "تغییر رمز عبور",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            return new CamelCaseJsonResult { Data = new { Type = "success", Title = "رمز عبور شما با موفقیت تغییر یافت." } };
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult Bio()
        {
            var bio = _professorService.GetBio(CurrentUserId);
            return View(MVC.Dashboard.Home.Views.Bio, bio);
        }

        [HttpPost]
        [AjaxOnly]
        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        [Demo]
        public virtual ActionResult Bio(BioViewModel bio)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");
            sanitizer.AllowedSchemes.Add("mailto");
            sanitizer.AllowedSchemes.Add("ftp");
            sanitizer.AllowedSchemes.Add("data");
            sanitizer.RemovingAttribute += (s, e) => e.Cancel = e.Reason == RemoveReason.NotAllowedUrlValue
                    && e.Attribute.Value.Length >= 0xfff0
                    && e.Attribute.Value.StartsWith("data:", StringComparison.OrdinalIgnoreCase);

            var sanitizedBio = sanitizer.Sanitize(bio.BioText);
            if (sanitizedBio.Trim().Length == 0)
            {
                sanitizedBio = null;
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _professorService.UpdateBio(CurrentUserId, sanitizedBio);
            _uow.SaveAllChanges();

            return new CamelCaseJsonResult { Data = new { Type = "success", Title = "بیوگرافی شما با موفقیت بروز شد." } };
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult FreePage()
        {
            var freePage = _professorService.GetFreePage(CurrentUserId);
            return View(MVC.Dashboard.Home.Views.FreePage, freePage);
        }

        [HttpPost]
        [AjaxOnly]
        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        [Demo]
        public virtual ActionResult FreePage(FreePageViewModel freePage)
        {
            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedAttributes.Add("class");
            sanitizer.AllowedSchemes.Add("mailto");
            sanitizer.AllowedSchemes.Add("ftp");
            sanitizer.AllowedSchemes.Add("data");
            sanitizer.RemovingAttribute += (s, e) => e.Cancel = e.Reason == RemoveReason.NotAllowedUrlValue
                    && e.Attribute.Value.Length >= 0xfff0
                    && e.Attribute.Value.StartsWith("data:", StringComparison.OrdinalIgnoreCase);

            var sanitizedFreePage = sanitizer.Sanitize(freePage.FreePageText);
            if (sanitizedFreePage.Trim().Length == 0)
            {
                sanitizedFreePage = null;
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _professorService.UpdateFreePage(CurrentUserId, sanitizedFreePage);
            _uow.SaveAllChanges();

            return new CamelCaseJsonResult { Data = new { Type = "success", Title = "صفحه آزاد شما با موفقیت بروز شد." } };
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult Address()
        {
            return View(MVC.Dashboard.Home.Views.Address);
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult Administration()
        {
            return View(MVC.Dashboard.Home.Views.Administration);
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult Research()
        {
            return View(MVC.Dashboard.Home.Views.Research);
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult Training()
        {
            return View(MVC.Dashboard.Home.Views.Training);
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult Studing()
        {
            return View(MVC.Dashboard.Home.Views.Studing);
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult Honor()
        {
            return View(MVC.Dashboard.Home.Views.Honor);
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult Thesis()
        {
            return View(MVC.Dashboard.Home.Views.Thesis);
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult Publication()
        {
            return View(MVC.Dashboard.Home.Views.Publication);
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult Workshop()
        {
            return View(MVC.Dashboard.Home.Views.Workshop);
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult Language()
        {
            return View(MVC.Dashboard.Home.Views.Language);
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult Lesson()
        {
            return View(MVC.Dashboard.Home.Views.Lesson);
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult Avatar()
        {
            var avatarName = _professorService.GetAvatar(CurrentUserId);
            if (!string.IsNullOrEmpty(avatarName))
            {
                ViewBag.HasPreview = true;
            }

            return View();
        }

        [HttpPost]
        //[AllowUploadSpecialFilesOnly(".jpg,.jpeg,.png")]
        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        [AjaxOnly]
        [Demo(isAjaxCaller: true)]
        public virtual ActionResult Avatar(HttpPostedFileBase image)
        {
            if (image != null && !IsImageFile(image))
            {
                return new CamelCaseJsonResult { Data = new { Type = "info", Title = "ابتدا یک تصویر را انتخاب نمائید." } };
            }

            if (image == null) // remove avatar
            {
                var currentAvatarName = RemoveAvatar(Server.MapPath("~") + @"\App_Data\Avatars\");

                if (!string.IsNullOrEmpty(currentAvatarName))
                {
                    _professorService.UpdateLastUpdateTime(CurrentUserId);
                    _professorService.UpdateAvatar(CurrentUserId, null);
                    _uow.SaveAllChanges();

                    return new CamelCaseJsonResult { Data = new { Type = "success", Title = "تصویر پروفایل شما با موفقیت حذف شد.", Link = Url.Action(MVC.Dashboard.Home.MyAvatar(withDefault: true)) } };
                }

                return new CamelCaseJsonResult { Data = new { Type = "info", Title = "هیچ تصویری جهت حذف یافت نشد." } };
            }

            if (image.ContentType != "image/png" && image.ContentType != "image/jpeg")
            {
                return new CamelCaseJsonResult { Data = new { Type = "info", Title = "پسوند تصویر می بایست یکی از پسوندهای png، jpg یا jpeg باشد." } };
            }

            if (image.ContentLength > 5000000)
            {
                return new CamelCaseJsonResult { Data = new { Type = "info", Title = "حجم تصویر نباید بیشتر از 5 مگابایت باشد." } };
            }

            var uploadFolder = @"~/App_Data/Avatars/";
            var ext = "jpg";
            if (image.ContentType == "image/png")
            {
                ext = "png";
            }

            if (!Directory.Exists(Server.MapPath(uploadFolder)))
            {
                Directory.CreateDirectory(Server.MapPath(uploadFolder));
            }
            RemoveAvatar(Server.MapPath("~") + @"\App_Data\Avatars\");

            var path = $"{uploadFolder}{CleanFileName(CurrentUserName.Substring(0, CurrentUserName.IndexOf("@")))}-{Guid.NewGuid().ToString("N")}.{ext}";
            image.SaveAs(Server.MapPath(path));

            var imageFileName = Path.GetFileName(path); // include extension
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _professorService.UpdateAvatar(CurrentUserId, imageFileName);
            _uow.SaveAllChanges();

            return new CamelCaseJsonResult { Data = new { Type = "success", Title = "تصویر شما با موفقیت بروز شد.", Link = Url.Action(MVC.Dashboard.Home.ActionNames.MyAvatar, MVC.Dashboard.Home.Name) } };
        }

        public virtual ActionResult MyAvatar(bool withDefault = true)
        {
            var avatarName = "avatar.png";
            var avatarPath = Server.MapPath("~") + @"\Content\admin\img\" + avatarName;
            var contentTypes = new Dictionary<string, string>
            {
                { ".jpg", "image/jpeg" },
                { ".png", "image/png" }
            };

            if (!User.IsInRole(ConstantsUtil.ProfessorRole))
            {
                Response.AddHeader("Content-Disposition", "attachment; filename=myavatar" + Path.GetExtension(avatarName));
                return File(System.IO.File.ReadAllBytes(avatarPath), contentTypes[Path.GetExtension(avatarName)]);
            }

            avatarName = _professorService.GetAvatar(CurrentUserId);
            avatarPath = Server.MapPath("~") + @"\App_Data\Avatars\" + avatarName;

            if (withDefault && string.IsNullOrEmpty(avatarName))
            {
                avatarName = "avatar.png";
                avatarPath = Server.MapPath("~") + @"\Content\admin\img\" + avatarName;
            }
            else if (!withDefault && string.IsNullOrEmpty(avatarName))
            {
                return Content("");
            }

            Response.AddHeader("Content-Disposition", "attachment; filename=myavatar" + Path.GetExtension(avatarName));
            return File(System.IO.File.ReadAllBytes(avatarPath), contentTypes[Path.GetExtension(avatarName)]);
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult Details()
        {
            var details = _professorService.GetProfessorDashboardDetails(CurrentUserId);
            details.Colleges = _collegeService.GetColleges();
            details.Groups = _educationalGroupService.GetEducationalGroups();
            details.Degrees = _educationalDegreeService.GetEducationalDegrees();
            details.Ranks = _academicRankService.GetAcademicRanks();

            return View(details);
        }

#pragma warning disable SG0016 // Controller method is vulnerable to CSRF
        [HttpPost]
        [AjaxOnly]
        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        [Demo]
        public virtual async Task<ActionResult> Details(DetailsViewModel info, [ModelBinder(typeof(PersianDateModelBinder))] DateTime? birthdayDate, string updateIndex = "off")
        {
            var defaultCollegeId = _collegeService.GetIdByName("--") ;
            var defaultEducationalGroupId = _educationalGroupService.GetIdByName("--");
            var updateHIndexLastUpdateDate = false;

            if (CurrentUserName != info.Email && !_userService.IsUniqueEmail(CurrentUserId, info.Email))
            {
                return new CamelCaseJsonResult { Data = new { Type = "danger", Title = "این ایمیل توسط فرد دیگری در سامانه ثبت شده است." } };
            }

            if (string.IsNullOrEmpty(info.FirstName))
            {
                return new CamelCaseJsonResult { Data = new { Type = "info", Title = "وارد کردن فیلد نام الزامی می باشد." } };
            }

            if (string.IsNullOrEmpty(info.LastName))
            {
                return new CamelCaseJsonResult { Data = new { Type = "info", Title = "وارد کردن فیلد نام خانوادگی الزامی می باشد." } };
            }

            if (string.IsNullOrEmpty(info.Email))
            {
                return new CamelCaseJsonResult { Data = new { Type = "info", Title = "وارد کردن ایمیل اصلی الزامی می باشد." } };
            }

            if (info.College == defaultCollegeId)
            {
                return new CamelCaseJsonResult { Data = new { Type = "info", Title = "انتخاب فیلد دانشکده الزامی می باشد." } };
            }

            if (info.EducationalGroup == defaultEducationalGroupId)
            {
                return new CamelCaseJsonResult { Data = new { Type = "info", Title = "انتخاب گروه آموزشی الزامی می باشد." } };
            }

            if (!_userService.IsUniqueEmail(CurrentUserId, CurrentUserName))
            {
                return new CamelCaseJsonResult { Data = new { Type = "error", Title = "کاربری با این آدرس ایمیل موجود است." } };
            }

            if (birthdayDate != null)
            {
                info.BirthDate = birthdayDate;
            }

            if (!string.IsNullOrEmpty(info.PersonalWebPage) && !info.PersonalWebPage.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) && !info.PersonalWebPage.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                info.PersonalWebPage = "http://" + info.PersonalWebPage;
            }

            if (!string.IsNullOrEmpty(info.ScopusId) && !info.ScopusId.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) && !info.ScopusId.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                info.ScopusId = "https://" + info.ScopusId;
            }

            if (!string.IsNullOrEmpty(info.GoogleScholarId) && !info.GoogleScholarId.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) && !info.GoogleScholarId.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                info.GoogleScholarId = "https://" + info.GoogleScholarId;
            }

            if (!string.IsNullOrEmpty(info.PubMedId) && !info.PubMedId.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) && !info.PubMedId.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                info.PubMedId = "https://" + info.PubMedId;
            }

            if (!string.IsNullOrEmpty(info.MedLibId) && !info.MedLibId.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) && !info.MedLibId.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                info.MedLibId = "https://" + info.MedLibId;
            }

            if (!string.IsNullOrEmpty(info.ResearchGateId) && !info.ResearchGateId.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) && !info.ResearchGateId.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                info.ResearchGateId = "https://" + info.ResearchGateId;
            }

            if (!string.IsNullOrEmpty(info.OrcidId) && !info.OrcidId.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) && !info.OrcidId.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                info.OrcidId = "https://" + info.OrcidId;
            }

            if (!string.IsNullOrEmpty(info.ResearcherId) && !info.ResearcherId.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) && !info.ResearcherId.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                info.ResearcherId = "https://" + info.ResearcherId;
            }

            _userService.UpdateDetails(CurrentUserId, info);
            _professorService.UpdateDetails(CurrentUserId, info);

            if ("on" == updateIndex.ToLowerInvariant() && (!string.IsNullOrEmpty(info.ScopusId) || !string.IsNullOrEmpty(info.GoogleScholarId)))
            {
                var fetcher = new ExternalResearchRecordsFetcher();
                bool updateScopus = false;
                bool updateGoogle = false;
                updateHIndexLastUpdateDate = true;

                var scopusInfos = new ScopusHIndexViewModel();
                var googleInfos = new GoogleHIndexViewModel();

                if (!string.IsNullOrEmpty(info.ScopusId))
                {
                    scopusInfos = await fetcher.FetchScopusHindexAsync(info.ScopusId, CurrentUserId);
                    updateScopus = true;
                    //_citationService.AddOrUpdate(CurrentUserId, scopusInfos.DocumentsCitation, googleInfos.DocumentsCitation, updateScopus: updateScopus);

                    //_uow.SaveAllChanges();
                }

                if (!string.IsNullOrEmpty(info.GoogleScholarId))
                {
                    googleInfos = await fetcher.FetchGoogleHindex(info.GoogleScholarId, CurrentUserId);
                    updateGoogle = true;
                    //_citationService.AddOrUpdate(CurrentUserId, scopusInfos.DocumentsCitation, googleInfos.DocumentsCitation, updateGoogle: updateGoogle);

                    //_uow.SaveAllChanges();
                }

                _professorService.UpdateUserHIndexInfos(CurrentUserId, scopusInfos.ScopusHIndex, scopusInfos.ScopusDocuments, scopusInfos.OtherNamesFormat, googleInfos.GoogleHIndex, googleInfos.GoogleCitations, updateScopus, updateGoogle);
                _citationService.AddOrUpdate(CurrentUserId, scopusInfos.DocumentsCitation, googleInfos.DocumentsCitation, updateScopus, updateGoogle);
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId, updateHIndexAndCitationDate: updateHIndexLastUpdateDate);
            _uow.SaveAllChanges();

            return new CamelCaseJsonResult { Data = new { Type = "success", Title = "اطلاعات شما با موفقیت بروز شد." } };
        }
#pragma warning restore SG0016 // Controller method is vulnerable to CSRF

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult FreeField()
        {
            return View(MVC.Dashboard.Home.Views.FreeField);
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult SectionsOrder()
        {
            var sections = _sectionOrderService.GetSectionOrders(CurrentUserId);
            return View(MVC.Dashboard.Home.Views.SectionsOrder, sections);
        }

        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        [HttpPost]
        [AjaxOnly]
        [Demo]
        public virtual ActionResult SectionsOrder(string[] orders)
        {
            var sectionOrders = new Dictionary<SectionName, int>();
            foreach (var item in orders)
            {
                sectionOrders.Add((SectionName)Convert.ToByte(item.Split('_')[0]), Convert.ToInt32(item.Split('_')[1]));
            }

            _sectionOrderService.UpdateSections(CurrentUserId, sectionOrders);
            _uow.SaveAllChanges();

            return new CamelCaseJsonResult { Data = new { Type = "success", Title = "ترتیب قرارگیری بخش ها با موفقیت ذخیره شد." } };
        }

        [ChildActionOnly]
        public virtual ActionResult AdminStatistics()
        {
            if (!User.IsInRole(ConstantsUtil.AdminRole))
            {
                return Content(string.Empty);
            }

            var adminDashboardStatistics = new AdminDashboardStatisticViewModel();
            adminDashboardStatistics.NumberOfProfessors = _professorService.TotalCount(null);
            adminDashboardStatistics.NumberOfPendingAdminMessages = _adminMessageService.CountOfPendingMessages();
            adminDashboardStatistics.NumberOfColleges = _collegeService.NumberOfColleges();
            adminDashboardStatistics.NumberOfEducationalGroups = _educationalGroupService.NumberOfEducationalGroups();
            adminDashboardStatistics.NumberOfDisableUsers = _professorService.DisableUsersCount();

            return PartialView(MVC.Dashboard.Home.Views._AdminDashboardStatistics, adminDashboardStatistics);
        }

        [ChildActionOnly]
        public virtual ActionResult ProfessorStatistics()
        {
            if (!User.IsInRole(ConstantsUtil.ProfessorRole))
            {
                return Content(string.Empty);
            }

            var professorDashboardStatistics = _professorService.GetDashboardStatistics(CurrentUserId);
            return PartialView(MVC.Dashboard.Home.Views._ProfessorDashboardStatistics, professorDashboardStatistics);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult CheckPassword(string previousPassword)
        {
            var hashedPassword = _userService.GetPasswordById(CurrentUserId);
            var isValidPassword = PasswordHash.ValidatePassword(previousPassword, hashedPassword);

            if (isValidPassword)
            {
                return new HttpStatusCodeResult(200);
            }

            return new HttpStatusCodeResult(404);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult SafePassword(string newPassword)
        {
            if (newPassword.IsSafePasword())
            {
                return new HttpStatusCodeResult(200);
            }

            return new HttpStatusCodeResult(404);
        }

        private static bool IsImageFile(HttpPostedFileBase photoFile)
        {
            using (var img = Image.FromStream(photoFile.InputStream))
            {
                return img.Width > 0;
            }
        }

        private string RemoveAvatar(string avatarPath)
        {
            var avatarName = _professorService.GetAvatar(CurrentUserId);
            if (!string.IsNullOrEmpty(avatarName) && System.IO.File.Exists(avatarPath + avatarName))
            {
                System.IO.File.Delete(avatarPath + avatarName);
            }

            return avatarName;
        }


        [NonAction]
        private static string CleanFileName(string fileName)
        {
            return Path.GetInvalidFileNameChars().Aggregate(fileName, (current, c) => current.Replace(c.ToString(), string.Empty));
        }
    }
}
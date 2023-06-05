using System.Web.Mvc;
using ContentManagementSystem.Web.Utils;
using ContentManagementSystem.ServiceLayer.Contracts;
using ContentManagementSystem.DataLayer.Context;
using System.Collections.Generic;
using System.IO;
using ContentManagementSystem.Commons.Web;
using System;
using System.Web;
using ContentManagementSystem.Commons.Web.Attributes;
using System.Linq;
using ContentManagementSystem.Commons.ActionResults;

namespace ContentManagementSystem.Web.Controllers
{
    [ValidatePageId]
    public partial class ProfileController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IProfessorService _professorService;
        private readonly IAddressService _addressService;
        private readonly ILessonService _lessonService;
        private readonly ILessonClassInfoService _classService;
        private readonly IPracticeClassInfoService _practiceClassService;
        private readonly ILessonImportantDateService _importantDateService;
        private readonly ILessonNewsService _lessonNewsService;
        private readonly ILessonFileService _lessonFileService;
        private readonly ILessonPracticeService _lessonPracticeService;
        private readonly ILessonScoreService _lessonScoreService;
        private readonly IGalleryService _galleryService;
        private readonly ISectionOrderService _sectionsOrderService;
        private readonly IGalleryItemService _galleryItemService;
        private readonly IExternalResearchService _externalResearchService;
        private readonly IExternalSeminarService _externalSeminarService;
        private readonly IInternalResearchService _internalResearchService;
        private readonly IInternalSeminarService _internalSeminarService;
        private readonly IDocumentCitationService _documentCitationService;

        public ProfileController(IUnitOfWork uow, IUserService userService, 
            IProfessorService professorService, IAddressService addressService, 
            ISectionOrderService sectionsOrderService, ILessonService lessonService,
            ILessonClassInfoService classService, IPracticeClassInfoService practiceClassService,
            ILessonImportantDateService importantDateService, ILessonNewsService lessonNewsService,
            ILessonFileService lessonFileService, ILessonPracticeService lessonPracticeService,
            ILessonScoreService lessonScoreService, IGalleryService galleryService,
            IGalleryItemService galleryItemService, IExternalResearchService externalResearchService,
            IExternalSeminarService externalSeminarService, IInternalResearchService internalResearchService,
            IInternalSeminarService internalSeminarService, IDocumentCitationService documentCitationService)
        {
            _uow = uow;
            _userService = userService;
            _professorService = professorService;
            _addressService = addressService;
            _sectionsOrderService = sectionsOrderService;
            _lessonService = lessonService;
            _classService = classService;
            _practiceClassService = practiceClassService;
            _importantDateService = importantDateService;
            _lessonNewsService = lessonNewsService;
            _lessonFileService = lessonFileService;
            _lessonPracticeService = lessonPracticeService;
            _lessonScoreService = lessonScoreService;
            _galleryService = galleryService;
            _galleryItemService = galleryItemService;
            _externalResearchService = externalResearchService;
            _externalSeminarService = externalSeminarService;
            _internalResearchService = internalResearchService;
            _internalSeminarService = internalSeminarService;
            _documentCitationService = documentCitationService;
        }

        public virtual ActionResult Index(string pageId)
        {
            var infos = _professorService.GetProfileIntroInfos(pageId);
            infos.SectionsOrder = _sectionsOrderService.GetSectionOrders(infos.UserId);

            return View(infos);
        }

        [ChildActionOnly]
        public virtual ActionResult FreeFields()
        {
            var parentViewContext = ControllerContext.ParentActionViewContext;
            var pageId = parentViewContext.RouteData.Values["pageId"].ToString().Trim().ToLowerInvariant();
            var freeFields = _professorService.GetFreeFields(pageId);

            return PartialView(MVC.Profile.Views._FreeFields, freeFields);
        }

        [ChildActionOnly]
        public virtual ActionResult Addresses()
        {
            var parentViewContext = ControllerContext.ParentActionViewContext;
            var pageId = parentViewContext.RouteData.Values["pageId"].ToString().Trim().ToLowerInvariant();
            var addresses = _professorService.GetListAddresses(pageId);

            return PartialView(MVC.Profile.Views._Addresses, addresses);
        }

        [ChildActionOnly]
        public virtual ActionResult Studings()
        {
            var parentViewContext = ControllerContext.ParentActionViewContext;
            var pageId = parentViewContext.RouteData.Values["pageId"].ToString().Trim().ToLowerInvariant();
            var studings = _professorService.GetListStudings(pageId);

            return PartialView(MVC.Profile.Views._Studings, studings);
        }

        [ChildActionOnly]
        public virtual ActionResult Trainings()
        {
            var parentViewContext = ControllerContext.ParentActionViewContext;
            var pageId = parentViewContext.RouteData.Values["pageId"].ToString().Trim().ToLowerInvariant();
            var trainings = _professorService.GetListTrainings(pageId);

            return PartialView(MVC.Profile.Views._Trainings, trainings);
        }

        [ChildActionOnly]
        public virtual ActionResult Memberships()
        {
            var parentViewContext = ControllerContext.ParentActionViewContext;
            var pageId = parentViewContext.RouteData.Values["pageId"].ToString().Trim().ToLowerInvariant();
            var memberships = _professorService.GetListMemberships(pageId);

            return PartialView(MVC.Profile.Views._Memberships, memberships);
        }

        [ChildActionOnly]
        public virtual ActionResult Researchs()
        {
            var parentViewContext = ControllerContext.ParentActionViewContext;
            var pageId = parentViewContext.RouteData.Values["pageId"].ToString().Trim().ToLowerInvariant();
            var researchs = _professorService.GetListResearchs(pageId);

            return PartialView(MVC.Profile.Views._Researchs, researchs);
        }

        [ChildActionOnly]
        public virtual ActionResult Administrations()
        {
            var parentViewContext = ControllerContext.ParentActionViewContext;
            var pageId = parentViewContext.RouteData.Values["pageId"].ToString().Trim().ToLowerInvariant();
            var administrations = _professorService.GetListAdministrations(pageId);

            return PartialView(MVC.Profile.Views._Administrations, administrations);
        }

        [ChildActionOnly]
        public virtual ActionResult Honors()
        {
            var parentViewContext = ControllerContext.ParentActionViewContext;
            var pageId = parentViewContext.RouteData.Values["pageId"].ToString().Trim().ToLowerInvariant();
            var honors = _professorService.GetListHonors(pageId);

            return PartialView(MVC.Profile.Views._Honors, honors);
        }

        [ChildActionOnly]
        public virtual ActionResult Theses()
        {
            var parentViewContext = ControllerContext.ParentActionViewContext;
            var pageId = parentViewContext.RouteData.Values["pageId"].ToString().Trim().ToLowerInvariant();
            var theses = _professorService.GetListTheses(pageId);

            return PartialView(MVC.Profile.Views._Theses, theses);
        }

        [ChildActionOnly]
        public virtual ActionResult Publications()
        {
            var parentViewContext = ControllerContext.ParentActionViewContext;
            var pageId = parentViewContext.RouteData.Values["pageId"].ToString().Trim().ToLowerInvariant();
            var publications = _professorService.GetListPublications(pageId);

            return PartialView(MVC.Profile.Views._Publications, publications);
        }

        [ChildActionOnly]
        public virtual ActionResult Workshops()
        {
            var parentViewContext = ControllerContext.ParentActionViewContext;
            var pageId = parentViewContext.RouteData.Values["pageId"].ToString().Trim().ToLowerInvariant();
            var workshops = _professorService.GetListWorkshops(pageId);
            
            return PartialView(MVC.Profile.Views._Workshops, workshops);
        }

        [ChildActionAjaxOnly]
        public virtual ActionResult ExternalResearchs(string pageId, int id = 1) // id => page number
        {
            var currentPageNumber = id >= 1 ? id : 1;
            var currentPageIndex = currentPageNumber - 1;
            var pageSize = 20;

            var externalResearchs = _externalResearchService.GetProfessorResearchs(pageId, currentPageIndex, pageSize);

            if (externalResearchs == null || !externalResearchs.Any())
                return Content("no-more-info");

            if (externalResearchs.Any() && !Request.IsAjaxRequest())
            {
                externalResearchs.First().ExternalResearchLastUpdateTime = _professorService.GetLastExternalResearchUpdateTime(pageId);
            }

            return PartialView(MVC.Profile.Views._ExternalResearchs, externalResearchs);
        }

        [ChildActionAjaxOnly]
        public virtual ActionResult ExternalSeminars(string pageId, int id = 1) // id => page number
        {
            var currentPageNumber = id >= 1 ? id : 1;
            var currentPageIndex = currentPageNumber - 1;
            var pageSize = 20;

            var externalSeminars = _externalSeminarService.GetProfessorSeminars(pageId, currentPageIndex, pageSize);

            if (externalSeminars == null || !externalSeminars.Any())
                return Content("no-more-info");

            return PartialView(MVC.Profile.Views._ExternalSeminars, externalSeminars);
        }

        [ChildActionAjaxOnly]
        public virtual ActionResult InternalResearchs(string pageId, int id = 1) // id => page number
        {
            var currentPageNumber = id >= 1 ? id : 1;
            var currentPageIndex = currentPageNumber - 1;
            var pageSize = 20;

            var internalResearchs = _internalResearchService.GetProfessorResearchs(pageId, currentPageIndex, pageSize);

            if (internalResearchs == null || !internalResearchs.Any())
                return Content("no-more-info");

            return PartialView(MVC.Profile.Views._InternalArticles, internalResearchs);
        }

        [ChildActionAjaxOnly]
        public virtual ActionResult InternalSeminars(string pageId, int id = 1) // id => page number
        {
            var currentPageNumber = id >= 1 ? id : 1;
            var currentPageIndex = currentPageNumber - 1;
            var pageSize = 20;

            var internalSeminars = _internalSeminarService.GetProfessorSeminars(pageId, currentPageIndex, pageSize);

            if (internalSeminars == null || !internalSeminars.Any())
                return Content("no-more-info");

            return PartialView(MVC.Profile.Views._InternalSeminars, internalSeminars);
        }

        public virtual ActionResult FreePage(string pageId)
        {
            var freePage = _professorService.GetFreePage(pageId);
            if (!freePage.IsActiveFreePage || string.IsNullOrEmpty(freePage.FreePageText))
            {
                return Redirect("/" + pageId);
            }

            ViewBag.PageId = pageId;
            ViewBag.Title = $"صفحه آزاد {freePage.Fullname}";
            return View(freePage);
        }

        public virtual ActionResult GalleryList(string pageId)
        {
            var infos = _professorService.GalleriesIndex(pageId);
            if (infos.GalleriessCount == 0)
            {
                return Redirect("/" + pageId);
            }

            ViewBag.Title = $"لیست گالری های {infos.Fullname}";
            return View(infos);
        }

        public virtual ActionResult Gallery(string pageId, long id, string title)
        {
            var userInfo = _professorService.NameAndId(pageId);
            var gallery = _galleryService.GalleryIndex(userInfo.UserId, id);

            if (gallery == null || !gallery.IsActive)
            {
                return RedirectToRoute("Profile", new { action = MVC.Profile.ActionNames.GalleryList, id = (UrlParameter)null, title = (UrlParameter)null });
            }

            if (string.IsNullOrEmpty(title) || !string.Equals(title, gallery.UrlSlug))
            {
                return RedirectPermanent(Url.RouteUrl("Profile", new { title = gallery.UrlSlug }));
            }

            gallery.Firstname = userInfo.Firstname;
            gallery.Lastname = userInfo.Lastname;
            ViewBag.PageId = pageId;
            ViewBag.Title = $"گالری {gallery.Title}";
            return View(gallery);
        }

        public virtual ActionResult WeeklyProgram(string pageId)
        {
            var programs = _professorService.GetWeeklyProgramIndex(pageId);

            if (programs == null || !programs.WeeklyPrograms.Any() || !programs.IsActiveWeeklyProgram)
            {
                return Redirect("/" + pageId);
            }

            ViewBag.Title = $"برنامه هفتگی {programs.Fullname}";
            return View(programs);
        }

        [ChildActionOnly]
        public virtual ActionResult GalleryItems(string pageId, long id)
        {
            var userId = _professorService.GetUserIdByPageId(pageId);
            var infos = _galleryItemService.GetGalleryItems(userId, id);
            if (!infos.Any())
            {
                if (Request.IsAjaxRequest())
                {
                    return Content("");
                }
                return Content("<div class=\"md-card\"><div class=\"md-card-content uk-text-center\">هیچ آیتمی در این گالری جهت نمایش موجود نیست!</div></div>");
            }

            return PartialView(MVC.Profile.Views._GalleryItem, infos);
        }

        public virtual ActionResult Lessons(string pageId)
        {
            var infos = _professorService.LessonsIndex(pageId);
            if (infos.LessonsCount == 0)
            {
                return Redirect("/" + pageId);
            }

            ViewBag.Title = $"دروس ارائه شده توسط {infos.Fullname}";
            return View(infos);
        }

        public virtual ActionResult Lesson(string pageId, long id, string title)
        {
            var userInfo = _professorService.NameAndId(pageId);
            var lesson = _lessonService.LessonIndex(userInfo.UserId, id);

            if (lesson == null)
            {
                return RedirectToRoute("Profile", new { action = MVC.Profile.ActionNames.Lessons, id = (UrlParameter)null, title = (UrlParameter)null });
            }

            if (string.IsNullOrEmpty(title) || !string.Equals(title, lesson.UrlSlug))
            {
                return RedirectPermanent(Url.RouteUrl("Profile", new { title = lesson.UrlSlug }));
            }

            lesson.Firstname = userInfo.Firstname;
            lesson.Lastname = userInfo.Lastname;
            ViewBag.Title = $"درس {lesson.LessonName}";
            return View(lesson);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ClassInfo(string pageId, long id)
        {
            var userId = _professorService.GetUserIdByPageId(pageId);
            var infos = _classService.GetLessonClasses(userId, id);
            if (!infos.Any())
            {
                if (Request.IsAjaxRequest())
                {
                    return Content("");
                }
                return Content("<span class=\"uk-text-center\">هیچ داده ای جهت نمایش موجود نیست!</span>");
            }

            return PartialView(MVC.Profile.Views._ClassInfo, infos);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult PracticeClassInfo(string pageId, long id)
        {
            var userId = _professorService.GetUserIdByPageId(pageId);
            var infos = _practiceClassService.GetPracticeClasses(userId, id);
            if (!infos.Any())
            {
                if (Request.IsAjaxRequest())
                {
                    return Content("");
                }
                return Content("<span class=\"uk-text-center\">هیچ داده ای جهت نمایش موجود نیست!</span>");
            }

            return PartialView(MVC.Profile.Views._PracticeClass, infos);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ClassImportantDate(string pageId, long id)
        {
            var userId = _professorService.GetUserIdByPageId(pageId);
            var infos = _importantDateService.GetImportantDates(userId, id);
            if (!infos.Any())
            {
                if (Request.IsAjaxRequest())
                {
                    return Content("");
                }
                return Content("<span class=\"uk-text-center\">هیچ داده ای جهت نمایش موجود نیست!</span>");
            }

            return PartialView(MVC.Profile.Views._LessonImportantDate, infos);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ClassNews(string pageId, long id)
        {
            var userId = _professorService.GetUserIdByPageId(pageId);
            var infos = _lessonNewsService.GetLessonNews(userId, id);
            if (!infos.Any())
            {
                if (Request.IsAjaxRequest())
                {
                    return Content("");
                }
                return Content("<span class=\"uk-text-center\">هیچ داده ای جهت نمایش موجود نیست!</span>");
            }

            return PartialView(MVC.Profile.Views._LessonNews, infos);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ClassFile(string pageId, long id)
        {
            var userId = _professorService.GetUserIdByPageId(pageId);
            var infos = _lessonFileService.GetLessonFiles(userId, id);
            if (!infos.Any())
            {
                if (Request.IsAjaxRequest())
                {
                    return Content("");
                }
                return Content("<span class=\"uk-text-center\">هیچ داده ای جهت نمایش موجود نیست!</span>");
            }

            return PartialView(MVC.Profile.Views._LessonFile, infos);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ClassPractice(string pageId, long id)
        {
            var userId = _professorService.GetUserIdByPageId(pageId);
            var infos = _lessonPracticeService.GetLessonPractices(userId, id);
            if (!infos.Any())
            {
                if (Request.IsAjaxRequest())
                {
                    return Content("");
                }
                return Content("<span class=\"uk-text-center\">هیچ داده ای جهت نمایش موجود نیست!</span>");
            }

            return PartialView(MVC.Profile.Views._LessonPractice, infos);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ClassScore(string pageId, long id)
        {
            var userId = _professorService.GetUserIdByPageId(pageId);
            var infos = _lessonScoreService.GetLessonScores(userId, id);
            if (!infos.Any())
            {
                if (Request.IsAjaxRequest())
                {
                    return Content("");
                }
                return Content("<span class=\"uk-text-center\">هیچ داده ای جهت نمایش موجود نیست!</span>");
            }

            return PartialView(MVC.Profile.Views._LessonScore, infos);
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ScopusChartData(string pageId)
        {
            var infos = _documentCitationService.GetScopusChartData(pageId);
            if (!infos.Citations.Any() || !infos.Documents.Any())
            {
                if (Request.IsAjaxRequest())
                {
                    return Content("");
                }
                return Content("<span class=\"uk-text-center\">هیچ داده ای جهت نمایش موجود نیست!</span>");
            }

            return new CamelCaseJsonResult { Data = infos };
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult GoogleChartData(string pageId)
        {
            var infos = _documentCitationService.GetGoogleChartData(pageId);
            if (!infos.Citations.Any())
            {
                if (Request.IsAjaxRequest())
                {
                    return Content("");
                }
                return Content("<span class=\"uk-text-center\">هیچ داده ای جهت نمایش موجود نیست!</span>");
            }

            return new CamelCaseJsonResult { Data = infos };
        }

        [ChildActionOnly]
        public virtual ActionResult Languages()
        {
            var parentViewContext = ControllerContext.ParentActionViewContext;
            var pageId = parentViewContext.RouteData.Values["pageId"].ToString().Trim().ToLowerInvariant();
            var languages = _professorService.GetListLanguages(pageId);

            return PartialView(MVC.Profile.Views._Languages, languages);
        }

        [ChildActionOnly]
        public virtual ActionResult TopMenu()
        {
            var parentViewContext = ControllerContext.ParentActionViewContext;
            var pageId = parentViewContext.RouteData.Values["pageId"].ToString().Trim().ToLowerInvariant();
            var profileMenuInfo = _professorService.ProfileTopMenu(pageId);

            ViewBag.PageId = pageId;
            ViewBag.UserProfileId = profileMenuInfo.Id;
            if (Request.IsAuthenticated)
            {
                ViewBag.CurrentUserId = CurrentUserId;
            }
            
            return PartialView(MVC.Profile.Views._TopMenu, profileMenuInfo);
        }

        public virtual ActionResult ProfessorAvatar(string pageId)
        {
            var avatarName = _professorService.GetAvatar(pageId);
            var avatarPath = Server.MapPath("~") + @"\App_Data\Avatars\" + avatarName;
            var contentTypes = new Dictionary<string, string>();
            contentTypes.Add(".jpg", "image/jpeg");
            contentTypes.Add(".png", "image/png");

            if (string.IsNullOrEmpty(pageId) || string.IsNullOrEmpty(avatarName))
            {
                avatarName = "avatar.png";
                avatarPath = Server.MapPath("~") + @"\Content\admin\img\" + avatarName;
            }

            Response.AddHeader("Content-Disposition", "attachment; filename=myavatar" + Path.GetExtension(avatarName));
            return File(System.IO.File.ReadAllBytes(avatarPath), contentTypes[Path.GetExtension(avatarName)]);
        }
    }
}

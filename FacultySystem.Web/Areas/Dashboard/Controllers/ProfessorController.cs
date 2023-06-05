using ContentManagementSystem.Commons.ActionResults;
using ContentManagementSystem.Commons.Web;
using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using ContentManagementSystem.Web.Infrastructure;
using ContentManagementSystem.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Web.Areas.Dashboard.Controllers
{
    [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
    public partial class ProfessorController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProfessorService _professorService;
        private readonly IExternalResearchService _externalResearchService;

        public ProfessorController(IUnitOfWork uow, IProfessorService professorService, IExternalResearchService externalResearchService)
        {
            _uow = uow;
            _professorService = professorService;
            _externalResearchService = externalResearchService;
        }

        public virtual ActionResult Resume()
        {
            var currentResume = _professorService.GetResumeFilename(CurrentUserId);
            if (!string.IsNullOrEmpty(currentResume))
            {
                ViewBag.ResumeUrl = Url.ActionAbsolute(MVC.Home.GetFile(RijndaelManagedEncryption.EncryptRijndael(CurrentUserId.ToString() + ";#;" + currentResume), 4));
            }

            return View();
        }

        [HttpPost]
        [AllowUploadSpecialFilesOnly(".pdf")]
        [Demo]
        public virtual ActionResult Resume(HttpPostedFileBase resume, bool deleteResume = false)
        {
            var currentResume = _professorService.GetResumeFilename(CurrentUserId);
            var newResumeFilename = currentResume;
            var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\Resume\";

            if (resume != null && resume.ContentLength > 0)
            {
                if (ConvertBytesToMegabytes(resume.ContentLength) > 12.0)
                {
                    ViewBag.HasInfo = true;
                    ViewBag.Message = "حجم رزومه شما نباید بیشتر از 12 مگابایت باشد..";
                    return View();
                }

                var fileExtension = System.IO.Path.GetExtension(resume.FileName);
                var filename = "__" + CurrentUserId.ToString() + "__" + System.IO.Path.GetFileNameWithoutExtension(resume.FileName).Replace(" ", "-").Replace("_", "-").Replace(";#;", "-") + "__" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();

                if (System.IO.File.Exists(path + currentResume))
                {
                    System.IO.File.Delete(path + currentResume);
                }
                else if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                resume.SaveAs(path + filename + fileExtension);
                newResumeFilename = filename + fileExtension;
            }
            else if (deleteResume)
            {
                if (System.IO.File.Exists(path + currentResume))
                {
                    System.IO.File.Delete(path + currentResume);
                }

                newResumeFilename = null;
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _professorService.UpdateResumeFilename(CurrentUserId, newResumeFilename);
            _uow.SaveAllChanges(isAjaxCaller: false);

            ViewBag.HasInfo = true;
            if (newResumeFilename == currentResume)
            {
                ViewBag.Message = "لطفاً رزومه خود را انتخاب نمائید.";
            }
            else if (newResumeFilename != null)
            {
                ViewBag.Message = "رزومه شما با موفقیت ذخیره شد.";
            }
            else
            {
                ViewBag.Message = "رزومه شما با موفقیت حذف شد.";
            }

            if (newResumeFilename != null)
            {
                ViewBag.ResumeUrl = Url.ActionAbsolute(MVC.Home.GetFile(RijndaelManagedEncryption.EncryptRijndael(CurrentUserId.ToString() + ";#;" + newResumeFilename), 4));
            }

            return View();
        }

        public virtual ActionResult Membership()
        {
            return View(MVC.Dashboard.Professor.Views.Membership);
        }

        public virtual ActionResult Gallery()
        {
            return View(MVC.Dashboard.Professor.Views.Gallery);
        }

        public virtual ActionResult AdminMessage()
        {
            return View(MVC.Dashboard.Professor.Views.AdminMessage);
        }

        public virtual ActionResult WeeklyProgram()
        {
            var programDetails = _professorService.GetWeeklyProgramDetails(CurrentUserId);
            return View(MVC.Dashboard.Professor.Views.WeeklyProgram, programDetails);
        }

        [HttpPost]
        [AjaxOnly]
        [Demo]
        public virtual ActionResult WeeklyProgram(WeeklyProgramDetailsViewModel details)
        {
            _professorService.UpdateWeeklyProgramDetails(CurrentUserId, details); // Also update professor LastUpdateTime field
            _uow.SaveAllChanges();

            return new CamelCaseJsonResult { Data = new { Type = "success", Title = "اطلاعات برنامه هفتگی با موفقیت ذخیره شد." } };
        }

        public virtual ActionResult Settings()
        {
            var settings = _professorService.GetSettings(CurrentUserId);
            return View(MVC.Dashboard.Professor.Views.Settings, settings);
        }

        public virtual ActionResult ExternalArticles()
        {
            return View(MVC.Dashboard.Professor.Views.ExternalArticles);
        }

        public virtual ActionResult ExternalSeminars()
        {
            return View(MVC.Dashboard.Professor.Views.ExternalSeminars);
        }

        public virtual ActionResult InternalArticles()
        {
            return View(MVC.Dashboard.Professor.Views.InternalArticles);
        }

        public virtual ActionResult InternalSeminars()
        {
            return View(MVC.Dashboard.Professor.Views.InternalSeminars);
        }

        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        [Demo]
        public virtual ActionResult Settings(UserSettingsViewModel settings)
        {
            try
            {
                _professorService.UpdateSettings(CurrentUserId, settings);
                _uow.SaveAllChanges();
            }
            catch (Exception)
            {
                return new CamelCaseJsonResult { Data = new { Type = "danger", Title = "در حال حاضر سرور قادر به پاسخگویی نمی باشد." } };
            }

            return new CamelCaseJsonResult { Data = new { Type = "success", Title = "تنظیمات پروفایل شما با موفقیت ذخیره شد." } };
        }

#pragma warning disable SG0016 // Controller method is vulnerable to CSRF
        [HttpPost]
        [AjaxOnly]
        [Demo]
        public virtual async Task<ActionResult> UpdateExternalArticles()
        {
            var userId = CurrentUserId;
            var ids = _professorService.GetOrcidAndResearcherId(userId); // Item1: ORCID, Item2: ResearcherID
            var fetcher = new ArticlesFetcher();
            var articles = new List<ExternalResearchRecord>();
            var researcherIdArticles = new List<ExternalResearchRecord>();
            var orcidArticles = new List<ExternalResearchRecord>();

            if (string.IsNullOrEmpty(ids.Item2) && string.IsNullOrEmpty(ids.Item1))
            {
                return new CamelCaseJsonResult { Data = new { Type = "warning", Title = "لطفاً شناسه ResearcherID و یا ORCID خود را در بخش مشخصات پایه وارد نمائید." } };
            }

            var researcherIdHasError = false;
            if (!string.IsNullOrEmpty(ids.Item2)) // ResearcherID
            {
                try
                {
                    researcherIdArticles = (List<ExternalResearchRecord>)await fetcher.SetProfessorResIdArticles(ids.Item2);
                    researcherIdArticles = researcherIdArticles.Where(a => a.Doi != null).ToList();

                    if (string.IsNullOrEmpty(ids.Item1))
                    {
                        articles = researcherIdArticles;
                    }
                }
                catch (Exception e)
                {
                    researcherIdHasError = true;
                }
            }

            try
            {
                if (!string.IsNullOrEmpty(ids.Item1) && !string.IsNullOrEmpty(ids.Item2) && researcherIdArticles.IsNotNull() && researcherIdArticles.Count > 0 && !researcherIdHasError)
                {
                    orcidArticles = (List<ExternalResearchRecord>)await fetcher.SetProfessorOrcidArticles(ids.Item1);
                    orcidArticles = orcidArticles.Where(a => a.Doi != null).ToList();

                    if (orcidArticles.Count > 0)
                    {
                        //var orcidDois = orcidArticles.Select(o => o.Doi.Trim()).ToList();
                        //var researcherIdDois = researcherIdArticles.Select(r => r.Doi.Trim()).ToList();
                        //var newDois = orcidDois.Where(o => !researcherIdDois.Contains(o)).ToList();
                        //orcidArticles = orcidArticles.Where(o => newDois.Contains(o.Doi.Trim())).ToList();
                        orcidArticles = orcidArticles.Where(o => !researcherIdArticles.Any(r => r.Doi.Trim() == o.Doi.Trim())).ToList();

                        articles = researcherIdArticles.Union(orcidArticles).ToList();
                    }
                    else
                    {
                        articles = researcherIdArticles;
                    }
                }
                else if (!string.IsNullOrEmpty(ids.Item1) && researcherIdHasError) // ORCID (If researcherid throw an error we use orcid to fetch articles)
                {
                    articles = (List<ExternalResearchRecord>)await fetcher.SetProfessorOrcidArticles(ids.Item1);
                    articles = articles.Where(a => a.Doi != null).ToList();
                }
                else if (!string.IsNullOrEmpty(ids.Item1) && string.IsNullOrEmpty(ids.Item2) && !researcherIdHasError) // ORCID (If researcherid is null)
                {
                    articles = (List<ExternalResearchRecord>)await fetcher.SetProfessorOrcidArticles(ids.Item1);
                    articles = articles.Where(a => a.Doi != null).ToList();
                }
                else if (!string.IsNullOrEmpty(ids.Item1) && !string.IsNullOrEmpty(ids.Item2) && !researcherIdHasError && researcherIdArticles.Count <= 0) // ORCID (If in researcherid not exist any article [fallback])
                {
                    articles = (List<ExternalResearchRecord>)await fetcher.SetProfessorOrcidArticles(ids.Item1);
                    articles = articles.Where(a => a.Doi != null).ToList();
                }


                if (articles.Count > 0)
                {
                    _externalResearchService.UpdateExternalArticlesByFetcher(userId, articles);
                    _professorService.UpdateLastUpdateTime(userId, updateExternalArticlesDate: true);
                    _uow.SaveAllChanges();
                }
                else
                {
                    return new CamelCaseJsonResult { Data = new { Type = "success", Title = "متاسفانه هیچ مقاله ای با شناسه شما در ResearcherID و ORCID یافت نشد." } };
                }
            }
            catch (Exception e) when (researcherIdHasError)
            {
                return new CamelCaseJsonResult { Data = new { Type = "danger", Title = "در حال حاضر سرور قادر به پاسخگویی نمی باشد ..." } };
            }
            catch (Exception e) when (researcherIdArticles.Count <= 0)
            {
                return new CamelCaseJsonResult { Data = new { Type = "warning", Title = "متاسفانه هیچ مقاله ای با شناسه شما در ResearcherID یافت نشد." } };
            }
            catch (Exception e) when (researcherIdArticles.Count > 0)
            {
                _externalResearchService.UpdateExternalArticlesByFetcher(userId, researcherIdArticles);
                _professorService.UpdateLastUpdateTime(userId, updateExternalArticlesDate: true);
                _uow.SaveAllChanges();

                return new CamelCaseJsonResult { Data = new { Type = "success", Title = "مقالات شما با موفقیت استخراج شدند." } };
            }

            return new CamelCaseJsonResult { Data = new { Type = "success", Title = "مقالات شما با موفقیت استخراج شدند." } };
        }
#pragma warning restore SG0016 // Controller method is vulnerable to CSRF

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}
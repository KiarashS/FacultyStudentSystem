using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using ContentManagementSystem.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Web.Areas.Dashboard.Controllers
{
    [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
    public partial class LanguageController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ILanguageService _languageService;
        private readonly IProfessorService _professorService;

        public LanguageController(IUnitOfWork uow, ILanguageService languageService, IProfessorService professorService)
        {
            _uow = uow;
            _languageService = languageService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var languages = _languageService.GetLanguages(CurrentUserId);
            return Json(new { Result = "OK", Records = languages });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(LanguageViewModel language)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newLanguage = _languageService.CreateLanguage(CurrentUserId, language);
            _uow.SaveAllChanges();

            language.Id = newLanguage.Id;

            return Json(new { Result = "OK", Record = language });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(LanguageViewModel language)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _languageService.UpdateLanguage(CurrentUserId, language);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _languageService.DeleteLanguage(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
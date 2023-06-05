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
    public partial class ThesisController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IThesisService _thesisService;
        private readonly IProfessorService _professorService;

        public ThesisController(IUnitOfWork uow, IThesisService thesisService, IProfessorService professorService)
        {
            _uow = uow;
            _thesisService = thesisService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var theses = _thesisService.GetThesisList(CurrentUserId);
            return Json(new { Result = "OK", Records = theses });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(ThesisViewModel thesis)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newThesis = _thesisService.CreateThesis(CurrentUserId, thesis);
            _uow.SaveAllChanges();

            thesis.Id = newThesis.Id;

            return Json(new { Result = "OK", Record = thesis });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(ThesisViewModel thesis)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _thesisService.UpdateThesis(CurrentUserId, thesis);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _thesisService.DeleteThesis(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
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
    public partial class ResearchController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IResearchRecordService _researchService;
        private readonly IProfessorService _professorService;

        public ResearchController(IUnitOfWork uow, IResearchRecordService researchService, IProfessorService professorService)
        {
            _uow = uow;
            _researchService = researchService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var researchs = _researchService.GetResearchs(CurrentUserId);
            return Json(new { Result = "OK", Records = researchs });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(ResearchViewModel research)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newResearch = _researchService.CreateResearch(CurrentUserId, research);
            _uow.SaveAllChanges();

            research.Id = newResearch.Id;

            return Json(new { Result = "OK", Record = research });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(ResearchViewModel research)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _researchService.UpdateResearch(CurrentUserId, research);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _researchService.DeleteResearch(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
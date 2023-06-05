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
    public partial class AdministrationController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IAdministrationRecordService _administrationService;
        private readonly IProfessorService _professorService;

        public AdministrationController(IUnitOfWork uow, IAdministrationRecordService administrationService, IProfessorService professorService)
        {
            _uow = uow;
            _administrationService = administrationService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var administrations = _administrationService.GetAdministrations(CurrentUserId);
            return Json(new { Result = "OK", Records = administrations });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(AdministrationViewModel administration)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newAdministration = _administrationService.CreateAdministration(CurrentUserId, administration);
            _uow.SaveAllChanges();

            administration.Id = newAdministration.Id;

            return Json(new { Result = "OK", Record = administration });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(AdministrationViewModel administration)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _administrationService.UpdateAdministration(CurrentUserId, administration);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _administrationService.DeleteAdministration(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
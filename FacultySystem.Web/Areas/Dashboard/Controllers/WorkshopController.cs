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
    public partial class WorkshopController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IWorkshopService _workshopService;
        private readonly IProfessorService _professorService;

        public WorkshopController(IUnitOfWork uow, IWorkshopService workshopService, IProfessorService professorService)
        {
            _uow = uow;
            _workshopService = workshopService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var workshops = _workshopService.GetWorkshops(CurrentUserId);
            return Json(new { Result = "OK", Records = workshops });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(WorkshopViewModel workshop)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newWorkshop = _workshopService.CreateWorkshop(CurrentUserId, workshop);
            _uow.SaveAllChanges();

            workshop.Id = newWorkshop.Id;

            return Json(new { Result = "OK", Record = workshop });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(WorkshopViewModel workshop)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _workshopService.UpdateWorkshop(CurrentUserId, workshop);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _workshopService.DeleteWorkshop(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
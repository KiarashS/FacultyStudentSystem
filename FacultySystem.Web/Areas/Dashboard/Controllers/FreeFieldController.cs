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
    public partial class FreeFieldController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IFreeFieldService _freeFieldService;
        private readonly IProfessorService _professorService;

        public FreeFieldController(IUnitOfWork uow, IFreeFieldService freeFieldService, IProfessorService professorService)
        {
            _uow = uow;
            _freeFieldService = freeFieldService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var freeFields = _freeFieldService.GetListFreeFields(CurrentUserId);
            return Json(new { Result = "OK", Records = freeFields });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(FreeFieldListViewModel freeField)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newFreeField = _freeFieldService.CreateFreeField(CurrentUserId, freeField);
            _uow.SaveAllChanges();

            freeField.Id = newFreeField.Id;

            return Json(new { Result = "OK", Record = freeField });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(FreeFieldListViewModel freeField)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _freeFieldService.UpdateFreeField(CurrentUserId, freeField);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(int id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _freeFieldService.DeleteFreeField(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
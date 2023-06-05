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
    public partial class HonorController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IHonorService _honorService;
        private readonly IProfessorService _professorService;

        public HonorController(IUnitOfWork uow, IHonorService honorService, IProfessorService professorService)
        {
            _uow = uow;
            _honorService = honorService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var honors = _honorService.GetHonors(CurrentUserId);
            return Json(new { Result = "OK", Records = honors });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(HonorViewModel honor)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newHonor = _honorService.CreateHonor(CurrentUserId, honor);
            _uow.SaveAllChanges();

            honor.Id = newHonor.Id;

            return Json(new { Result = "OK", Record = honor });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(HonorViewModel honor)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _honorService.UpdateHonor(CurrentUserId, honor);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _honorService.DeleteHonor(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
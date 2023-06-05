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
    public partial class StudingController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IStudingRecordService _studingService;
        private readonly IProfessorService _professorService;

        public StudingController(IUnitOfWork uow, IStudingRecordService studingService, IProfessorService professorService)
        {
            _uow = uow;
            _studingService = studingService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var studings = _studingService.GetStudings(CurrentUserId);
            return Json(new { Result = "OK", Records = studings });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(StudingViewModel studing)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newStuding = _studingService.CreateStuding(CurrentUserId, studing);
            _uow.SaveAllChanges();

            studing.Id = newStuding.Id;

            return Json(new { Result = "OK", Record = studing });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(StudingViewModel studing)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _studingService.UpdateStuding(CurrentUserId, studing);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _studingService.DeleteStuding(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
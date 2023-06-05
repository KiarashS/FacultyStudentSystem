using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.DomainClasses;
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
    public partial class WeeklyProgramController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProfessorService _professorService;
        private readonly IWeeklyProgramService _programService;

        public WeeklyProgramController(IUnitOfWork uow, IProfessorService professorService, 
            IWeeklyProgramService programService)
        {
            _uow = uow;
            _professorService = professorService;
            _programService = programService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List(byte filterDayOfWeek = 0)
        {
            var programs = _programService.GetListPrograms(CurrentUserId, filterDayOfWeek);
            return Json(new { Result = "OK", Records = programs });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(WeeklyProgramViewModel program)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newProgram = _programService.CreateProgram(CurrentUserId, program);
            _uow.SaveAllChanges();

            program.Id = newProgram.Id;

            return Json(new { Result = "OK", Record = program });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(WeeklyProgramViewModel newProgram)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _programService.UpdateProgram(CurrentUserId, newProgram);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [System.Web.Mvc.HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _programService.DeleteProgram(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}

using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using ContentManagementSystem.Web.Utils;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Web.Areas.Dashboard.Controllers
{
    [SiteAuthorize(Roles = ConstantsUtil.AdminRole)]
    public partial class DefaultFreeFieldController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IDefaultFreeFieldService _defaultFreeFieldService;
        private readonly IProfessorService _professorService;
        private readonly IFreeFieldService _freeFieldService;
        private readonly IActivityLogService _logs;

        public DefaultFreeFieldController(IUnitOfWork uow, IDefaultFreeFieldService defaultFreeFieldService, IProfessorService professorService, 
            IFreeFieldService freeFieldService, IActivityLogService logs)
        {
            _uow = uow;
            _defaultFreeFieldService = defaultFreeFieldService;
            _professorService = professorService;
            _freeFieldService = freeFieldService;
            _logs = logs;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var freeFields = _defaultFreeFieldService.GetListFreeFields();
            return Json(new { Result = "OK", Records = freeFields });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(DefaultFreeFieldListViewModel freeField)
        {
            var newFreeField = _defaultFreeFieldService.CreateFreeField(freeField);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "create",
                Message = $"فیلد آزاد پیش فرض \"{freeField.Name}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            freeField.Id = newFreeField.Id;

            if (freeField.AddToUsers)
            {
                var usersIds = _professorService.GetAllUsersIds();
                _freeFieldService.AddFreeFieldToAll(freeField.Name, freeField.Value, freeField.Order, usersIds);
                _uow.SaveAllChanges();
            }

            return Json(new { Result = "OK", Record = freeField });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(DefaultFreeFieldListViewModel freeField)
        {
            _defaultFreeFieldService.UpdateFreeField(freeField);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "update",
                Message = $"فیلد آزاد پیش فرض \"{freeField.Name}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(int id)
        {
            _defaultFreeFieldService.DeleteFreeField(id);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "delete",
                Message = "فیلد آزاد پیش فرض",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
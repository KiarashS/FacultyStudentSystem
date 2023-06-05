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
    [SiteAuthorize(Roles = ConstantsUtil.AdminRole)]
    public partial class EducationalGroupController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEducationalGroupService _educationalGroupService;
        private readonly IActivityLogService _logs;

        public EducationalGroupController(IUnitOfWork uow, IEducationalGroupService educationalGroupService, IActivityLogService logs)
        {
            _uow = uow;
            _educationalGroupService = educationalGroupService;
            _logs = logs;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var groups = _educationalGroupService.GetListEducationalGroups();
            return Json(new { Result = "OK", Records = groups });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(EducationalGroupViewModel group)
        {
            group.Name = group.Name.Trim();
            if (group.Name == "--")
            {
                return Json(new { Result = "ERROR", Message = "نام وارد شده معتبر نمی باشد." });
            }

            var newGroup = _educationalGroupService.CreateEducationalGroup(group);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "create",
                Message = $"گروه آموزشی \"{group.Name}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            group.Id = newGroup.Id;

            return Json(new { Result = "OK", Record = group });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(EducationalGroupViewModel group)
        {
            group.Name = group.Name.Trim();
            if (group.Name == "--")
            {
                return Json(new { Result = "ERROR", Message = "نام وارد شده معتبر نمی باشد." });
            }

            _educationalGroupService.UpdateEducationalGroup(group);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "update",
                Message = $"گروه آموزشی \"{group.Name}\"",
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
            var result = _educationalGroupService.DeleteEducationalGroup(id);
            if (!result)
            {
                return Json(new { Result = "ERROR", Message = "گروه آموزشی مورد نظر را نمی توانید حذف کنید چون در حال استفاده توسط کاربران می باشد. در صورت لزوم می توانید آن را ویرایش کنید." });
            }

            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "delete",
                Message = "گروه آموزشی",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();
            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult CheckName(int id, string name)
        {
            name = name.Trim();
            if (_educationalGroupService.ExistName(id, name))
            {
                return new HttpStatusCodeResult(404);
            }

            return new HttpStatusCodeResult(200);
        }
    }
}
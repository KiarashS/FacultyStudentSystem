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
    [SiteAuthorize(Roles = ConstantsUtil.AdminRole)]
    public partial class EducationalDegreeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IEducationalDegreeService _educationalDegreeService;
        private readonly IActivityLogService _logs;

        public EducationalDegreeController(IUnitOfWork uow, IEducationalDegreeService educationalDegreeService, IActivityLogService logs)
        {
            _uow = uow;
            _educationalDegreeService = educationalDegreeService;
            _logs = logs;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var degrees = _educationalDegreeService.GetListEducationalDegrees();
            return Json(new { Result = "OK", Records = degrees });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(EducationalDegreeViewModel degree)
        {
            degree.Name = degree.Name.Trim();
            if (degree.Name == "--")
            {
                return Json(new { Result = "ERROR", Message = "نام وارد شده معتبر نمی باشد." });
            }

            var newDegree = _educationalDegreeService.CreateEducationalDegree(degree);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "create",
                Message = $"درجه تحصیلی \"{degree.Name}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            degree.Id = newDegree.Id;

            return Json(new { Result = "OK", Record = degree });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(EducationalDegreeViewModel degree)
        {
            degree.Name = degree.Name.Trim();
            if (degree.Name == "--")
            {
                return Json(new { Result = "ERROR", Message = "نام وارد شده معتبر نمی باشد." });
            }

            _educationalDegreeService.UpdateEducationalDegree(degree);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "update",
                Message = $"درجه تحصیلی \"{degree.Name}\"",
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
            var result = _educationalDegreeService.DeleteEducationalDegree(id);
            if (!result)
            {
                return Json(new { Result = "ERROR", Message = "درجه تحصیلی مورد نظر را نمی توانید حذف کنید چون در حال استفاده توسط کاربران می باشد. در صورت لزوم می توانید آن را ویرایش کنید." });
            }

            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "delete",
                Message = "درجه تحصیلی",
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
            if (_educationalDegreeService.ExistName(id, name))
            {
                return new HttpStatusCodeResult(404);
            }

            return new HttpStatusCodeResult(200);
        }
    }
}
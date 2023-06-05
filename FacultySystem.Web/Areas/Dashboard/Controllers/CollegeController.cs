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
    public partial class CollegeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ICollegeService _collegeService;
        private readonly IActivityLogService _logs;

        public CollegeController(IUnitOfWork uow, ICollegeService collegeService, IActivityLogService logs)
        {
            _uow = uow;
            _collegeService = collegeService;
            _logs = logs;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var colleges = _collegeService.GetListColleges();
            return Json(new { Result = "OK", Records = colleges });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(CollegeViewModel college)
        {
            college.Name = college.Name.Trim();
            if (college.Name == "--")
            {
                return Json(new { Result = "ERROR", Message = "نام وارد شده معتبر نمی باشد." });
            }

            var newCollege = _collegeService.CreateCollege(college);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "create",
                Message = $"دانشکده \"{college.Name}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            college.Id = newCollege.Id;

            return Json(new { Result = "OK", Record = college });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(CollegeViewModel college)
        {
            college.Name = college.Name.Trim();
            if (college.Name == "--")
            {
                return Json(new { Result = "ERROR", Message = "نام وارد شده معتبر نمی باشد." });
            }

            _collegeService.UpdateCollege(college);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "update",
                Message = $"دانشکده \"{college.Name}\"",
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
            var result = _collegeService.DeleteCollege(id);
            if (!result)
            {
                return Json(new { Result = "ERROR", Message = "دانشکده مورد نظر را نمی توانید حذف کنید چون در حال استفاده توسط کاربران می باشد. در صورت لزوم می توانید آن را ویرایش کنید." });
            }

            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "delete",
                Message = "دانشکده",
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
            if (_collegeService.ExistName(id, name))
            {
                return new HttpStatusCodeResult(404);
            }

            return new HttpStatusCodeResult(200);
        }
    }
}
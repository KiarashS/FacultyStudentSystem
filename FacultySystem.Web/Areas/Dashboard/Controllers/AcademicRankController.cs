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
    public partial class AcademicRankController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IAcademicRankService _academicRankService;
        private readonly IActivityLogService _logs;

        public AcademicRankController(IUnitOfWork uow, IAcademicRankService academicRankService, IActivityLogService logs)
        {
            _uow = uow;
            _academicRankService = academicRankService;
            _logs = logs;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var ranks = _academicRankService.GetListAcademicRanks();
            return Json(new { Result = "OK", Records = ranks });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(AcademicRankViewModel rank)
        {
            rank.Name = rank.Name.Trim();
            if (rank.Name == "--")
            {
                return Json(new { Result = "ERROR", Message = "نام وارد شده معتبر نمی باشد." });
            }

            var newRank = _academicRankService.CreateAcademicRank(rank);
            _logs.CreateActivityLog(new ActivityLogViewModel {
                ActionBy = CurrentUserName,
                ActionType = "create",
                Message = $"مرتبه علمی \"{rank.Name}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            rank.Id = newRank.Id;

            return Json(new { Result = "OK", Record = rank });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(AcademicRankViewModel rank)
        {
            rank.Name = rank.Name.Trim();
            if (rank.Name == "--")
            {
                return Json(new { Result = "ERROR", Message = "نام وارد شده معتبر نمی باشد." });
            }

            _academicRankService.UpdateAcademicRank(rank);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "update",
                Message = $"مرتبه علمی \"{rank.Name}\"",
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
            var result = _academicRankService.DeleteAcademicRank(id);
            if (!result)
            {
                return Json(new { Result = "ERROR", Message = "مرتبه علمی مورد نظر را نمی توانید حذف کنید چون در حال استفاده توسط کاربران می باشد. در صورت لزوم می توانید آن را ویرایش کنید." });
            }

            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "delete",
                Message = "مرتبه علمی",
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
            if (_academicRankService.ExistName(id, name))
            {
                return new HttpStatusCodeResult(404);
            }

            return new HttpStatusCodeResult(200);
        }
    }
}
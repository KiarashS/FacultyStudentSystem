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
using System.Web.Http;
using System.Web.Mvc;

namespace ContentManagementSystem.Web.Areas.Dashboard.Controllers
{
    [SiteAuthorize(Roles = ConstantsUtil.AdminRole)]
    public partial class CitationController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProfessorService _professorService;
        private readonly IUserService _userService;
        private readonly IDocumentCitationService _citationService;
        private readonly IActivityLogService _logs;

        public CitationController(IUnitOfWork uow, IProfessorService professorService, 
            IDocumentCitationService citationService, IActivityLogService logs,
            IUserService userService)
        {
            _uow = uow;
            _professorService = professorService;
            _citationService = citationService;
            _logs = logs;
            _userService = userService;
        }

        [System.Web.Mvc.HttpPost]
        [AjaxOnly]
        public virtual ActionResult List(string filterLastname, string filterPageId, string filterEmail, int jtStartIndex = 0, int jtPageSize = 20)
        {
            var hIndexes = _professorService.GetHIndexList(filterLastname, filterPageId, filterEmail, jtStartIndex, jtPageSize);
            var totalRecordsCount = _professorService.GetHIndexCount(filterLastname, filterPageId, filterEmail);

            return Json(new { Result = "OK", TotalRecordCount = totalRecordsCount, Records = hIndexes });
        }

        [System.Web.Mvc.HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(HIndexManagementViewModel hIndex)
        {
            var userDetails = _userService.GetTopMenuInfo(hIndex.UserId);

            _professorService.UpdateHIndex(hIndex.UserId, hIndex); // Also professor LastUpdateTime field updated in this method
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "update",
                Message = $"بروزرسانی H-Index \"{userDetails.Fullname}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [System.Web.Mvc.HttpPost]
        [AjaxOnly]
        public virtual ActionResult ListCitations(int userId, int jtStartIndex = 0, int jtPageSize = 10)
        {
            var citations = _citationService.GetListCitations(userId, jtStartIndex, jtPageSize);
            var totalRecordsCount = _citationService.GetListCitationCount(userId);

            return Json(new { Result = "OK", TotalRecordCount = totalRecordsCount, Records = citations });
        }

        [System.Web.Mvc.HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult CreateCitation(int userId, int citation, int year, int? document, DocSource source)
        {
            var newCitation = new DocumentCitationViewModel
            {
                Year = year,
                Citation = citation,
                Source = source,
                Document = document
            };

            if (_citationService.IsExist(userId, year, source))
            {
                return Json(new { Result = "ERROR", Message = "اطلاعات وارد شده موجود می باشند، در صورت نیاز می توانید آن را ویرایش نمائید." });
            }

            var userDetails = _userService.GetTopMenuInfo(userId);

            _professorService.UpdateLastUpdateTime(userId, updateHIndexAndCitationDate: true);
            var newCitationEntity = _citationService.CreateCitation(userId, newCitation);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "create",
                Message = $"ایجاد ارجاع در سال {year} برای \"{userDetails.Fullname}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            newCitation.Id = newCitationEntity.Id;

            return Json(new { Result = "OK", Record = newCitation });
        }

        [System.Web.Mvc.HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult UpdateCitation(int userId, long id, int citation, int year, int? document, DocSource source)
        {
            var newCitation = new DocumentCitationViewModel
            {
                Id = id,
                Year = year,
                Citation = citation,
                Source = source,
                Document = document
            };

            if (_citationService.IsExist(userId, year, source))
            {
                return Json(new { Result = "ERROR", Message = "اطلاعات وارد شده موجود می باشند، در صورت نیاز می توانید آن را ویرایش نمائید." });
            }

            var userDetails = _userService.GetTopMenuInfo(userId);

            _professorService.UpdateLastUpdateTime(userId, updateHIndexAndCitationDate: true);
            _citationService.UpdateCitation(userId, newCitation);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "update",
                Message = $"بروزرسانی ارجاع در سال {year} برای \"{userDetails.Fullname}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [System.Web.Mvc.HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult DeleteCitation(int userId, long id)
        {
            var userDetails = _userService.GetTopMenuInfo(userId);

            _professorService.UpdateLastUpdateTime(userId, updateHIndexAndCitationDate: true);
            _citationService.DeleteCitation(userId, id);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "delete",
                Message = $"حذف ارجاع برای \"{userDetails.Fullname}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
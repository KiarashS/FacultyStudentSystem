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
    public partial class ActivityLogController : BaseController
    {
        //private readonly IUnitOfWork _uow;
        private readonly IActivityLogService _logs;

        public ActivityLogController(/*IUnitOfWork uow, */IActivityLogService logs)
        {
            //_uow = uow;
            _logs = logs;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List(string filterEmail, int jtStartIndex = 0, int jtPageSize = 10)
        {
            jtPageSize = 50;
            var activityLogs = _logs.GetActivityLogs(filterEmail, jtStartIndex, jtPageSize);
            var totalRecordsCount = _logs.TotalLogsCount(filterEmail);

            return Json(new { Result = "OK", TotalRecordCount = totalRecordsCount, Records = activityLogs });
        }
    }
}
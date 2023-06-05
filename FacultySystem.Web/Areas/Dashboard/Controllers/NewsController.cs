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
    public partial class NewsController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly INewsService _newsService;
        private readonly IActivityLogService _logs;

        public NewsController(IUnitOfWork uow, INewsService newsService, IActivityLogService logs)
        {
            _uow = uow;
            _newsService = newsService;
            _logs = logs;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List(int jtStartIndex = 0, int jtPageSize = 20)
        {
            var newsList = _newsService.GetListNews(jtStartIndex, jtPageSize);
            var newsCount = _newsService.GetNewsCount();

            return Json(new { Result = "OK", TotalRecordCount = newsCount, Records = newsList });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(NewsViewModel news)
        {
            var newNews = _newsService.CreateNews(news);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "create",
                Message = $"درج خبر جدید با عنوان \"{news.Title}\"",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            news.Id = newNews.Id;

            return Json(new { Result = "OK", Record = news });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(NewsViewModel news)
        {
            _newsService.UpdateNews(news);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "update",
                Message = $"بروزرسانی خبر با عنوان \"{news.Title}\"",
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
            _newsService.DeleteNews(id);
            _logs.CreateActivityLog(new ActivityLogViewModel
            {
                ActionBy = CurrentUserName,
                ActionType = "delete",
                Message = "حذف خبر",
                SourceAddress = Request.UserHostAddress,
                Url = Request.RawUrl
            });
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
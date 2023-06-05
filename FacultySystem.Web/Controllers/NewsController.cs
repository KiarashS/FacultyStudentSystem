using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Web.Controllers
{
    public partial class NewsController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly INewsService _newsService;

        public NewsController(IUnitOfWork uow, INewsService newsService)
        {
            _uow = uow;
            _newsService = newsService;
        }


        public virtual ActionResult Index(int page = 1)
        {
            var pageSize = int.Parse(ConfigurationManager.AppSettings["NewsPageSize"]);
            if(pageSize == 0)
            {
                return Redirect("/");
            }

            var newsCount = _newsService.GetNewsCount();
            var newsList = _newsService.NewsList(page, pageSize);
            if(newsCount == 0)
            {
                return RedirectToAction(MVC.Home.ActionNames.Index, MVC.Home.Name);
            }

            var pageLinks = NextAndPreviousPages(newsCount, pageSize, page);
            ViewBag.PreviousPageLink = pageLinks.Item1;
            ViewBag.NextPageLink = pageLinks.Item2;
            ViewBag.Title = "لیست اخبار";

            if (Request.IsAjaxRequest())
            {
                return PartialView(MVC.News.Views._NewsList, newsList);
            }
            return View(newsList);
        }

        public virtual ActionResult Detail(int? id)
        {
            if (!id.HasValue)
            {
                return Redirect("/");
            }

            var pageSize = int.Parse(ConfigurationManager.AppSettings["NewsPageSize"]);
            if (pageSize == 0)
            {
                return Redirect("/");
            }

            var news = _newsService.GetNewsDetail((int)id);
            if(news == null)
            {
                return Redirect("/");
            }

            ViewBag.Title = $"خبر: {news.Title}";
            return View(news);
        }

        private Tuple<string, string> NextAndPreviousPages(int totalCount, int pageSize, int currentPage)
        {
            var totalPages = Math.Ceiling(decimal.Divide(totalCount, pageSize));
            string previousPageLink = null;
            string nextPageLink = null;

            if (currentPage == 1 && totalPages > currentPage)
            {
                nextPageLink = Url.Action(MVC.News.Index(currentPage + 1));
            }
            else if (currentPage != 1 && currentPage == totalPages)
            {
                previousPageLink = Url.Action(MVC.News.Index(currentPage - 1));
            }
            else if (currentPage > 1)
            {
                previousPageLink = Url.Action(MVC.News.Index(currentPage - 1));
                nextPageLink = Url.Action(MVC.News.Index(currentPage + 1));
            }

            return new Tuple<string, string>(previousPageLink, nextPageLink);
        }
    }
}
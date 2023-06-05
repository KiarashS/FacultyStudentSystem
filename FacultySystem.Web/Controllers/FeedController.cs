using ContentManagementSystem.Commons.Web;
using ContentManagementSystem.Commons.Web.MvcRss;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Web.Controllers
{
    public partial class FeedController : Controller
    {
        const int Min15 = 900;
        private readonly INewsService _newsService;

        public FeedController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [OutputCache(Duration = Min15)]
        public virtual ActionResult Index()
        {
            var newsFeedSize = int.Parse(ConfigurationManager.AppSettings["NewsFeedSize"]);
            IList<NewsFeedViewModel> newsList = new List<NewsFeedViewModel>();
            if (newsFeedSize > 0)
            {
                newsList = _newsService.NewsFeed(size: newsFeedSize);
            }
            var feedItemsList = mapNewsToFeedItems(newsList);
            return new FeedResult($"فيد اخبار {ConfigurationManager.AppSettings["SystemNameFa"]}", feedItemsList);
        }

        private List<FeedItem> mapNewsToFeedItems(IList<NewsFeedViewModel> list)
        {
            var feedItemsList = new List<FeedItem>();
            foreach (var item in list)
            {
                feedItemsList.Add(new FeedItem
                {
                    AuthorName = item.AuthorName,
                    Content = item.Body.Replace("\n", "<br />").RemoveHexadecimalSymbols(),
                    LastUpdatedTime = item.Date.DateTime.UtcToLocalDateTime(),
                    PublishDate = item.Date.DateTime.UtcToLocalDateTime(),
                    Title = item.Title.RemoveHexadecimalSymbols(),
                    Url = this.Url.Action(actionName: "Detail", controllerName: "News", routeValues: new { id = item.Id, title = item.Title }, protocol: "http")
                });
            }
            return feedItemsList;
        }
    }
}
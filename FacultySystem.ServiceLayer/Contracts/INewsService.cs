using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface INewsService
    {
        IEnumerable<NewsViewModel> GetListNews(int startIndex = 0, int pageSize = 20);
        News CreateNews(NewsViewModel news);
        void UpdateNews(NewsViewModel newNews);
        void DeleteNews(int id);
        int GetNewsCount();
        IEnumerable<NewsViewModel> GetNewsTicker(int count = 5);
        IEnumerable<NewsViewModel> NewsList(int page = 1, int pageSize = 20);
        NewsViewModel GetNewsDetail(int id);
        IList<NewsFeedViewModel> NewsFeed(int page = 1, int size = 20);
    }
}

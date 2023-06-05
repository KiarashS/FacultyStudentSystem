using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.DataLayer.Context;
using System.Data.Entity;
using ContentManagementSystem.DomainClasses;
using EFSecondLevelCache;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFNewsService : INewsService
    {
        IUnitOfWork _uow;
        readonly IDbSet<News> _news;
        public EFNewsService(IUnitOfWork uow)
        {
            _uow = uow;
            _news = _uow.Set<News>();
        }

        public IEnumerable<NewsViewModel> GetListNews(int startIndex = 0, int pageSize = 20)
        {
            var newsList = new List<NewsViewModel>();
            var news = _news
                .OrderByDescending(n => n.CreateDate)
                .Skip(startIndex)
                .Take(pageSize)
                .Cacheable()
                .ToList();

            foreach (var item in news)
            {
                newsList.Add(new NewsViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    Content = item.Content,
                    CreateDate = item.CreateDate
                });
            }

            return newsList;
        }

        public IEnumerable<NewsViewModel> NewsList(int page = 1, int pageSize = 15)
        {
            var pageIndex = page >= 1 ? page - 1 : 0;
            var newsList = new List<NewsViewModel>();
            var news = _news
                .OrderByDescending(n => n.CreateDate)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Cacheable()
                .ToList();

            foreach (var item in news)
            {
                newsList.Add(new NewsViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    Content = item.Content,
                    CreateDate = item.CreateDate
                });
            }

            return newsList;
        }

        public News CreateNews(NewsViewModel news)
        {
            var newNews = _news.Add(new News
            {
                Title = news.Title,
                Content = news.Content
            });

            return newNews;
        }

        public void UpdateNews(NewsViewModel newNews)
        {
            var news = _news.Single(n => n.Id == newNews.Id);

            news.Title = newNews.Title;
            news.Content = newNews.Content;
        }

        public void DeleteNews(int id)
        {
            var news = _news.Single(n => n.Id == id);
            _news.Remove(news);
        }

        public int GetNewsCount()
        {
            return _news.Count();
        }

        public IEnumerable<NewsViewModel> GetNewsTicker(int count = 5)
        {
            var newsList = new List<NewsViewModel>();
            var news = _news
                .Select(n => new { n.Id, n.Title, n.CreateDate })
                .OrderByDescending(n => n.CreateDate)
                .Take(count)
                .Cacheable()
                .ToList();

            foreach (var item in news)
            {
                newsList.Add(new NewsViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    CreateDate = item.CreateDate
                });
            }

            return newsList;
        }

        public NewsViewModel GetNewsDetail(int id)
        {
            var news = _news.Where(n => n.Id == id).Cacheable().SingleOrDefault();

            if (news == null)
            {
                return null;
            }

            return new NewsViewModel
            {
                Id = news.Id,
                CreateDate = news.CreateDate,
                Title = news.Title,
                Content = news.Content
            };
        }

        public IList<NewsFeedViewModel> NewsFeed(int page = 1, int size = 20)
        {
            var startIndex = (page - 1) * size;
            var newsList = new List<NewsFeedViewModel>();
            var news = _news
                .OrderByDescending(n => n.CreateDate)
                .Skip(startIndex)
                .Take(size)
                .Cacheable()
                .ToList();

            foreach (var item in news)
            {
                newsList.Add(new NewsFeedViewModel
                {
                    Id = item.Id,
                    Title = item.Title,
                    Body = item.Content,
                    Date = item.CreateDate
                });
            }

            return newsList;
        }
    }
}

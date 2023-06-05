using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentManagementSystem.Models.ViewModels;
using EFSecondLevelCache;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFLessonNewsService: ILessonNewsService
    {
        IUnitOfWork _uow;
        readonly IDbSet<LessonNews> _lessonNews;
        //private readonly Lazy<Professor> _professorService;
        public EFLessonNewsService(IUnitOfWork uow)
        {
            _uow = uow;
            _lessonNews = _uow.Set<LessonNews>();
        }

        public IEnumerable<LessonNewsViewModel> GetLessonNews(int userId, long lessonId)
        {
            var newsList = new List<LessonNewsViewModel>();
            var news = _lessonNews
                .Where(l => l.ProfessorId == userId && l.LessonId == lessonId)
                .OrderByDescending(l => l.Order)
                .ThenByDescending(l => l.Id)
                .Cacheable()
                .ToList();

            foreach (var @new in news)
            {
                newsList.Add(new LessonNewsViewModel
                {
                    Id = @new.Id,
                    Title = @new.Title,
                    Content = @new.Content,
                    NewsColor = @new.NewsColor,
                    CreateDate = @new.CreateDate,
                    Link = @new.Link,
                    Order = @new.Order
                });
            }

            return newsList;
        }

        public LessonNews CreateLessonNews(int userId, LessonNewsViewModel lessonNews)
        {
            var newNews = _lessonNews.Add(new LessonNews
            {
                ProfessorId = userId,
                LessonId = lessonNews.LessonId,
                Title = lessonNews.Title,
                Content = lessonNews.Content,
                NewsColor = lessonNews.NewsColor,
                Link = lessonNews.Link,
                Order = lessonNews.Order
            });

            return newNews;
        }

        public void UpdateLessonNews(int userId, LessonNewsViewModel newNews)
        {
            var news = _lessonNews.Single(l => l.ProfessorId == userId && l.Id == newNews.Id);

            news.Title = newNews.Title;
            news.Content = newNews.Content;
            news.NewsColor = newNews.NewsColor;
            news.Link = newNews.Link;
            news.Order = newNews.Order;
        }

        public void DeleteLessonNews(int userId, long id)
        {
            var lessonNews = _lessonNews.Single(l => l.ProfessorId == userId && l.Id == id);
            _lessonNews.Remove(lessonNews);
        }
    }
}

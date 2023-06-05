using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface ILessonNewsService
    {
        IEnumerable<LessonNewsViewModel> GetLessonNews(int userId, long lessonId);
        LessonNews CreateLessonNews(int userId, LessonNewsViewModel news);
        void UpdateLessonNews(int userId, LessonNewsViewModel newNews);
        void DeleteLessonNews(int userId, long id);
    }
}

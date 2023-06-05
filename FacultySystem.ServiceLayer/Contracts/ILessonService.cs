using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface ILessonService
    {
        IEnumerable<LessonViewModel> GetLessons(int userId);
        Lesson CreateLesson(int userId, LessonViewModel lesson);
        void UpdateLesson(int userId, LessonViewModel newLesson);
        void DeleteLesson(int userId, long id);
        LessonIndexViewModel LessonIndex(int userId, long lessonId);
        void DeleteAllLesson(int userId);
    }
}

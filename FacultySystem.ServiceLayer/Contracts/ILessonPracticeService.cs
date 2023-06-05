using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface ILessonPracticeService
    {
        IEnumerable<LessonPracticesViewModel> GetLessonPractices(int userId, long lessonId);
        LessonPractices CreateLessonPractice(int userId, LessonPracticesViewModel practice);
        void UpdateLessonPractice(int userId, LessonPracticesViewModel newPractice);
        void DeleteLessonPractice(int userId, long id);
        string GetFilename(int userId, long id);
    }
}

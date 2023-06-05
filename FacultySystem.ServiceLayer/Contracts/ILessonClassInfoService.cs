using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface ILessonClassInfoService
    {
        IEnumerable<LessonClassInfoViewModel> GetLessonClasses(int userId, long lessonId);
        LessonClassInfo CreateLessonClass(int userId, LessonClassInfoViewModel lessonClass);
        void UpdateLessonClass(int userId, LessonClassInfoViewModel newLessonClass);
        void DeleteLessonClass(int userId, long id);
    }
}

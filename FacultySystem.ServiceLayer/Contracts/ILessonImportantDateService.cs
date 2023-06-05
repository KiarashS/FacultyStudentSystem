using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface ILessonImportantDateService
    {
        IEnumerable<LessonImportantDateViewModel> GetImportantDates(int userId, long lessonId);
        LessonImportantDate CreateImportantDate(int userId, LessonImportantDateViewModel date);
        void UpdateImportantDate(int userId, LessonImportantDateViewModel newDate);
        void DeleteImportantDate(int userId, long id);
    }
}

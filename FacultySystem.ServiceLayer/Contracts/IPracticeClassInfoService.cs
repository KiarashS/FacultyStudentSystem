using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IPracticeClassInfoService
    {
        IEnumerable<PracticeClassInfoViewModel> GetPracticeClasses(int userId, long lessonId);
        PracticeClassInfo CreatePracticeClass(int userId, PracticeClassInfoViewModel practiceClass);
        void UpdatePracticeClass(int userId, PracticeClassInfoViewModel newPracticeClass);
        void DeletePracticeClass(int userId, long id);
    }
}

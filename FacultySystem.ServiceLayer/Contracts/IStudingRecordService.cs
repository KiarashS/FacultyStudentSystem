using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IStudingRecordService
    {
        IEnumerable<StudingViewModel> GetStudings(int userId);
        StudingRecord CreateStuding(int userId, StudingViewModel studing);
        void UpdateStuding(int userId, StudingViewModel newStuding);
        void DeleteStuding(int userId, long id);
    }
}

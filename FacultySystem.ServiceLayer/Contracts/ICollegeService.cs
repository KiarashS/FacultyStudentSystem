using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface ICollegeService
    {
        IEnumerable<CollegeViewModel> GetListColleges();
        IEnumerable<CollegeViewModel> GetColleges();
        College CreateCollege(CollegeViewModel college);
        void UpdateCollege(CollegeViewModel newCollege);
        bool DeleteCollege(int id);
        bool ExistName(int id, string name);
        int GetIdByName(string name);
        int NumberOfColleges();
    }
}

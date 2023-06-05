using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IEducationalDegreeService
    {
        IEnumerable<EducationalDegreeViewModel> GetListEducationalDegrees();
        IEnumerable<EducationalDegreeViewModel> GetEducationalDegrees();
        EducationalDegree CreateEducationalDegree(EducationalDegreeViewModel educationalDegree);
        void UpdateEducationalDegree(EducationalDegreeViewModel newEducationalDegree);
        bool DeleteEducationalDegree(int id);
        bool ExistName(int id, string name);
        int GetIdByName(string name);
    }
}

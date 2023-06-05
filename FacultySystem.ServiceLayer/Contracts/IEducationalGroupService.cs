using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IEducationalGroupService
    {
        IEnumerable<EducationalGroupViewModel> GetListEducationalGroups();
        IEnumerable<EducationalGroupViewModel> GetEducationalGroups();
        EducationalGroup CreateEducationalGroup(EducationalGroupViewModel educationalGroup);
        void UpdateEducationalGroup(EducationalGroupViewModel newEducationalGroup);
        bool DeleteEducationalGroup(int id);
        bool ExistName(int id, string name);
        int GetIdByName(string name);
        int NumberOfEducationalGroups();
    }
}

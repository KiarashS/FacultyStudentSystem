using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IThesisService
    {
        IEnumerable<ThesisViewModel> GetThesisList(int userId);
        Thesis CreateThesis(int userId, ThesisViewModel thesis);
        void UpdateThesis(int userId, ThesisViewModel newThesis);
        void DeleteThesis(int userId, long id);
    }
}

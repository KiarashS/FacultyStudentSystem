using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IAdministrationRecordService
    {
        IEnumerable<AdministrationViewModel> GetAdministrations(int userId);
        AdministrationRecord CreateAdministration(int userId, AdministrationViewModel administration);
        void UpdateAdministration(int userId, AdministrationViewModel newAdministration);
        void DeleteAdministration(int userId, long id);
    }
}

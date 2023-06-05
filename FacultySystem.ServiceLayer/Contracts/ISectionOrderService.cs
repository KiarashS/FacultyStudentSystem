using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface ISectionOrderService
    {
        IEnumerable<SectionsOrderViewModel> GetSectionOrders(int userId);
        void CreateSections(int userId);
        void UpdateSections(int userId, Dictionary<SectionName, int> orders);
        void DeleteSections(int userId);
    }
}

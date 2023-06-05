using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IDefaultFreeFieldService
    {
        IEnumerable<DefaultFreeFieldListViewModel> GetListFreeFields();
        DefaultFreeField CreateFreeField(DefaultFreeFieldListViewModel freeField);
        void UpdateFreeField(DefaultFreeFieldListViewModel newFreeField);
        void DeleteFreeField(int id);
    }
}

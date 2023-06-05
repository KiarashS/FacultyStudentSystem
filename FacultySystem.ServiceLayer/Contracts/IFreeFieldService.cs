using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IFreeFieldService
    {
        IEnumerable<FreeFieldListViewModel> GetListFreeFields(int userId);
        FreeField CreateFreeField(int userId, FreeFieldListViewModel freeField);
        void CreateFreeFields(int userId, IEnumerable<DefaultFreeFieldListViewModel> freeFields);
        void UpdateFreeField(int userId, FreeFieldListViewModel newFreeField);
        void DeleteFreeField(int userId, int freeFieldId);
        void AddFreeFieldToAll(string name, string value, int? order, IList<int> usersIds);
    }
}

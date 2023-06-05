using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IHonorService
    {
        IEnumerable<HonorViewModel> GetHonors(int userId);
        Honor CreateHonor(int userId, HonorViewModel honor);
        void UpdateHonor(int userId, HonorViewModel newHonor);
        void DeleteHonor(int userId, long id);
    }
}

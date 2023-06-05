using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IMembershipService
    {
        IEnumerable<ProfessorMembershipViewModel> GetMemberships(int userId);
        ProfessorMembership CreateMembership(int userId, ProfessorMembershipViewModel membership);
        void UpdateMembership(int userId, ProfessorMembershipViewModel newMembership);
        void DeleteMembership(int userId, long id);
    }
}

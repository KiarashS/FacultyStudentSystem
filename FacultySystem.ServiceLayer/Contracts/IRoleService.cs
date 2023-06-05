using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IRoleService
    {
        IEnumerable<Role> GetRoles(string[] rolesName);
        Role GetRole(string roleName);
        Role AttachRole(Role role);
        void DetachRoles(IEnumerable<Role> roles);
        IEnumerable<Role> GetRoles(int[] rolesId);
    }
}

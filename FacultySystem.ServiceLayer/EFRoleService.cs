using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.DataLayer.Context;
using System.Data.Entity;
using EFSecondLevelCache;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFRoleService : IRoleService
    {
        IUnitOfWork _uow;
        readonly IDbSet<Role> _roles;
        //private readonly Lazy<Professor> _professorService;
        public EFRoleService(IUnitOfWork uow)
        {
            _uow = uow;
            _roles = _uow.Set<Role>();
        }

        public IEnumerable<Role> GetRoles(string[] rolesName)
        {
            return _roles.Where(r => rolesName.Contains(r.Name)).ToList();
        }

        public Role GetRole(string roleName)
        {
            return _roles.Where(r => r.Name == roleName).Cacheable().Single();
        }

        public Role AttachRole(Role role)
        {
            return _roles.Attach(role);
        }

        public void DetachRoles(IEnumerable<Role> roles)
        {
            foreach (var role in roles)
            {
                _uow.Entry(role).State = EntityState.Detached;
            }
        }

        public IEnumerable<Role> GetRoles(int[] rolesId)
        {
            return _roles.Where(r => rolesId.Contains(r.Id)).ToList();
        }
    }
}

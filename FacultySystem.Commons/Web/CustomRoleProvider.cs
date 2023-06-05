using System;
using System.Web.Security;
using ContentManagementSystem.ServiceLayer.Contracts;
using StructureMap;
using ContentManagementSystem.IocConfig;

namespace ContentManagementSystem.Commons.Web
{
    public class CustomRoleProvider : RoleProvider
    {
        public override bool IsUserInRole(string username, string roleName)
        {
            var memberService = SmObjectFactory.Container.GetInstance<IUserService>();
            var userName = username.Split(new string[] { ",#;" }, StringSplitOptions.None)[0];
            return memberService.IsUserInRole(userName.ToLowerInvariant(), roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            var memberService = SmObjectFactory.Container.GetInstance<IUserService>();
            var userName = username.Split(new string[] { ",#;" }, StringSplitOptions.None)[0];
            return memberService.GetRolesForUser(userName.ToLowerInvariant());
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}
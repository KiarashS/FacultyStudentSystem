using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IUserService
    {
        bool IsUserInRole(string email, string roleName);
        bool IsUserInRole(int userId, string roleName);
        string[] GetRolesForUser(string email);
        IEnumerable<string> GetRolesForUser(int userId);
        IEnumerable<string> GetRolesForUserAndDetach(int userId);
        string GetPassword(string email);
        string GetPasswordById(int userId);
        LogonViewModel GetInfoForLogon(string email);
        bool IsExistUser(string email);
        void UpdatePasswordByEmail(string email, string newPassword);
        User UpdatePasswordById(int userId, string newPassword);
        void UpdateNote(int userId, string note);
        int GetUserId(string email);
        string GetNote(int userId);
        UserTopMenuViewModel GetTopMenuInfo(int userId);
        bool IsUniqueEmail(int userId, string newEmail);
        void UpdateDetails(int userId, DetailsViewModel info);
        void UpdateLastLogonTimeAndIp(string email, string ip);
        DetailsViewModel GetAdminDashboardDetails(int userId);
        IEnumerable<UserListViewModel> GetListUsers(string filterFirstname, string filterLastname, string filterEmail, string filterPageId, string filterRole = "notdefined", bool filterIsBanned = false, bool filterIsSoftDelete = false, int startIndex = 0, int pageSize = 10);
        User CreateUser(UserListViewModel user, string password, string[] userRoles);
        void UpdateUser(UserListViewModel updatedUser, string[] userRoles, bool isProfessor, bool isRolesChanged);
        void DeleteUser(int userId);
        int TotalUsersCount(string filterFirstname, string filterLastname, string filterEmail, string filterPageId, string filterRole = "notdefined", bool filterIsBanned = false, bool filterIsSoftDelete = false);
        bool IsFirstLogin(int userId);
    }
}

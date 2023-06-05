using System;
using System.Data.Entity;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.ServiceLayer.Contracts;
using System.Linq;
using EFSecondLevelCache;
using ContentManagementSystem.Models.ViewModels;
using System.Collections.Generic;
using Z.EntityFramework.Plus;

namespace ContentManagementSystem.ServiceLayer
{
    public class EfUserService : IUserService
    {
        IUnitOfWork _uow;
        readonly IDbSet<User> _users;
        private readonly Lazy<IRoleService> _roleService;
        private readonly Lazy<IProfessorService> _professorService;

        public EfUserService(IUnitOfWork uow, Lazy<IRoleService> roleService, Lazy<IProfessorService> professorService)
        {
            _uow = uow;
            _users = _uow.Set<User>();
            _roleService = roleService;
            _professorService = professorService;
        }

        public bool IsUserInRole(string email, string roleName)
        {
            return _users.Any(u => u.Email == email && u.Roles.Any(r => r.Name == roleName));
        }

        public string[] GetRolesForUser(string email)
        {
            var user = _users.Single(m => m.Email == email);

            var roles = user.
                        Roles.
                        Select(r => r.Name).ToList();

            return roles.ToArray();
        }

        public string GetPassword(string email)
        {
            return _users.Where(u => u.Email == email).Select(u => u.Password).SingleOrDefault();
        }

        public string GetPasswordById(int userId)
        {
            return _users.Where(u => u.Id == userId).Select(u => u.Password).SingleOrDefault();
        }

        public LogonViewModel GetInfoForLogon(string email)
        {
            var userInfo = _users.Where(u => u.Email == email).Select(u => new { u.Id, u.Password, u.Roles }).SingleOrDefault();

            return new LogonViewModel
            {
                UserId = userInfo.Id,
                Password = userInfo.Password,
                Roles = userInfo.Roles.Select(r => r.Name).ToArray()
            };
        }

        public bool IsExistUser(string email)
        {
            return _users.Any(u => u.Email == email);
        }

        public void UpdatePasswordByEmail(string email, string newPassword)
        {
            var user = _users.Single(u => u.Email == email);
            user.Password = newPassword;
        }

        public User UpdatePasswordById(int userId, string newPassword)
        {
            var user = _users.Single(u => u.Id == userId);
            user.Password = newPassword;
            return user;
        }

        public void UpdateNote(int userId, string note)
        {
            var user = _users.Single(u => u.Id == userId);
            user.Note = note;
        }

        public int GetUserId(string email)
        {
            return _users.Where(u => u.Email == email).Select(u => u.Id).Single();
        }

        public string GetNote(int userId)
        {
            return _users.Where(u => u.Id == userId).Select(u => u.Note).Cacheable().Single();
        }

        public UserTopMenuViewModel GetTopMenuInfo(int userId)
        {
            var infos = _users.Where(u => u.Id == userId).Select(u => new { u.FirstName, u.LastName, u.ProfessorProfile.PageId }).Cacheable().SingleOrDefault();

            return new UserTopMenuViewModel
            {
                Firstname = infos.FirstName,
                Lastname = infos.LastName,
                PageId = infos.PageId
            };
        }

        public bool IsUniqueEmail(int userId, string newEmail)
        {
            return !_users.Any(u => u.Id != userId && u.Email == newEmail);
        }

        public void UpdateDetails(int userId, DetailsViewModel info)
        {
            var user = _users.Single(u => u.Id == userId);

            user.FirstName = info.FirstName;
            user.LastName = info.LastName;
            user.Email = info.Email.Trim().ToLowerInvariant();
        }

        public void UpdateLastLogonTimeAndIp(string email, string ip)
        {
            //var stub = new User { Email = email };
            //_uow.Entry(stub).State = EntityState.Modified;
            //stub.LastLoginDate = DateTime.UtcNow;
            //stub.LastIp = ip;
            var user = _users.Single(u => u.Email == email);
            user.LastLoginDate = DateTime.UtcNow;
            user.LastIp = ip;
        }

        public DetailsViewModel GetAdminDashboardDetails(int userId)
        {
            var userDetails = _users.Single(u => u.Id == userId);

            return new DetailsViewModel
            {
                FirstName = userDetails.FirstName,
                LastName = userDetails.LastName,
                Email = userDetails.Email
            };

        }

        public IEnumerable<string> GetRolesForUser(int userId)
        {
            var user = _users.Where(u => u.Id == userId).IncludeOptimized(u => u.Roles).Select(u => new { u.Roles }).Single();

            //var roles = user.
            //            Roles.
            //            Select(r => r.Name).ToList();

            return user.Roles.Select(r => r.Name).ToList();
        }

        public IEnumerable<UserListViewModel> GetListUsers(string filterFirstname, string filterLastname, string filterEmail, string filterPageId, string filterRole = "notdefined", bool filterIsBanned = false, bool filterIsSoftDelete = false, int startIndex = 0, int pageSize = 10)
        {
            var queryableUsers = _users.
                Include(u => u.ProfessorProfile).
                Include(u => u.Roles).AsQueryable();

            if (!string.IsNullOrEmpty(filterFirstname))
            {
                queryableUsers = queryableUsers.Where(u => u.FirstName.Contains(filterFirstname));
            }

            if (!string.IsNullOrEmpty(filterLastname))
            {
                queryableUsers = queryableUsers.Where(u => u.LastName.Contains(filterLastname));
            }

            if (!string.IsNullOrEmpty(filterEmail))
            {
                queryableUsers = queryableUsers.Where(u => u.Email.Contains(filterEmail) || u.ProfessorProfile.SecondaryEmails.Contains(filterEmail));
            }

            if (!string.IsNullOrEmpty(filterPageId))
            {
                queryableUsers = queryableUsers.Where(u => u.ProfessorProfile.PageId.Contains(filterPageId));
            }

            if (!string.IsNullOrEmpty(filterRole) && filterRole != "notdefined")
            {
                queryableUsers = queryableUsers.IncludeOptimized(u => u.Roles).Where(u => u.Roles.Where(r => r.Name == filterRole).Any());
            }

            if (filterIsBanned == true)
            {
                queryableUsers = queryableUsers.IncludeOptimized(u => u.ProfessorProfile).Where(u => u.ProfessorProfile.IsBanned == filterIsBanned);
            }

            if (filterIsSoftDelete == true)
            {
                queryableUsers = queryableUsers.IncludeOptimized(u => u.ProfessorProfile).Where(u => u.ProfessorProfile.IsSoftDelete == filterIsSoftDelete);
            }

            var users = queryableUsers.
                Select(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.ProfessorProfile.PageId, IsSoftDelete = u.ProfessorProfile.IsSoftDelete ? u.ProfessorProfile.IsSoftDelete : false, IsBanned = u.ProfessorProfile.IsBanned ? u.ProfessorProfile.IsBanned : false, u.ProfessorProfile.BannedDate, u.ProfessorProfile.BannedReason, u.Roles }).
                OrderByDescending(u => u.Id).
                Skip(startIndex).
                Take(pageSize).
                ToList();

            var userSection = users.Select(u => new { u.Id, u.FirstName, u.LastName, u.Email }).Distinct();
            var professorSection = users.Select(u => new { u.Id, u.BannedDate, u.BannedReason, u.IsBanned, u.IsSoftDelete, u.PageId }).Distinct();

            var usersList = new List<UserListViewModel>();
            foreach (var user in userSection)
            {
                var roles = users.Where(r => r.Id == user.Id).Select(r => r.Roles).First();
                var tempDictionary = new Dictionary<string, string>();

                foreach (var item in roles)
                {
                    tempDictionary.Add(item.Id.ToString(), item.Name);
                }

                usersList.Add(new UserListViewModel
                {
                    UserId = user.Id,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    Email = user.Email,
                    PageId = professorSection.Where(p => p.Id == user.Id).Select(p => p.PageId).Single(),
                    IsBanned = professorSection.Where(p => p.Id == user.Id).Select(p => p.IsBanned).Single(),
                    IsSoftDelete = professorSection.Where(p => p.Id == user.Id).Select(p => p.IsSoftDelete).Single(),
                    BannedDate = professorSection.Where(p => p.Id == user.Id).Select(p => p.BannedDate).Single(),
                    BannedReason = professorSection.Where(p => p.Id == user.Id).Select(p => p.BannedReason).Single(),
                    Roles = tempDictionary
                });
            }

            return usersList;
        }

        public User CreateUser(UserListViewModel user, string password, string[] userRoles)
        {
            var newUser = new User();

            newUser.FirstName = user.Firstname;
            newUser.LastName = user.Lastname;
            newUser.Email = user.Email.Trim().ToLowerInvariant();
            newUser.Password = password;

            for (int i = 0; i < userRoles.Length; i++)
            {
                newUser.Roles.Add(_roleService.Value.AttachRole(new Role { Id = Convert.ToInt32(userRoles[i].Split(':')[0].Trim()), Name = userRoles[i].Split(':')[1].Trim().ToLowerInvariant() }));
            }

            _users.Add(newUser);
            return newUser;
        }

        public void UpdateUser(UserListViewModel updatedUser, string[] userRoles, bool isProfessor, bool isRolesChanged)
        {
            var queryableUser = _users.AsQueryable();

            if (isProfessor)
            {
                queryableUser = queryableUser.IncludeOptimized(u => u.ProfessorProfile);
            }

            if (isRolesChanged)
            {
                queryableUser = queryableUser.IncludeOptimized(u => u.Roles);
            }

            var user = queryableUser.Single(u => u.Id == updatedUser.UserId);

            user.FirstName = updatedUser.Firstname;
            user.LastName = updatedUser.Lastname;
            user.Email = updatedUser.Email.Trim().ToLowerInvariant();

            if (isProfessor)
            {
                user.ProfessorProfile.PageId = updatedUser.PageId.Trim().ToLowerInvariant();
                user.ProfessorProfile.IsBanned = updatedUser.IsBanned;
                user.ProfessorProfile.IsSoftDelete = updatedUser.IsSoftDelete;
                user.ProfessorProfile.BannedDate = null;
                user.ProfessorProfile.BannedReason = null;

                if(updatedUser.IsSoftDelete)
                {
                    user.ProfessorProfile.SoftDeleteDate = DateTime.UtcNow;
                }
                else
                {
                    user.ProfessorProfile.SoftDeleteDate = null;
                }

                if (updatedUser.IsBanned)
                {
                    user.ProfessorProfile.BannedDate = DateTime.UtcNow;
                    user.ProfessorProfile.BannedReason = updatedUser.BannedReason;
                }
                else
                {
                    user.ProfessorProfile.BannedDate = null;
                    user.ProfessorProfile.BannedReason = null;
                }
            }

            if (isRolesChanged)
            {
                _uow.Entry(user).Collection(u => u.Roles).Load();
                var newRolesForUser = _roleService.Value.GetRoles(userRoles.Select(r => r.Split(':')[1].Trim().ToLowerInvariant()).ToArray());
                user.Roles = (ICollection<Role>)newRolesForUser;
            }
            //if (isRolesChanged)
            //{
            //    user.Roles.Clear();
            //    for (int i = 0; i < userRoles.Length; i++)
            //    {
            //        user.Roles.Add(_roleService.Value.AttachRole(new Role { Id = Convert.ToInt32(userRoles[i].Split(':')[0].Trim()), Name = userRoles[i].Split(':')[1].Trim().ToLowerInvariant() }));
            //    }
            //}
        }

        public void DeleteUser(int userId)
        {
            var user = new User { Id = userId };
            _uow.Entry(user).State = EntityState.Deleted;
        }

        public int TotalUsersCount(string filterFirstname, string filterLastname, string filterEmail, string filterPageId, string filterRole = "notdefined", bool filterIsBanned = false, bool filterIsSoftDelete = false)
        {
            var usersCount = _users.AsQueryable();

            if (!string.IsNullOrEmpty(filterFirstname))
            {
                usersCount = usersCount.Where(u => u.FirstName.Contains(filterFirstname));
            }

            if (!string.IsNullOrEmpty(filterLastname))
            {
                usersCount = usersCount.Where(u => u.LastName.Contains(filterLastname));
            }

            if (!string.IsNullOrEmpty(filterEmail))
            {
                usersCount = usersCount.Where(u => u.Email.Contains(filterEmail));
            }

            if (!string.IsNullOrEmpty(filterPageId))
            {
                usersCount = usersCount.Where(u => u.ProfessorProfile.PageId.Contains(filterPageId));
            }

            if (!string.IsNullOrEmpty(filterRole) && filterRole != "notdefined")
            {
                usersCount = usersCount.IncludeOptimized(u => u.Roles).Where(u => u.Roles.Where(r => r.Name == filterRole).Any());
            }

            if (filterIsBanned == true)
            {
                usersCount = usersCount.IncludeOptimized(u => u.ProfessorProfile).Where(u => u.ProfessorProfile.IsBanned == filterIsBanned);
            }

            if (filterIsSoftDelete == true)
            {
                usersCount = usersCount.IncludeOptimized(u => u.ProfessorProfile).Where(u => u.ProfessorProfile.IsSoftDelete == filterIsSoftDelete);
            }

            return usersCount.Select(u => u.Id).Cacheable().Count();
        }

        public bool IsUserInRole(int userId, string roleName)
        {
            return _users.Any(u => u.Id == userId && u.Roles.Any(r => r.Name == roleName));
        }

        public IEnumerable<string> GetRolesForUserAndDetach(int userId)
        {
            var user = _users.Where(u => u.Id == userId).IncludeOptimized(u => u.Roles).Select(u => new { u.Roles }).Single();
            _roleService.Value.DetachRoles(user.Roles);
            return user.Roles.Select(r => r.Name).ToList();
        }

        public bool IsFirstLogin(int userId)
        {
            return _users.Any(u => u.Id == userId && u.LastLoginDate == null);
        }
    }
}

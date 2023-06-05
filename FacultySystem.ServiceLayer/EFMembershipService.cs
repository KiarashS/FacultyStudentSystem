using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.DataLayer.Context;
using System.Data.Entity;
using ContentManagementSystem.DomainClasses;
using EFSecondLevelCache;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFMembershipService : IMembershipService
    {
        IUnitOfWork _uow;
        readonly IDbSet<ProfessorMembership> _memberships;
        public EFMembershipService(IUnitOfWork uow)
        {
            _uow = uow;
            _memberships = _uow.Set<ProfessorMembership>();
        }

        public IEnumerable<ProfessorMembershipViewModel> GetMemberships(int userId)
        {
            var membershipList = new List<ProfessorMembershipViewModel>();
            var memberships = _memberships
                .Where(m => m.ProfessorId == userId)
                .OrderByDescending(m => m.Order)
                .ThenBy(m => m.Id)
                .Cacheable()
                .ToList();

            foreach (var membership in memberships)
            {
                membershipList.Add(new ProfessorMembershipViewModel
                {
                    Id = membership.Id,
                    Post = membership.Post,
                    CommitteeTitle = membership.CommitteeTitle,
                    StartTime = membership.StartTime,
                    EndTime = membership.EndTime,
                    Description = membership.Description,
                    Link = membership.Link,
                    Order = membership.Order
                });
            }

            return membershipList;
        }

        public ProfessorMembership CreateMembership(int userId, ProfessorMembershipViewModel administration)
        {
            var newMembership = _memberships.Add(new ProfessorMembership
            {
                ProfessorId = userId,
                Post = administration.Post,
                CommitteeTitle = administration.CommitteeTitle,
                StartTime = administration.StartTime,
                EndTime = administration.EndTime,
                Description = administration.Description,
                Link = administration.Link,
                Order = administration.Order
            });

            return newMembership;
        }

        public void UpdateMembership(int userId, ProfessorMembershipViewModel newMembership)
        {
            var membership = _memberships.Single(m => m.ProfessorId == userId && m.Id == newMembership.Id);

            membership.Post = newMembership.Post;
            membership.CommitteeTitle = newMembership.CommitteeTitle;
            membership.StartTime = newMembership.StartTime;
            membership.EndTime = newMembership.EndTime;
            membership.Description = newMembership.Description;
            membership.Link = newMembership.Link;
            membership.Order = newMembership.Order;
        }

        public void DeleteMembership(int userId, long id)
        {
            var membership = _memberships.Single(m => m.ProfessorId == userId && m.Id == id);
            _memberships.Remove(membership);
        }
    }
}

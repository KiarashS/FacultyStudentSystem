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
    public class EFAdministrationService : IAdministrationRecordService
    {
        IUnitOfWork _uow;
        readonly IDbSet<AdministrationRecord> _administrations;
        public EFAdministrationService(IUnitOfWork uow)
        {
            _uow = uow;
            _administrations = _uow.Set<AdministrationRecord>();
        }

        public IEnumerable<AdministrationViewModel> GetAdministrations(int userId)
        {
            var administrationList = new List<AdministrationViewModel>();
            var administrations = _administrations
                .Where(a => a.ProfessorId == userId)
                .OrderByDescending(a => a.Order)
                .ThenBy(a => a.Id)
                .Cacheable()
                .ToList();

            foreach (var administration in administrations)
            {
                administrationList.Add(new AdministrationViewModel
                {
                    Id = administration.Id,
                    Post = administration.Post,
                    Place = administration.Place,
                    StartTime = administration.StartTime,
                    EndTime = administration.EndTime,
                    Description = administration.Description,
                    Link = administration.Link,
                    Order = administration.Order
                });
            }

            return administrationList;
        }

        public AdministrationRecord CreateAdministration(int userId, AdministrationViewModel administration)
        {
            var newAdministration = _administrations.Add(new AdministrationRecord
            {
                ProfessorId = userId,
                Post = administration.Post,
                Place = administration.Place,
                StartTime = administration.StartTime,
                EndTime = administration.EndTime,
                Description = administration.Description,
                Link = administration.Link,
                Order = administration.Order
            });

            return newAdministration;
        }

        public void UpdateAdministration(int userId, AdministrationViewModel newAdministration)
        {
            var administration = _administrations.Single(a => a.ProfessorId == userId && a.Id == newAdministration.Id);

            administration.Post = newAdministration.Post;
            administration.Place = newAdministration.Place;
            administration.StartTime = newAdministration.StartTime;
            administration.EndTime = newAdministration.EndTime;
            administration.Description = newAdministration.Description;
            administration.Link = newAdministration.Link;
            administration.Order = newAdministration.Order;
        }

        public void DeleteAdministration(int userId, long id)
        {
            var administration = _administrations.Single(a => a.ProfessorId == userId && a.Id == id);
            _administrations.Remove(administration);
        }
    }
}

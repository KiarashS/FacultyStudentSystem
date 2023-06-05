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
    public class EFWorkshopService : IWorkshopService
    {
        IUnitOfWork _uow;
        readonly IDbSet<CourseAndWorkshop> _workshops;
        public EFWorkshopService(IUnitOfWork uow)
        {
            _uow = uow;
            _workshops = _uow.Set<CourseAndWorkshop>();
        }

        public IEnumerable<WorkshopViewModel> GetWorkshops(int userId)
        {
            var workshopList = new List<WorkshopViewModel>();
            var workshops = _workshops
                .Where(w => w.ProfessorId == userId)
                .OrderByDescending(w => w.Order)
                .ThenBy(w => w.Id)
                .Cacheable()
                .ToList();

            foreach (var workshop in workshops)
            {
                workshopList.Add(new WorkshopViewModel
                {
                    Id = workshop.Id,
                    Title = workshop.Title,
                    Host = workshop.Host,
                    Place = workshop.Place,
                    StartTime = workshop.StartTime,
                    EndTime = workshop.EndTime,
                    Description = workshop.Description,
                    Link = workshop.Link,
                    Order = workshop.Order
                });
            }

            return workshopList;
        }

        public CourseAndWorkshop CreateWorkshop(int userId, WorkshopViewModel workshop)
        {
            var newWorkshop = _workshops.Add(new CourseAndWorkshop
            {
                ProfessorId = userId,
                Title = workshop.Title,
                Host = workshop.Host,
                Place = workshop.Place,
                StartTime = workshop.StartTime,
                EndTime = workshop.EndTime,
                Description = workshop.Description,
                Link = workshop.Link,
                Order = workshop.Order
            });

            return newWorkshop;
        }

        public void UpdateWorkshop(int userId, WorkshopViewModel newWorkshop)
        {
            var workshop = _workshops.Single(w => w.ProfessorId == userId && w.Id == newWorkshop.Id);

            workshop.Title = newWorkshop.Title;
            workshop.Host = newWorkshop.Host;
            workshop.Place = newWorkshop.Place;
            workshop.StartTime = newWorkshop.StartTime;
            workshop.EndTime = newWorkshop.EndTime;
            workshop.Description = newWorkshop.Description;
            workshop.Link = newWorkshop.Link;
            workshop.Order = newWorkshop.Order;
        }

        public void DeleteWorkshop(int userId, long id)
        {
            var workshop = _workshops.Single(w => w.ProfessorId == userId && w.Id == id);
            _workshops.Remove(workshop);
        }
    }
}

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
    public class EFHonorService : IHonorService
    {
        IUnitOfWork _uow;
        readonly IDbSet<Honor> _honors;
        public EFHonorService(IUnitOfWork uow)
        {
            _uow = uow;
            _honors = _uow.Set<Honor>();
        }

        public IEnumerable<HonorViewModel> GetHonors(int userId)
        {
            var honorList = new List<HonorViewModel>();
            var honors = _honors
                .Where(h => h.ProfessorId == userId)
                .OrderByDescending(h => h.Order)
                .ThenBy(h => h.Id)
                .Cacheable()
                .ToList();

            foreach (var honor in honors)
            {
                honorList.Add(new HonorViewModel
                {
                    Id = honor.Id,
                    Title = honor.Title,
                    Time = honor.Time,
                    Link = honor.Link,
                    Order = honor.Order
                });
            }

            return honorList;
        }

        public Honor CreateHonor(int userId, HonorViewModel honor)
        {
            var newHonor = _honors.Add(new Honor
            {
                ProfessorId = userId,
                Title = honor.Title,
                Time = honor.Time,
                Link = honor.Link,
                Order = honor.Order
            });

            return newHonor;
        }

        public void UpdateHonor(int userId, HonorViewModel newHonor)
        {
            var honor = _honors.Single(h => h.ProfessorId == userId && h.Id == newHonor.Id);

            honor.Title = newHonor.Title;
            honor.Time = newHonor.Time;
            honor.Link = newHonor.Link;
            honor.Order = newHonor.Order;
        }

        public void DeleteHonor(int userId, long id)
        {
            var honor = _honors.Single(h => h.ProfessorId == userId && h.Id == id);
            _honors.Remove(honor);
        }
    }
}

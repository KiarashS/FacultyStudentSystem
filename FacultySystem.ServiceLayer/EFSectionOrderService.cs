using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.DomainClasses;
using System.Data.Entity;
using EFSecondLevelCache;
using Z.EntityFramework.Plus;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFSectionOrderService : ISectionOrderService
    {
        IUnitOfWork _uow;
        readonly IDbSet<SectionOrder> _sections;
        //private readonly Lazy<IUserService> _userService;

        public EFSectionOrderService(IUnitOfWork uow)
        {
            _uow = uow;
            _sections = _uow.Set<SectionOrder>();
        }

        public IEnumerable<SectionsOrderViewModel> GetSectionOrders(int userId)
        {
            var list = _sections.Where(so => so.ProfessorId == userId).OrderBy(so => so.Order).ThenBy(so => so.Id).Cacheable().ToList();
            var sections = new List<SectionsOrderViewModel>();

            foreach (var section in list)
            {
                sections.Add(new SectionsOrderViewModel
                {
                    Id = section.Id,
                    Order = section.Order,
                    SectionName = section.SectionName
                });
            }

            return sections;
        }

        public void DeleteSections(int userId)
        {
            var affectedRecordsNumber = _sections.Where(so => so.ProfessorId == userId).Delete();
        }

        public void CreateSections(int userId)
        {
            foreach (SectionName item in Enum.GetValues(typeof(SectionName)))
            {
                _sections.Add(new SectionOrder {
                    ProfessorId = userId,
                    SectionName = item,
                    Order = DomainEnumExtensions.GetDefaultOrder(item)
                });
            }
        }

        public void UpdateSections(int userId, Dictionary<SectionName, int> orders)
        {
            if (orders.Count == 0)
            {
                return;
            }

            var sectionOrders = _sections.Where(so => so.ProfessorId == userId && orders.Keys.Contains(so.SectionName)).ToList();

            sectionOrders.ForEach(so => {
                so.Order = orders[so.SectionName];
            });
        }
    }
}

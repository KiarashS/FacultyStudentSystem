using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using EFSecondLevelCache;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFEducationalGroupService: IEducationalGroupService
    {
        IUnitOfWork _uow;
        readonly IDbSet<EducationalGroup> _educationalGroups;
        private readonly Lazy<IProfessorService> _professorService;
        public EFEducationalGroupService(IUnitOfWork uow, Lazy<IProfessorService> professorService)
        {
            _uow = uow;
            _educationalGroups = _uow.Set<EducationalGroup>();
            _professorService = professorService;
        }

        public IEnumerable<EducationalGroupViewModel> GetListEducationalGroups()
        {
            var list = _educationalGroups
                .Where(eg => eg.Name != "--")
                .Select(eg => new { eg.Id, eg.Name, eg.Order, count = eg.Professors.Count() })
                .OrderByDescending(eg => eg.Order)
                .ThenBy(eg => eg.Id)
                .Cacheable()
                .ToList();
            var groups = new List<EducationalGroupViewModel>();

            foreach (var item in list)
            {
                groups.Add(new EducationalGroupViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order = item.Order,
                    ProfessorCount = item.count
                });
            }

            return groups;
        }

        public IEnumerable<EducationalGroupViewModel> GetEducationalGroups()
        {
            var list = _educationalGroups
                .Where(eg => eg.Name == "--")
                .Union(_educationalGroups
                .Where(eg => eg.Name != "--"))
                .OrderByDescending(eg => eg.Order)
                .ThenBy(eg => eg.Id)
                .Cacheable()
                .ToList();
            var groups = new List<EducationalGroupViewModel>();

            foreach (var item in list)
            {
                groups.Add(new EducationalGroupViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order = item.Order
                });
            }

            return groups;
        }

        public EducationalGroup CreateEducationalGroup(EducationalGroupViewModel educationalGroup)
        {
            var group = new EducationalGroup
            {
                Name = educationalGroup.Name,
                Order = educationalGroup.Order
            };

            _educationalGroups.Add(group);

            return group;
        }

        public void UpdateEducationalGroup(EducationalGroupViewModel newEducationalGroup)
        {
            var group = _educationalGroups.Single(eg => eg.Id == newEducationalGroup.Id);

            group.Name = newEducationalGroup.Name;
            group.Order = newEducationalGroup.Order;
        }

        public bool DeleteEducationalGroup(int id)
        {
            var group = _educationalGroups.Single(eg => eg.Id == id);

            if (group.Professors.Any())
            {
                var defaultGroupId = _educationalGroups.Where(eg => eg.Name == "--").Select(ed => ed.Id).Single();
                _professorService.Value.UpdateEducationalGroupToDefault(group.Id, defaultGroupId);
            }

            _educationalGroups.Remove(group);
            return true;
        }

        public bool ExistName(int id, string name)
        {
            return _educationalGroups.Any(eg => eg.Id != id && eg.Name == name.Trim());
        }

        public int GetIdByName(string name)
        {
            return _educationalGroups.Where(eg => eg.Name == name.Trim()).Select(eg => eg.Id).SingleOrDefault();
        }

        public int NumberOfEducationalGroups()
        {
            return _educationalGroups.Where(g => g.Name != "--").Cacheable().Count();
        }
    }
}

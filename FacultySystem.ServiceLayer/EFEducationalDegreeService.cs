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
    public class EFEducationalDegreeService: IEducationalDegreeService
    {
        IUnitOfWork _uow;
        readonly IDbSet<EducationalDegree> _educationalDegrees;
        private readonly Lazy<IProfessorService> _professorService;
        public EFEducationalDegreeService(IUnitOfWork uow, Lazy<IProfessorService> professorService)
        {
            _uow = uow;
            _educationalDegrees = _uow.Set<EducationalDegree>();
            _professorService = professorService;
        }

        public IEnumerable<EducationalDegreeViewModel> GetListEducationalDegrees()
        {
            var list = _educationalDegrees
                .Where(ed => ed.Name != "--")
                .Select(ed => new { ed.Id, ed.Name, ed.Order, count = ed.Professors.Count() })
                .OrderByDescending(ed => ed.Order)
                .ThenBy(ed => ed.Id)
                .Cacheable()
                .ToList();
            var degrees = new List<EducationalDegreeViewModel>();

            foreach (var item in list)
            {
                degrees.Add(new EducationalDegreeViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order = item.Order,
                    ProfessorCount = item.count
                });
            }

            return degrees;
        }

        public IEnumerable<EducationalDegreeViewModel> GetEducationalDegrees()
        {
            var list = _educationalDegrees
                .Where(ed => ed.Name == "--")
                .Union(_educationalDegrees
                .Where(ed => ed.Name != "--"))
                .OrderByDescending(ed => ed.Order)
                .ThenBy(ed => ed.Id)
                .Cacheable()
                .ToList();
            var degrees = new List<EducationalDegreeViewModel>();

            foreach (var item in list)
            {
                degrees.Add(new EducationalDegreeViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order = item.Order
                });
            }

            return degrees;
        }

        public EducationalDegree CreateEducationalDegree(EducationalDegreeViewModel educationalDegree)
        {
            var degree = new EducationalDegree
            {
                Name = educationalDegree.Name,
                Order = educationalDegree.Order
            };

            _educationalDegrees.Add(degree);

            return degree;
        }

        public void UpdateEducationalDegree(EducationalDegreeViewModel newEducationalDegree)
        {
            var degree = _educationalDegrees.Single(ed => ed.Id == newEducationalDegree.Id);

            degree.Name = newEducationalDegree.Name;
            degree.Order = newEducationalDegree.Order;
        }

        public bool DeleteEducationalDegree(int id)
        {
            var degree = _educationalDegrees.Single(ed => ed.Id == id);

            if (degree.Professors.Any())
            {
                var defaultDegreeId = _educationalDegrees.Where(ed => ed.Name == "--").Select(ed => ed.Id).Single();
                _professorService.Value.UpdateEducationalDegreeToDefault(degree.Id, defaultDegreeId);
            }

            _educationalDegrees.Remove(degree);
            return true;
        }

        public bool ExistName(int id, string name)
        {
            return _educationalDegrees.Any(ed => ed.Id != id && ed.Name == name.Trim());
        }

        public int GetIdByName(string name)
        {
            return _educationalDegrees.Where(ed => ed.Name == name.Trim()).Select(ed => ed.Id).SingleOrDefault();
        }
    }
}

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
    public class EFCollegeService: ICollegeService
    {
        IUnitOfWork _uow;
        readonly IDbSet<College> _colleges;
        private readonly Lazy<IProfessorService> _professorService;
        public EFCollegeService(IUnitOfWork uow, Lazy<IProfessorService> professorService)
        {
            _uow = uow;
            _colleges = _uow.Set<College>();
            _professorService = professorService;
        }

        public IEnumerable<CollegeViewModel> GetListColleges()
        {
            var list = _colleges
                .Where(c => c.Name != "--")
                .Select(c => new { c.Id, c.Name, c.Order, count = c.Professors.Count() })
                .OrderByDescending(c => c.Order)
                .ThenBy(c => c.Id)
                .Cacheable()
                .ToList();
            var colleges = new List<CollegeViewModel>();

            foreach (var item in list)
            {
                colleges.Add(new CollegeViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order = item.Order,
                    ProfessorCount = item.count
                });
            }

            return colleges;
        }

        public IEnumerable<CollegeViewModel> GetColleges()
        {
            var list = _colleges
                .Where(c => c.Name == "--")
                .Union(_colleges
                .Where(c => c.Name != "--"))
                .OrderByDescending(c => c.Order)
                .ThenBy(c => c.Id)
                .Cacheable()
                .ToList();
            var colleges = new List<CollegeViewModel>();

            foreach (var item in list)
            {
                colleges.Add(new CollegeViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Order = item.Order
                });
            }

            return colleges;
        }

        public College CreateCollege(CollegeViewModel college)
        {
            var newCollege = new College
            {
                Name = college.Name,
                Order = college.Order
            };

            _colleges.Add(newCollege);

            return newCollege;
        }

        public void UpdateCollege(CollegeViewModel newCollege)
        {
            var college = _colleges.Single(c => c.Id == newCollege.Id);

            college.Name = newCollege.Name;
            college.Order = newCollege.Order;
        }

        public bool DeleteCollege(int id)
        {
            var college = _colleges.Single(c => c.Id == id);

            if (college.Professors.Any())
            {
                var defaultCollegeId = _colleges.Where(eg => eg.Name == "--").Select(ed => ed.Id).Single();
                _professorService.Value.UpdateCollegeToDefault(college.Id, defaultCollegeId);
            }

            _colleges.Remove(college);
            return true;
        }

        public bool ExistName(int id, string name)
        {
            return _colleges.Any(c => c.Id != id && c.Name == name.Trim());
        }

        public int GetIdByName(string name)
        {
            return _colleges.Where(c => c.Name == name.Trim()).Select(c => c.Id).SingleOrDefault();
        }

        public int NumberOfColleges()
        {
            return _colleges.Where(c => c.Name != "--").Cacheable().Count();
        }
    }
}

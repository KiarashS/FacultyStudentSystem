using ContentManagementSystem.ServiceLayer.Contracts;
using System.Collections.Generic;
using System.Linq;
using ContentManagementSystem.DataLayer.Context;
using System.Data.Entity;
using ContentManagementSystem.DomainClasses;
using EFSecondLevelCache;
using ContentManagementSystem.Models.ViewModels;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFStudingService : IStudingRecordService
    {
        IUnitOfWork _uow;
        readonly IDbSet<StudingRecord> _studings;
        public EFStudingService(IUnitOfWork uow)
        {
            _uow = uow;
            _studings = _uow.Set<StudingRecord>();
        }

        public IEnumerable<StudingViewModel> GetStudings(int userId)
        {
            var studingList = new List<StudingViewModel>();
            var studings = _studings
                .Where(s => s.ProfessorId == userId)
                .OrderByDescending(s => s.Order)
                .ThenBy(s => s.Id)
                .Cacheable()
                .ToList();

            foreach (var studing in studings)
            {
                studingList.Add(new StudingViewModel
                {
                    Id = studing.Id,
                    Grade = studing.Grade,
                    Field = studing.Field,
                    Trend = studing.Trend,
                    University = studing.University,
                    ThesisTitle = studing.ThesisTitle,
                    ThesisSupervisors = studing.ThesisSupervisors,
                    ThesisAdvisors = studing.ThesisAdvisors,
                    StartTime = studing.StartTime,
                    EndTime = studing.EndTime,
                    Link = studing.Link,
                    Order = studing.Order
                });
            }

            return studingList;
        }

        public StudingRecord CreateStuding(int userId, StudingViewModel studing)
        {
            var newStuding = _studings.Add(new StudingRecord
            {
                ProfessorId = userId,
                Grade = studing.Grade,
                Field = studing.Field,
                Trend = studing.Trend,
                University = studing.University,
                ThesisTitle = studing.ThesisTitle,
                ThesisSupervisors = studing.ThesisSupervisors,
                ThesisAdvisors = studing.ThesisAdvisors,
                StartTime = studing.StartTime,
                EndTime = studing.EndTime,
                Link = studing.Link,
                Order = studing.Order
            });

            return newStuding;
        }

        public void UpdateStuding(int userId, StudingViewModel newStuding)
        {
            var studing = _studings.Single(s => s.ProfessorId == userId && s.Id == newStuding.Id);

            studing.Grade = newStuding.Grade;
            studing.Field = newStuding.Field;
            studing.Trend = newStuding.Trend;
            studing.University = newStuding.University;
            studing.ThesisTitle = newStuding.ThesisTitle;
            studing.ThesisSupervisors = newStuding.ThesisSupervisors;
            studing.ThesisAdvisors = newStuding.ThesisAdvisors;
            studing.StartTime = newStuding.StartTime;
            studing.EndTime = newStuding.EndTime;
            studing.Link = newStuding.Link;
            studing.Order = newStuding.Order;
        }

        public void DeleteStuding(int userId, long id)
        {
            var studing = _studings.Single(s => s.ProfessorId == userId && s.Id == id);
            _studings.Remove(studing);
        }
    }
}

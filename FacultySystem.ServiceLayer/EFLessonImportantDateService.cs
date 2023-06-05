using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentManagementSystem.Models.ViewModels;
using EFSecondLevelCache;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFLessonImportantDateService : ILessonImportantDateService
    {
        IUnitOfWork _uow;
        readonly IDbSet<LessonImportantDate> _dates;
        //private readonly Lazy<Professor> _professorService;
        public EFLessonImportantDateService(IUnitOfWork uow)
        {
            _uow = uow;
            _dates = _uow.Set<LessonImportantDate>();
        }

        public IEnumerable<LessonImportantDateViewModel> GetImportantDates(int userId, long lessonId)
        {
            var dateList = new List<LessonImportantDateViewModel>();
            var dates = _dates
                .Where(l => l.ProfessorId == userId && l.LessonId == lessonId)
                .OrderByDescending(l => l.Order)
                .ThenBy(l => l.Id)
                .Cacheable()
                .ToList();

            foreach (var date in dates)
            {
                dateList.Add(new LessonImportantDateViewModel
                {
                    Id = date.Id,
                    Date = date.Date,
                    Time = date.Time,
                    DateDay = date.DateDay,
                    Description = date.Description,
                    CreateDate = date.CreateDate,
                    Link = date.Link,
                    Order = date.Order
                });
            }

            return dateList;
        }

        public LessonImportantDate CreateImportantDate(int userId, LessonImportantDateViewModel date)
        {
            var newDate = _dates.Add(new LessonImportantDate
            {
                ProfessorId = userId,
                LessonId = date.LessonId,
                Date = date.Date,
                Time = date.Time,
                DateDay = date.DateDay,
                Description = date.Description,
                Link = date.Link,
                Order = date.Order
            });

            return newDate;
        }

        public void UpdateImportantDate(int userId, LessonImportantDateViewModel newDate)
        {
            var date =_dates.Single(l => l.ProfessorId == userId && l.Id == newDate.Id);

            date.Date = newDate.Date;
            date.Time = newDate.Time;
            date.DateDay = newDate.DateDay;
            date.Description = newDate.Description;
            date.Link = newDate.Link;
            date.Order = newDate.Order;
        }

        public void DeleteImportantDate(int userId, long id)
        {
            var date = _dates.Single(l => l.ProfessorId == userId && l.Id == id);
            _dates.Remove(date);
        }
    }
}

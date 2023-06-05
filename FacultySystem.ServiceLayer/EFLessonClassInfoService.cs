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
    public class EFLessonClassInfoService: ILessonClassInfoService
    {
        IUnitOfWork _uow;
        readonly IDbSet<LessonClassInfo> _lessonClasses;
        //private readonly Lazy<Professor> _professorService;
        public EFLessonClassInfoService(IUnitOfWork uow)
        {
            _uow = uow;
            _lessonClasses = _uow.Set<LessonClassInfo>();
        }

        public IEnumerable<LessonClassInfoViewModel> GetLessonClasses(int userId, long lessonId)
        {
            var classList = new List<LessonClassInfoViewModel>();
            var classes = _lessonClasses
                .Where(l => l.ProfessorId == userId && l.LessonId == lessonId)
                .OrderByDescending(l => l.Order)
                .ThenBy(l => l.Id)
                .Cacheable()
                .ToList();

            foreach (var @class in classes)
            {
                classList.Add(new LessonClassInfoViewModel
                {
                    Id = @class.Id,
                    ClassDay = @class.ClassDay,
                    CreateDate = @class.CreateDate,
                    Description = @class.Description,
                    EndHour = @class.EndHour,
                    StartHour = @class.StartHour,
                    Place = @class.Place,
                    Link = @class.Link,
                    Order = @class.Order
                });
            }

            return classList;
        }

        public LessonClassInfo CreateLessonClass(int userId, LessonClassInfoViewModel lessonClass)
        {
            var newClass = _lessonClasses.Add(new LessonClassInfo
            {
                ProfessorId = userId,
                LessonId = lessonClass.LessonId,
                ClassDay = lessonClass.ClassDay,
                Description = lessonClass.Description,
                EndHour = lessonClass.EndHour,
                StartHour = lessonClass.StartHour,
                Place = lessonClass.Place,
                Link = lessonClass.Link,
                Order = lessonClass.Order
            });

            return newClass;
        }

        public void UpdateLessonClass(int userId, LessonClassInfoViewModel newLessonClass)
        {
            var lesson =_lessonClasses.Single(l => l.ProfessorId == userId && l.Id == newLessonClass.Id);

            lesson.ClassDay = newLessonClass.ClassDay;
            lesson.Description = newLessonClass.Description;
            lesson.EndHour = newLessonClass.EndHour;
            lesson.StartHour = newLessonClass.StartHour;
            lesson.Place = newLessonClass.Place;
            lesson.Link = newLessonClass.Link;
            lesson.Order = newLessonClass.Order;
        }

        public void DeleteLessonClass(int userId, long id)
        {
            var lessonClass = _lessonClasses.Single(l => l.ProfessorId == userId && l.Id == id);
            _lessonClasses.Remove(lessonClass);
        }
    }
}

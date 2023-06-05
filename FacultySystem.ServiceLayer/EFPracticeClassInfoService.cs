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
    public class EFPracticeClassInfoService: IPracticeClassInfoService
    {
        IUnitOfWork _uow;
        readonly IDbSet<PracticeClassInfo> _practiceClasses;
        //private readonly Lazy<Professor> _professorService;
        public EFPracticeClassInfoService(IUnitOfWork uow)
        {
            _uow = uow;
            _practiceClasses = _uow.Set<PracticeClassInfo>();
        }

        public IEnumerable<PracticeClassInfoViewModel> GetPracticeClasses(int userId, long lessonId)
        {
            var classList = new List<PracticeClassInfoViewModel>();
            var practiceClasses = _practiceClasses
                .Where(l => l.ProfessorId == userId && l.LessonId == lessonId)
                .OrderByDescending(l => l.Order)
                .ThenBy(l => l.Id)
                .Cacheable()
                .ToList();

            foreach (var practiceClass in practiceClasses)
            {
                classList.Add(new PracticeClassInfoViewModel
                {
                    Id = practiceClass.Id,
                    PracticeClassDay = practiceClass.PracticeClassDay,
                    CreateDate = practiceClass.CreateDate,
                    TeacherName = practiceClass.TeacherName,
                    Description = practiceClass.Description,
                    EndHour = practiceClass.EndHour,
                    StartHour = practiceClass.StartHour,
                    Place = practiceClass.Place,
                    Link = practiceClass.Link,
                    Order = practiceClass.Order
                });
            }

            return classList;
        }

        public PracticeClassInfo CreatePracticeClass(int userId, PracticeClassInfoViewModel practiceClass)
        {
            var newPracticeClass = _practiceClasses.Add(new PracticeClassInfo
            {
                ProfessorId = userId,
                LessonId = practiceClass.LessonId,
                PracticeClassDay = practiceClass.PracticeClassDay,
                TeacherName = practiceClass.TeacherName,
                Description = practiceClass.Description,
                EndHour = practiceClass.EndHour,
                StartHour = practiceClass.StartHour,
                Place = practiceClass.Place,
                Link = practiceClass.Link,
                Order = practiceClass.Order
            });

            return newPracticeClass;
        }

        public void UpdatePracticeClass(int userId, PracticeClassInfoViewModel newPracticeClass)
        {
            var practiceClass = _practiceClasses.Single(l => l.ProfessorId == userId && l.Id == newPracticeClass.Id);

            practiceClass.PracticeClassDay = newPracticeClass.PracticeClassDay;
            practiceClass.Description = newPracticeClass.Description;
            practiceClass.TeacherName = newPracticeClass.TeacherName;
            practiceClass.EndHour = newPracticeClass.EndHour;
            practiceClass.StartHour = newPracticeClass.StartHour;
            practiceClass.Place = newPracticeClass.Place;
            practiceClass.Link = newPracticeClass.Link;
            practiceClass.Order = newPracticeClass.Order;
        }

        public void DeletePracticeClass(int userId, long id)
        {
            var practiceClass = _practiceClasses.Single(l => l.ProfessorId == userId && l.Id == id);
            _practiceClasses.Remove(practiceClass);
        }
    }
}

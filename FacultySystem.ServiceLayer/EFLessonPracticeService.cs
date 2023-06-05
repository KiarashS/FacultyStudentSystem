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
    public class EFLessonPracticeService: ILessonPracticeService
    {
        IUnitOfWork _uow;
        readonly IDbSet<LessonPractices> _lessonPractices;
        //private readonly Lazy<Professor> _professorService;
        public EFLessonPracticeService(IUnitOfWork uow)
        {
            _uow = uow;
            _lessonPractices = _uow.Set<LessonPractices>();
        }

        public IEnumerable<LessonPracticesViewModel> GetLessonPractices(int userId, long lessonId)
        {
            var practicesList = new List<LessonPracticesViewModel>();
            var practices = _lessonPractices
                .Where(l => l.ProfessorId == userId && l.LessonId == lessonId)
                .OrderByDescending(l => l.Order)
                .ThenByDescending(l => l.Id)
                .Cacheable()
                .ToList();

            foreach (var practice in practices)
            {
                practicesList.Add(new LessonPracticesViewModel
                {
                    UserId = practice.ProfessorId,
                    Id = practice.Id,
                    CreateDate = practice.CreateDate,
                    DeliverDate = practice.DeliverDate,
                    FileLink = practice.FileLink,
                    Title = practice.Title,
                    Description = practice.Description,
                    Filename = practice.Filename,
                    Link = practice.Link,
                    Order = practice.Order
                });
            }

            return practicesList;
        }

        public LessonPractices CreateLessonPractice(int userId, LessonPracticesViewModel practice)
        {
            var newPractices = _lessonPractices.Add(new LessonPractices
            {
                ProfessorId = userId,
                LessonId = practice.LessonId,
                DeliverDate = practice.DeliverDate,
                FileLink = practice.FileLink,
                Title = practice.Title,
                Description = practice.Description,
                Filename = practice.Filename,
                Link = practice.Link,
                Order = practice.Order
            });

            return newPractices;
        }

        public void UpdateLessonPractice(int userId, LessonPracticesViewModel newPractice)
        {
            var practice = _lessonPractices.Single(l => l.ProfessorId == userId && l.Id == newPractice.Id);

            practice.Title = newPractice.Title;
            practice.DeliverDate = newPractice.DeliverDate;
            practice.Description = newPractice.Description;
            practice.Filename = newPractice.Filename;
            practice.FileLink = practice.FileLink;
            practice.Link = newPractice.Link;
            practice.Order = newPractice.Order;
        }

        public void DeleteLessonPractice(int userId, long id)
        {
            var practice = _lessonPractices.Single(l => l.ProfessorId == userId && l.Id == id);
            _lessonPractices.Remove(practice);
        }

        public string GetFilename(int userId, long id)
        {
            return _lessonPractices
                .Where(l => l.ProfessorId == userId && l.Id == id)
                .Select(l => l.Filename)
                .SingleOrDefault();
        }
    }
}

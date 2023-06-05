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
using Z.EntityFramework.Plus;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFLessonService: ILessonService
    {
        IUnitOfWork _uow;
        readonly IDbSet<Lesson> _lessons;
        //private readonly Lazy<Professor> _professorService;
        public EFLessonService(IUnitOfWork uow)
        {
            _uow = uow;
            _lessons = _uow.Set<Lesson>();
        }

        public IEnumerable<LessonViewModel> GetLessons(int userId)
        {
            var lessonList = new List<LessonViewModel>();
            var lessons = _lessons
                .Where(l => l.ProfessorId == userId)
                .OrderByDescending(l => l.Order)
                .ThenBy(l => l.Id)
                .Cacheable()
                .ToList();

            foreach (var lesson in lessons)
            {
                lessonList.Add(new LessonViewModel
                {
                    Id = lesson.Id,
                    AcademicYear = lesson.AcademicYear,
                    CreateDate = lesson.CreateDate,
                    Description = lesson.Description,
                    Field = lesson.Field,
                    GroupNumber = lesson.GroupNumber,
                    LessonCode = lesson.LessonCode,
                    LessonGrade = lesson.LessonGrade,
                    LessonName = lesson.LessonName,
                    LessonState = lesson.LessonState,
                    LessonType = lesson.LessonType,
                    ProjectDescription = lesson.ProjectDescription,
                    ScoringDescription = lesson.ScoringDescription,
                    Semester = lesson.Semester,
                    Trend = lesson.Trend,
                    UnitNumber = lesson.UnitNumber,
                    UnitState = lesson.UnitState,
                    Reference = lesson.Reference,
                    Link = lesson.Link,
                    Order = lesson.Order
                });
            }

            return lessonList;
        }

        public Lesson CreateLesson(int userId, LessonViewModel lesson)
        {
            var newLesson = _lessons.Add(new Lesson
            {
                ProfessorId = userId,
                AcademicYear = lesson.AcademicYear,
                Description = lesson.Description,
                Field = lesson.Field,
                GroupNumber = lesson.GroupNumber,
                LessonCode = lesson.LessonCode,
                LessonGrade = lesson.LessonGrade,
                LessonName = lesson.LessonName,
                LessonState = lesson.LessonState,
                LessonType = lesson.LessonType,
                ProjectDescription = lesson.ProjectDescription,
                ScoringDescription = lesson.ScoringDescription,
                Semester = lesson.Semester,
                Trend = lesson.Trend,
                UnitNumber = lesson.UnitNumber,
                UnitState = lesson.UnitState,
                Reference = lesson.Reference,
                Link = lesson.Link,
                Order = lesson.Order
            });

            return newLesson;
        }

        public void UpdateLesson(int userId, LessonViewModel newLesson)
        {
            var lesson = _lessons.Single(l => l.ProfessorId == userId && l.Id == newLesson.Id);

            lesson.AcademicYear = newLesson.AcademicYear;
            lesson.Description = newLesson.Description;
            lesson.Field = newLesson.Field;
            lesson.GroupNumber = newLesson.GroupNumber;
            lesson.LessonCode = newLesson.LessonCode;
            lesson.LessonGrade = newLesson.LessonGrade;
            lesson.LessonName = newLesson.LessonName;
            lesson.LessonState = newLesson.LessonState;
            lesson.LessonType = newLesson.LessonType;
            lesson.ProjectDescription = newLesson.ProjectDescription;
            lesson.ScoringDescription = newLesson.ScoringDescription;
            lesson.Semester = newLesson.Semester;
            lesson.Trend = newLesson.Trend;
            lesson.UnitNumber = newLesson.UnitNumber;
            lesson.UnitState = newLesson.UnitState;
            lesson.Reference = newLesson.Reference;
            lesson.Link = newLesson.Link;
            lesson.Order = newLesson.Order;
        }

        public void DeleteLesson(int userId, long id)
        {
            var lessson = _lessons.Single(l => l.ProfessorId == userId && l.Id == id);
            _lessons.Remove(lessson);
        }

        public LessonIndexViewModel LessonIndex(int userId, long lessonId)
        {
            var lesson = _lessons
                .Where(l => l.ProfessorId == userId && l.Id == lessonId)
                .Select(l => new
                {
                    l.AcademicYear,
                    l.CreateDate,
                    l.Description,
                    l.Field,
                    l.GroupNumber,
                    l.Id,
                    l.LessonCode,
                    l.LessonGrade,
                    l.LessonName,
                    l.LessonState,
                    l.LessonType,
                    l.Link,
                    l.ProjectDescription,
                    l.ScoringDescription,
                    l.Semester,
                    l.Trend,
                    l.UnitNumber,
                    l.UnitState,
                    l.Reference,
                    HasLessonClassInfo = l.LessonClassInfos.Any(),
                    HasPracticeClassInfo = l.PracticeClassInfos.Any(),
                    HasLessonNews = l.LessonNews.Any(),
                    HasLessonFile = l.LessonFiles.Any(),
                    HasImportantDate = l.ImportantDates.Any(),
                    HasLessonPractice = l.LessonPractices.Any(),
                    HasLessonScore = l.LessonScores.Any()
                })
                .Cacheable()
                .SingleOrDefault();

            if(lesson == null)
            {
                return null;
            }

            return new LessonIndexViewModel
            {
                AcademicYear = lesson.AcademicYear,
                CreateDate = lesson.CreateDate,
                Description = lesson.Description,
                Field = lesson.Field,
                GroupNumber = lesson.GroupNumber,
                LessonCode = lesson.LessonCode,
                LessonGrade = lesson.LessonGrade,
                LessonId = lesson.Id,
                LessonName = lesson.LessonName,
                LessonState = lesson.LessonState,
                LessonType = lesson.LessonType,
                ProjectDescription = lesson.ProjectDescription,
                Link = lesson.Link,
                ScoringDescription = lesson.ScoringDescription,
                Trend = lesson.Trend,
                Semester = lesson.Semester,
                UnitNumber = lesson.UnitNumber,
                UnitState = lesson.UnitState,
                Reference = lesson.Reference,
                HasImportantDate = lesson.HasImportantDate,
                HasLessonClassInfo = lesson.HasLessonClassInfo,
                HasLessonFile = lesson.HasLessonFile,
                HasLessonNews = lesson.HasLessonNews,
                HasLessonPractice = lesson.HasLessonPractice,
                HasLessonScore = lesson.HasLessonScore,
                HasPracticeClassInfo = lesson.HasPracticeClassInfo
            };
        }

        public void DeleteAllLesson(int userId)
        {
            _lessons.Where(l => l.ProfessorId == userId).Delete();
        }
    }
}

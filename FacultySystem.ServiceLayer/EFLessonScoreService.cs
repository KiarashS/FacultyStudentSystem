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
    public class EFLessonScoreService : ILessonScoreService
    {
        IUnitOfWork _uow;
        readonly IDbSet<LessonScores> _lessonScores;
        //private readonly Lazy<Professor> _professorService;
        public EFLessonScoreService(IUnitOfWork uow)
        {
            _uow = uow;
            _lessonScores = _uow.Set<LessonScores>();
        }

        public IEnumerable<LessonScoresViewModel> GetLessonScores(int userId, long lessonId)
        {
            var scoresList = new List<LessonScoresViewModel>();
            var scores = _lessonScores
                .Where(l => l.ProfessorId == userId && l.LessonId == lessonId)
                .OrderByDescending(l => l.Order)
                .ThenByDescending(l => l.Id)
                .Cacheable()
                .ToList();

            foreach (var score in scores)
            {
                scoresList.Add(new LessonScoresViewModel
                {
                    UserId = score.ProfessorId,
                    Id = score.Id,
                    Title = score.Title,
                    CreateDate = score.CreateDate,
                    Description = score.Description,
                    Filename = score.Filename,
                    FileLink = score.FileLink,
                    Link = score.Link,
                    Order = score.Order
                });
            }

            return scoresList;
        }

        public LessonScores CreateLessonScore(int userId, LessonScoresViewModel score)
        {
            var newScore = _lessonScores.Add(new LessonScores
            {
                ProfessorId = userId,
                LessonId = score.LessonId,
                Title = score.Title,
                Description = score.Description,
                Filename = score.Filename,
                FileLink = score.FileLink,
                Link = score.Link,
                Order = score.Order
            });

            return newScore;
        }

        public void UpdateLessonScore(int userId, LessonScoresViewModel newScore)
        {
            var score = _lessonScores.Single(l => l.ProfessorId == userId && l.Id == newScore.Id);

            score.Title = newScore.Title;
            score.Description = newScore.Description;
            score.Filename = newScore.Filename;
            score.FileLink = score.FileLink;
            score.Link = newScore.Link;
            score.Order = newScore.Order;
        }

        public void DeleteLessonScore(int userId, long id)
        {
            var score = _lessonScores.Single(l => l.ProfessorId == userId && l.Id == id);
            _lessonScores.Remove(score);
        }

        public string GetFilename(int userId, long id)
        {
            return _lessonScores
                .Where(l => l.ProfessorId == userId && l.Id == id)
                .Select(l => l.Filename)
                .SingleOrDefault();
        }
    }
}

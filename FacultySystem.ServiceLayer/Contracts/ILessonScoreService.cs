using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface ILessonScoreService
    {
        IEnumerable<LessonScoresViewModel> GetLessonScores(int userId, long lessonId);
        LessonScores CreateLessonScore(int userId, LessonScoresViewModel score);
        void UpdateLessonScore(int userId, LessonScoresViewModel newScore);
        void DeleteLessonScore(int userId, long id);
        string GetFilename(int userId, long id);
    }
}

using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface ILessonFileService
    {
        IEnumerable<LessonFilesViewModel> GetLessonFiles(int userId, long lessonId);
        LessonFiles CreateLessonFile(int userId, LessonFilesViewModel file);
        void UpdateLessonFile(int userId, LessonFilesViewModel newFile);
        void DeleteLessonFile(int userId, long id);
        Tuple<string, string> GetFilename(int userId, long id);
    }
}

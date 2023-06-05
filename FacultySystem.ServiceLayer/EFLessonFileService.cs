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
    public class EFLessonFileService: ILessonFileService
    {
        IUnitOfWork _uow;
        readonly IDbSet<LessonFiles> _lessonFiles;
        //private readonly Lazy<Professor> _professorService;
        public EFLessonFileService(IUnitOfWork uow)
        {
            _uow = uow;
            _lessonFiles = _uow.Set<LessonFiles>();
        }

        public IEnumerable<LessonFilesViewModel> GetLessonFiles(int userId, long lessonId)
        {
            var filesList = new List<LessonFilesViewModel>();
            var files = _lessonFiles
                .Where(l => l.ProfessorId == userId && l.LessonId == lessonId)
                .OrderByDescending(l => l.Order)
                .ThenByDescending(l => l.Id)
                .Cacheable()
                .ToList();

            foreach (var file in files)
            {
                filesList.Add(new LessonFilesViewModel
                {
                    UserId = file.ProfessorId,
                    Id = file.Id,
                    Title = file.Title,
                    Description = file.Description,
                    CreateDate = file.CreateDate,
                    FileLink = file.FileLink,
                    Filename = file.Filename,
                    CoverFilename = file.CoverFilename,
                    FileType = file.FileType,
                    Link = file.Link,
                    Order = file.Order
                });
            }

            return filesList;
        }

        public LessonFiles CreateLessonFile(int userId, LessonFilesViewModel file)
        {
            var newFile = _lessonFiles.Add(new LessonFiles
            {
                ProfessorId = userId,
                LessonId = file.LessonId,
                Title = file.Title,
                Description = file.Description,
                FileLink = file.FileLink,
                Filename = file.Filename,
                CoverFilename = file.CoverFilename,
                FileType = file.FileType,
                Link = file.Link,
                Order = file.Order
            });

            return newFile;
        }

        public void UpdateLessonFile(int userId, LessonFilesViewModel newFile)
        {
            var file = _lessonFiles.Single(l => l.ProfessorId == userId && l.Id == newFile.Id);

            file.Title = newFile.Title;
            file.Description = newFile.Description;
            file.FileLink = newFile.FileLink;
            file.Filename = newFile.Filename;
            file.FileType = newFile.FileType;
            file.CoverFilename = newFile.CoverFilename;
            file.Link = newFile.Link;
            file.Order = newFile.Order;
        }

        public void DeleteLessonFile(int userId, long id)
        {
            var file = _lessonFiles.Single(l => l.ProfessorId == userId && l.Id == id);
            _lessonFiles.Remove(file);
        }

        public Tuple<string, string> GetFilename(int userId, long id)
        {
            var filenames = _lessonFiles
                .Where(l => l.ProfessorId == userId && l.Id == id)
                .Select(l => new { l.Filename, l.CoverFilename })
                .SingleOrDefault();
            
            return new Tuple<string, string>(filenames.Filename, filenames.CoverFilename);
        }
    }
}

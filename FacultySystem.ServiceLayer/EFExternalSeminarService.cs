using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.DataLayer.Context;
using System.Data.Entity;
using ContentManagementSystem.DomainClasses;
using EFSecondLevelCache;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFExternalSeminarService : IExternalSeminarService
    {
        IUnitOfWork _uow;
        readonly IDbSet<ExternalSeminarRecord> _seminars;
        //private readonly Lazy<Professor> _professorService;
        public EFExternalSeminarService(IUnitOfWork uow)
        {
            _uow = uow;
            _seminars = _uow.Set<ExternalSeminarRecord>();
        }

        public IEnumerable<ExternalSeminarRecordViewModel> GetListSeminars(int userId, string filterTitle, int startIndex = 0, int pageSize = 20)
        {
            var seminarList = new List<ExternalSeminarRecordViewModel>();
            var query = _seminars.AsQueryable();

            if (!string.IsNullOrEmpty(filterTitle))
            {
                query = query.Where(r => r.ProfessorId == userId && r.Title.Contains(filterTitle));
            }
            else
            {
                query = query.Where(r => r.ProfessorId == userId);
            }

            var seminars = query
                .Where(s => s.ProfessorId == userId)
                .OrderByDescending(s => s.Order)
                .ThenBy(s => s.Id)
                .Skip(startIndex)
                .Take(pageSize)
                .Cacheable()
                .ToList();

            foreach (var seminar in seminars)
            {
                seminarList.Add(new ExternalSeminarRecordViewModel
                {
                    Id = seminar.Id,
                    UserId = seminar.ProfessorId,
                    Abstract = seminar.Abstract,
                    Doi = seminar.Doi,
                    Authors = seminar.Authors,
                    Conference = seminar.Conference,
                    Date = seminar.Date,
                    Description = seminar.Description,
                    Filename = seminar.Filename,
                    Title = seminar.Title,
                    Link = seminar.Link,
                    Order = seminar.Order
                });
            }

            return seminarList;
        }

        public ExternalSeminarRecord CreateSeminar(int userId, ExternalSeminarRecordViewModel seminar)
        {
            var newSeminar = _seminars.Add(new ExternalSeminarRecord
            {
                ProfessorId = userId,
                Title = seminar.Title,
                Doi = seminar.Doi,
                Abstract = seminar.Abstract,
                Authors = seminar.Authors,
                Conference = seminar.Conference,
                Date = seminar.Date,
                Description = seminar.Description,
                Filename = seminar.Filename,
                Link = seminar.Link,
                Order = seminar.Order
            });

            return newSeminar;
        }

        public void UpdateSeminar(int userId, ExternalSeminarRecordViewModel newSeminar)
        {
            var seminar = _seminars.Single(s => s.ProfessorId == userId && s.Id == newSeminar.Id);

            seminar.Doi = newSeminar.Doi;
            seminar.Abstract = newSeminar.Abstract;
            seminar.Authors = newSeminar.Authors;
            seminar.Conference = newSeminar.Conference;
            seminar.Date = newSeminar.Date;
            seminar.Description = newSeminar.Description;
            seminar.Filename = newSeminar.Filename;
            seminar.Title = newSeminar.Title;
            seminar.Link = newSeminar.Link;
            seminar.Order = newSeminar.Order;
        }

        public void DeleteSeminar(int userId, long seminarId)
        {
            var seminar = _seminars.Single(s => s.ProfessorId == userId && s.Id == seminarId);
            _seminars.Remove(seminar);
        }

        public int TotalCount(int userId, string filterTitle)
        {
            var query = _seminars.AsQueryable();

            if (!string.IsNullOrEmpty(filterTitle))
            {
                query = query.Where(r => r.ProfessorId == userId && r.Title.Contains(filterTitle));
            }
            else
            {
                query = query.Where(r => r.ProfessorId == userId);
            }

            return query.Cacheable().Count();
        }

        public IEnumerable<ExternalSeminarRecordViewModel> GetProfessorSeminars(string pageId, int pageIndex = 0, int pageSize = 20)
        {
            var skipRecords = pageIndex * pageSize;
            var seminarList = new List<ExternalSeminarRecordViewModel>();
            var seminars = _seminars
                .Where(s => s.ProfessorProfile.PageId == pageId)
                .OrderByDescending(s => s.Order)
                .ThenBy(s => s.Id)
                .Skip(skipRecords)
                .Take(pageSize)
                .Cacheable()
                .ToList();

            foreach (var seminar in seminars)
            {
                seminarList.Add(new ExternalSeminarRecordViewModel
                {
                    Id = seminar.Id,
                    UserId = seminar.ProfessorId,
                    Abstract = seminar.Abstract,
                    Doi = seminar.Doi,
                    Authors = seminar.Authors,
                    Conference = seminar.Conference,
                    Date = seminar.Date,
                    Description = seminar.Description,
                    Filename = seminar.Filename,
                    Title = seminar.Title,
                    Link = seminar.Link,
                    Order = seminar.Order
                });
            }

            return seminarList;
        }

        public string GetFilename(int userId, long seminarId)
        {
            return _seminars
                .Where(s => s.ProfessorId == userId && s.Id == seminarId)
                .Select(s => s.Filename)
                .SingleOrDefault();
        }
    }
}

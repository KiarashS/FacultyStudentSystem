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
    public class EFInternalSeminarService : IInternalSeminarService
    {
        IUnitOfWork _uow;
        readonly IDbSet<InternalSeminarRecord> _seminars;
        //private readonly Lazy<Professor> _professorService;
        public EFInternalSeminarService(IUnitOfWork uow)
        {
            _uow = uow;
            _seminars = _uow.Set<InternalSeminarRecord>();
        }

        public IEnumerable<InternalSeminarRecordViewModel> GetListSeminars(int userId, string filterTitle, int startIndex = 0, int pageSize = 20)
        {
            var seminarList = new List<InternalSeminarRecordViewModel>();
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
                seminarList.Add(new InternalSeminarRecordViewModel
                {
                    Id = seminar.Id,
                    UserId = seminar.ProfessorId,
                    Abstract = seminar.Abstract,
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

        public InternalSeminarRecord CreateSeminar(int userId, InternalSeminarRecordViewModel seminar)
        {
            var newSeminar = _seminars.Add(new InternalSeminarRecord
            {
                ProfessorId = userId,
                Title = seminar.Title,
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

        public void UpdateSeminar(int userId, InternalSeminarRecordViewModel newSeminar)
        {
            var seminar = _seminars.Single(s => s.ProfessorId == userId && s.Id == newSeminar.Id);

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

        public IEnumerable<InternalSeminarRecordViewModel> GetProfessorSeminars(string pageId, int pageIndex = 0, int pageSize = 20)
        {
            var skipRecords = pageIndex * pageSize;
            var seminarList = new List<InternalSeminarRecordViewModel>();
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
                seminarList.Add(new InternalSeminarRecordViewModel
                {
                    Id = seminar.Id,
                    UserId = seminar.ProfessorId,
                    Abstract = seminar.Abstract,
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

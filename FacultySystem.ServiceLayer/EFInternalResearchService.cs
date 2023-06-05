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
    public class EFInternalResearchService : IInternalResearchService
    {
        IUnitOfWork _uow;
        readonly IDbSet<InternalResearchRecord> _researchs;
        //private readonly Lazy<Professor> _professorService;
        public EFInternalResearchService(IUnitOfWork uow)
        {
            _uow = uow;
            _researchs = _uow.Set<InternalResearchRecord>();
        }

        public IEnumerable<InternalResearchRecordViewModel> GetListResearchs(int userId, string filterTitle, int startIndex = 0, int pageSize = 20)
        {
            var researchList = new List<InternalResearchRecordViewModel>();
            var query = _researchs.AsQueryable();

            if (!string.IsNullOrEmpty(filterTitle))
            {
                query = query.Where(r => r.ProfessorId == userId && r.Title.Contains(filterTitle));
            }
            else
            {
                query = query.Where(r => r.ProfessorId == userId);
            }

            var researchs = query
                .Where(s => s.ProfessorId == userId)
                .OrderByDescending(s => s.Order)
                .ThenBy(s => s.Id)
                .Skip(startIndex)
                .Take(pageSize)
                .Cacheable()
                .ToList();

            foreach (var research in researchs)
            {
                researchList.Add(new InternalResearchRecordViewModel
                {
                    Id = research.Id,
                    UserId = research.ProfessorId,
                    Abstract = research.Abstract,
                    Authors = research.Authors,
                    Journal = research.Journal,
                    Year = research.Year,
                    Description = research.Description,
                    Filename = research.Filename,
                    Title = research.Title,
                    Link = research.Link,
                    Order = research.Order
                });
            }

            return researchList;
        }

        public InternalResearchRecord CreateResearch(int userId, InternalResearchRecordViewModel research)
        {
            var newResearch = _researchs.Add(new InternalResearchRecord
            {
                ProfessorId = userId,
                Abstract = research.Abstract,
                Authors = research.Authors,
                Journal = research.Journal,
                Year = research.Year,
                Description = research.Description,
                Filename = research.Filename,
                Title = research.Title,
                Link = research.Link,
                Order = research.Order
            });

            return newResearch;
        }

        public void UpdateResearch(int userId, InternalResearchRecordViewModel newResearch)
        {
            var seminar = _researchs.Single(s => s.ProfessorId == userId && s.Id == newResearch.Id);

            seminar.Abstract = newResearch.Abstract;
            seminar.Authors = newResearch.Authors;
            seminar.Journal = newResearch.Journal;
            seminar.Year = newResearch.Year;
            seminar.Description = newResearch.Description;
            seminar.Filename = newResearch.Filename;
            seminar.Title = newResearch.Title;
            seminar.Link = newResearch.Link;
            seminar.Order = newResearch.Order;
        }

        public void DeleteResearch(int userId, long researchId)
        {
            var research = _researchs.Single(s => s.ProfessorId == userId && s.Id == researchId);
            _researchs.Remove(research);
        }

        public int TotalCount(int userId, string filterTitle)
        {
            var query = _researchs.AsQueryable();

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

        public IEnumerable<InternalResearchRecordViewModel> GetProfessorResearchs(string pageId, int pageIndex = 0, int pageSize = 20)
        {
            var skipRecords = pageIndex * pageSize;
            var researchList = new List<InternalResearchRecordViewModel>();
            var researchs = _researchs
                .Where(s => s.ProfessorProfile.PageId == pageId)
                .OrderByDescending(s => s.Order)
                .ThenBy(s => s.Id)
                .Skip(skipRecords)
                .Take(pageSize)
                .Cacheable()
                .ToList();

            foreach (var research in researchs)
            {
                researchList.Add(new InternalResearchRecordViewModel
                {
                    Id = research.Id,
                    UserId = research.ProfessorId,
                    Abstract = research.Abstract,
                    Authors = research.Authors,
                    Journal = research.Journal,
                    Year = research.Year,
                    Description = research.Description,
                    Filename = research.Filename,
                    Title = research.Title,
                    Link = research.Link,
                    Order = research.Order
                });
            }

            return researchList;
        }

        public string GetFilename(int userId, long researchId)
        {
            return _researchs
                .Where(r => r.ProfessorId == userId && r.Id == researchId)
                .Select(r => r.Filename)
                .SingleOrDefault();
        }
    }
}

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
using System.Data.Entity.Migrations;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFExternalResearchService : IExternalResearchService
    {
        IUnitOfWork _uow;
        readonly IDbSet<ExternalResearchRecord> _researchs;
        //private readonly Lazy<Professor> _professorService;
        public EFExternalResearchService(IUnitOfWork uow)
        {
            _uow = uow;
            _researchs = _uow.Set<ExternalResearchRecord>();
        }

        public IEnumerable<ExternalResearchRecordViewModel> GetListResearchs(int userId, string filterTitle, int startIndex = 0, int pageSize = 20)
        {
            var researchList = new List<ExternalResearchRecordViewModel>();
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
                .OrderByDescending(s => s.Order)
                .ThenBy(s => s.Id)
                .Skip(startIndex)
                .Take(pageSize)
                .Cacheable()
                .ToList();

            foreach (var research in researchs)
            {
                researchList.Add(new ExternalResearchRecordViewModel
                {
                    Id = research.Id,
                    UserId = research.ProfessorId,
                    Doi = research.Doi,
                    Issue = research.Issue,
                    Pages = research.Pages,
                    Volume = research.Volume,
                    TimesCited = research.TimesCited,
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

        public ExternalResearchRecord CreateResearch(int userId, ExternalResearchRecordViewModel research)
        {
            research.Doi = research.Doi.Trim();

            var newResearch = _researchs.Add(new ExternalResearchRecord
            {
                ProfessorId = userId,
                Abstract = research.Abstract,
                Doi = research.Doi,
                Issue = research.Issue,
                Pages = research.Pages,
                Volume = research.Volume,
                TimesCited = research.TimesCited,
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

        public void UpdateResearch(int userId, ExternalResearchRecordViewModel newResearch)
        {
            var seminar = _researchs.Single(s => s.ProfessorId == userId && s.Id == newResearch.Id);

            seminar.Abstract = newResearch.Abstract;
            seminar.Authors = newResearch.Authors;
            seminar.Journal = newResearch.Journal;
            seminar.Doi = newResearch.Doi.Trim();
            seminar.Issue = newResearch.Issue;
            seminar.Pages = newResearch.Pages;
            seminar.Volume = newResearch.Volume;
            seminar.TimesCited = newResearch.TimesCited;
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

        public IEnumerable<ExternalResearchRecordViewModel> GetProfessorResearchs(string pageId, int pageIndex = 0, int pageSize = 20)
        {
            var skipRecords = pageIndex * pageSize;
            var researchList = new List<ExternalResearchRecordViewModel>();
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
                researchList.Add(new ExternalResearchRecordViewModel
                {
                    Id = research.Id,
                    UserId = research.ProfessorId,
                    Abstract = research.Abstract,
                    Authors = research.Authors,
                    Journal = research.Journal,
                    Doi = research.Doi,
                    Issue = research.Issue,
                    Pages = research.Pages,
                    Volume = research.Volume,
                    TimesCited = research.TimesCited,
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

        public void UpdateExternalArticlesByFetcher(int userId, IList<ExternalResearchRecord> articles)
        {
            foreach (var item in articles)
            {
                item.Doi = item.Doi.Trim();
                item.ProfessorId = userId;
            }

            _researchs.AddOrUpdate(a => new { a.ProfessorId, a.Doi }, articles.ToArray());
        }

        public bool IsExist(int userId, string doi)
        {
            return _researchs.Any(a => a.ProfessorId == userId && a.Doi == doi);
        }

        public string GetDoi(int userId, long researchId)
        {
            return _researchs
                .Where(r => r.ProfessorId == userId && r.Id == researchId)
                .Select(r => r.Doi)
                .SingleOrDefault();
        }
    }
}

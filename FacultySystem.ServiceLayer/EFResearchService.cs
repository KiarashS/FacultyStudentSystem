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
    public class EFResearchService : IResearchRecordService
    {
        IUnitOfWork _uow;
        readonly IDbSet<ResearchRecord> _researchs;
        public EFResearchService(IUnitOfWork uow)
        {
            _uow = uow;
            _researchs = _uow.Set<ResearchRecord>();
        }

        public IEnumerable<ResearchViewModel> GetResearchs(int userId)
        {
            var researchList = new List<ResearchViewModel>();
            var researchs = _researchs
                .Where(r => r.ProfessorId == userId)
                .OrderByDescending(r => r.Order)
                .ThenBy(r => r.Id)
                .Cacheable()
                .ToList();

            foreach (var research in researchs)
            {
                researchList.Add(new ResearchViewModel
                {
                    Id = research.Id,
                    Title = research.Title,
                    Place = research.Place,
                    StartTime = research.StartTime,
                    EndTime = research.EndTime,
                    Description = research.Description,
                    Link = research.Link,
                    Order = research.Order
                });
            }

            return researchList;
        }

        public ResearchRecord CreateResearch(int userId, ResearchViewModel research)
        {
            var newResearch = _researchs.Add(new ResearchRecord
            {
                ProfessorId = userId,
                Title = research.Title,
                Place = research.Place,
                StartTime = research.StartTime,
                EndTime = research.EndTime,
                Description = research.Description,
                Link = research.Link,
                Order = research.Order
            });

            return newResearch;
        }

        public void UpdateResearch(int userId, ResearchViewModel newResearch)
        {
            var research = _researchs.Single(r => r.ProfessorId == userId && r.Id == newResearch.Id);

            research.Title = newResearch.Title;
            research.Place = newResearch.Place;
            research.StartTime = newResearch.StartTime;
            research.EndTime = newResearch.EndTime;
            research.Description = newResearch.Description;
            research.Link = newResearch.Link;
            research.Order = newResearch.Order;
        }

        public void DeleteResearch(int userId, long id)
        {
            var research = _researchs.Single(r => r.ProfessorId == userId && r.Id == id);
            _researchs.Remove(research);
        }
    }
}

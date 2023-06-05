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
    public class EFThesisService : IThesisService
    {
        IUnitOfWork _uow;
        readonly IDbSet<Thesis> _theses;
        public EFThesisService(IUnitOfWork uow)
        {
            _uow = uow;
            _theses = _uow.Set<Thesis>();
        }

        public IEnumerable<ThesisViewModel> GetThesisList(int userId)
        {
            var thesisList = new List<ThesisViewModel>();
            var theses = _theses
                .Where(t => t.ProfessorId == userId)
                .OrderByDescending(t => t.Order)
                .ThenBy(t => t.Id)
                .Cacheable()
                .ToList();

            foreach (var thesis in theses)
            {
                thesisList.Add(new ThesisViewModel
                {
                    Id = thesis.Id,
                    Title = thesis.Title,
                    ThesisPost = thesis.ThesisPost,
                    ThesisGrade = thesis.ThesisGrade,
                    ThesisState = thesis.ThesisState,
                    ThesisType = thesis.ThesisType,
                    Executers = thesis.Executers,
                    Description = thesis.Description,
                    Time = thesis.Time,
                    University = thesis.University,
                    Link = thesis.Link,
                    Order = thesis.Order
                });
            }

            return thesisList;
        }

        public Thesis CreateThesis(int userId, ThesisViewModel thesis)
        {
            var newThesis = _theses.Add(new Thesis
            {
                ProfessorId = userId,
                Title = thesis.Title,
                ThesisPost = thesis.ThesisPost,
                ThesisGrade = thesis.ThesisGrade,
                ThesisState = thesis.ThesisState,
                ThesisType = thesis.ThesisType,
                Executers = thesis.Executers,
                University = thesis.University,
                Description = thesis.Description,
                Time = thesis.Time,
                Link = thesis.Link,
                Order = thesis.Order
            });

            return newThesis;
        }

        public void UpdateThesis(int userId, ThesisViewModel newThesis)
        {
            var thesis = _theses.Single(t => t.ProfessorId == userId && t.Id == newThesis.Id);

            thesis.Title = newThesis.Title;
            thesis.ThesisPost = newThesis.ThesisPost;
            thesis.ThesisGrade = newThesis.ThesisGrade;
            thesis.ThesisState = newThesis.ThesisState;
            thesis.ThesisType = newThesis.ThesisType;
            thesis.Executers = newThesis.Executers;
            thesis.University = newThesis.University;
            thesis.Description = newThesis.Description;
            thesis.Time = newThesis.Time;
            thesis.Link = newThesis.Link;
            thesis.Order = newThesis.Order;
        }

        public void DeleteThesis(int userId, long id)
        {
            var thesis = _theses.Single(t => t.ProfessorId == userId && t.Id == id);
            _theses.Remove(thesis);
        }
    }
}

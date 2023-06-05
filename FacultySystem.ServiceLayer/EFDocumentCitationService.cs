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
    public class EFDocumentCitationService : IDocumentCitationService
    {
        IUnitOfWork _uow;
        readonly IDbSet<DocumentCitation> _citations;
        //private readonly Lazy<Professor> _professorService;
        public EFDocumentCitationService(IUnitOfWork uow)
        {
            _uow = uow;
            _citations = _uow.Set<DocumentCitation>();
        }

        public IEnumerable<DocumentCitationViewModel> GetListCitations(int userId, int startIndex = 0, int pageSize = 10)
        {
            var citationList = new List<DocumentCitationViewModel>();
            var citations = _citations
                .Where(c => c.ProfessorId == userId)
                .OrderByDescending(c => c.Year)
                .Skip(startIndex)
                .Take(pageSize)
                .Cacheable()
                .ToList();

            foreach (var citation in citations)
            {
                citationList.Add(new DocumentCitationViewModel
                {
                    Id = citation.Id,
                    Year = citation.Year,
                    Citation = citation.Citation,
                    Document = citation.Document,
                    Source = citation.Source
                });
            }

            return citationList;
        }

        public int GetListCitationCount(int userId)
        {
            return _citations.Where(c => c.ProfessorId == userId).Cacheable().Count();
        }

        public DocumentCitation CreateCitation(int userId, DocumentCitationViewModel citation)
        {
            var newCitation = _citations.Add(new DocumentCitation
            {
                ProfessorId = userId,
                Year = citation.Year,
                Citation = citation.Citation,
                Document = citation.Source == DocSource.Scopus ? citation.Document : null,
                Source = citation.Source
            });

            return newCitation;
        }

        public void UpdateCitation(int userId, DocumentCitationViewModel newCitation)
        {
            var citation = _citations.Single(c => c.ProfessorId == userId && c.Id == newCitation.Id);

            citation.Year = newCitation.Year;
            citation.Citation = newCitation.Citation;
            citation.Document = citation.Source == DocSource.Scopus ? citation.Document : null;
            citation.Source = newCitation.Source;
        }

        public void DeleteCitation(int userId, long id)
        {
            var citation = _citations.Single(c => c.ProfessorId == userId && c.Id == id);
            _citations.Remove(citation);
        }

        public ScopusChartViewModel GetScopusChartData(string pageId)
        {
            var scopusChartViewModel = new ScopusChartViewModel();
            var citations = _citations
                        .Where(dc => dc.ProfessorProfile.PageId == pageId && dc.Source == DocSource.Scopus)
                        .Select(dc => new { dc.Year, dc.Citation, dc.Document })
                        .Distinct()
                        .OrderBy(dc => dc.Year)
                        .ToList();

            scopusChartViewModel.Years = citations.Select(c => c.Year).ToList();
            scopusChartViewModel.Citations = citations.Select(c => c.Citation).ToList();
            scopusChartViewModel.Documents = citations.Select(c => c.Document).ToList();

            return scopusChartViewModel;
        }

        public GoogleChartViewModel GetGoogleChartData(string pageId)
        {
            var googleChartViewModel = new GoogleChartViewModel();
            var citations = _citations
                        .Where(dc => dc.ProfessorProfile.PageId == pageId && dc.Source == DocSource.Google)
                        .Select(dc => new { dc.Year, dc.Citation })
                        .Distinct()
                        .OrderBy(dc => dc.Year)
                        .ToList();

            googleChartViewModel.Years = citations.Select(c => c.Year).ToList();
            googleChartViewModel.Citations = citations.Select(c => c.Citation).ToList();

            return googleChartViewModel;
        }

        public void AddOrUpdate(int userId, IList<DocumentCitation> scopusCitation, IList<DocumentCitation> googleCitation, bool updateScopus = false, bool updateGoogle = false)
        {
            if (updateScopus && scopusCitation != null && scopusCitation.Count() > 0)
            {
                foreach (var item in scopusCitation)
                {
                    item.ProfessorId = userId;
                    item.Source = DocSource.Scopus;

                    if (item.Document == null)
                    {
                        item.Document = 0;
                    }
                }
                //for (var i = 0; i < scopusCitation.Count; i++)
                //{
                //    scopusCitation[i].ProfessorId = userId;
                //    scopusCitation[i].Source = DocSource.Scopus;

                //    if (scopusCitation[i].Document == null)
                //    {
                //        scopusCitation[i].Document = 0;
                //    }
                //}
                _citations.AddOrUpdate(c => new { c.ProfessorId, c.Source, c.Year }, scopusCitation.ToArray());
            }

            if (updateGoogle && googleCitation != null && googleCitation.Count() > 0)
            {
                foreach (var item in googleCitation)
                {
                    item.ProfessorId = userId;
                    item.Source = DocSource.Google;
                }
                _citations.AddOrUpdate(c => new { c.ProfessorId, c.Source, c.Year }, googleCitation.ToArray());
            }
        }

        public bool IsExist(int userId, int year, DocSource source)
        {
            return _citations.Any(c => c.ProfessorId == userId && c.Year == year && c.Source == source);
        }
    }
}

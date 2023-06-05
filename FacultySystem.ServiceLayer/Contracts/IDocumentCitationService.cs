using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IDocumentCitationService
    {
        IEnumerable<DocumentCitationViewModel> GetListCitations(int userId, int startIndex = 0, int pageSize = 10);
        int GetListCitationCount(int userId);
        DocumentCitation CreateCitation(int userId, DocumentCitationViewModel citation);
        void UpdateCitation(int userId, DocumentCitationViewModel newCitation);
        void DeleteCitation(int userId, long id);
        ScopusChartViewModel GetScopusChartData(string pageId);
        GoogleChartViewModel GetGoogleChartData(string pageId);
        void AddOrUpdate(int userId, IList<DocumentCitation> scopusCitation, IList<DocumentCitation> googleCitation, bool updateScopus = false, bool updateGoogle = false);
        bool IsExist(int userId, int year, DocSource source);
    }
}

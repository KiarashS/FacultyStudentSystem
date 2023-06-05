using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IExternalResearchService
    {
        IEnumerable<ExternalResearchRecordViewModel> GetListResearchs(int userId, string filterTitle, int startIndex = 0, int pageSize = 20);
        int TotalCount(int userId, string filterTitle);
        ExternalResearchRecord CreateResearch(int userId, ExternalResearchRecordViewModel research);
        void UpdateResearch(int userId, ExternalResearchRecordViewModel newResearch);
        void DeleteResearch(int userId, long researchId);
        IEnumerable<ExternalResearchRecordViewModel> GetProfessorResearchs(string pageId, int pageIndex = 0, int pageSize = 20);
        string GetFilename(int userId, long researchId);
        void UpdateExternalArticlesByFetcher(int userId, IList<ExternalResearchRecord> articles);
        bool IsExist(int userId, string doi);
        string GetDoi(int userId, long researchId);
    }
}

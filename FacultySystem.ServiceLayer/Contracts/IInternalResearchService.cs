using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IInternalResearchService
    {
        IEnumerable<InternalResearchRecordViewModel> GetListResearchs(int userId, string filterTitle, int startIndex = 0, int pageSize = 20);
        int TotalCount(int userId, string filterTitle);
        InternalResearchRecord CreateResearch(int userId, InternalResearchRecordViewModel research);
        void UpdateResearch(int userId, InternalResearchRecordViewModel newResearch);
        void DeleteResearch(int userId, long researchId);
        IEnumerable<InternalResearchRecordViewModel> GetProfessorResearchs(string pageId, int pageIndex = 0, int pageSize = 20);
        string GetFilename(int userId, long researchId);
    }
}

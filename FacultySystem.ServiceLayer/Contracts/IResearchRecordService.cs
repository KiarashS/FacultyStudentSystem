using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IResearchRecordService
    {
        IEnumerable<ResearchViewModel> GetResearchs(int userId);
        ResearchRecord CreateResearch(int userId, ResearchViewModel research);
        void UpdateResearch(int userId, ResearchViewModel newResearch);
        void DeleteResearch(int userId, long id);
    }
}

using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IAcademicRankService
    {
        IEnumerable<AcademicRankViewModel> GetListAcademicRanks();
        IEnumerable<AcademicRankViewModel> GetAcademicRanks();
        AcademicRank CreateAcademicRank(AcademicRankViewModel academicRank);
        void UpdateAcademicRank(AcademicRankViewModel newAcademicRank);
        bool DeleteAcademicRank(int id);
        bool ExistName(int id, string name);
        int GetIdByName(string name);
    }
}

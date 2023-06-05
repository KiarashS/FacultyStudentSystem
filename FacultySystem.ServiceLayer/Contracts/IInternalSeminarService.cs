using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IInternalSeminarService
    {
        IEnumerable<InternalSeminarRecordViewModel> GetListSeminars(int userId, string filterTitle, int startIndex = 0, int pageSize = 20);
        int TotalCount(int userId, string filterTitle);
        InternalSeminarRecord CreateSeminar(int userId, InternalSeminarRecordViewModel seminar);
        void UpdateSeminar(int userId, InternalSeminarRecordViewModel newSeminar);
        void DeleteSeminar(int userId, long seminarId);
        IEnumerable<InternalSeminarRecordViewModel> GetProfessorSeminars(string pageId, int pageIndex = 0, int pageSize = 20);
        string GetFilename(int userId, long seminarId);
    }
}

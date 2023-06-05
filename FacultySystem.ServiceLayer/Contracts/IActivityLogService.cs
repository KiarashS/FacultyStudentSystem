using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IActivityLogService
    {
        IEnumerable<ActivityLogViewModel> GetActivityLogs(string email, int startIndex = 0, int pageSize = 50);
        ActivityLog CreateActivityLog(ActivityLogViewModel activityLog);
        void DeleteActivityLog(DateTime? to);
        int TotalLogsCount(string filterEmail);
    }
}

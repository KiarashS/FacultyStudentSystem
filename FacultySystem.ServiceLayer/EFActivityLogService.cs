using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using EFSecondLevelCache;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFActivityLogService: IActivityLogService
    {
        IUnitOfWork _uow;
        readonly IDbSet<ActivityLog> _logs;
        public EFActivityLogService(IUnitOfWork uow)
        {
            _uow = uow;
            _logs = _uow.Set<ActivityLog>();
        }

        public IEnumerable<ActivityLogViewModel> GetActivityLogs(string email, int startIndex = 0, int pageSize = 50)
        {
            var logs = new List<ActivityLogViewModel>();
            var logsQuery = _logs.AsQueryable();

            if (!string.IsNullOrEmpty(email))
            {
                logsQuery = logsQuery.Where(l => l.ActionBy.Contains(email) || l.Message.Contains(email));
            }

            var list = logsQuery
                .OrderByDescending(l => l.Id)
                .Skip(startIndex)
                .Take(pageSize)
                .Cacheable()
                .ToList();

            foreach (var log in list)
            {
                logs.Add(new ActivityLogViewModel
                {
                    Id = log.Id,
                    ActionBy = log.ActionBy,
                    ActionDate = log.ActionDate,
                    ActionLevel = log.ActionLevel,
                    ActionType = log.ActionType,
                    Message = log.Message,
                    SourceAddress = log.SourceAddress,
                    Url = log.Url
                });
            }

            return logs;
        }

        public ActivityLog CreateActivityLog(ActivityLogViewModel activityLog)
        {
            var newLog = new ActivityLog
            {
                ActionBy = activityLog.ActionBy,
                Url = activityLog.Url,
                SourceAddress = activityLog.SourceAddress,
                Message = activityLog.Message,
                ActionType = activityLog.ActionType,
                ActionLevel = activityLog.ActionLevel
            };

            _logs.Add(newLog);

            return newLog;
        }

        public void DeleteActivityLog(DateTime? to)
        {
            var deleteTo = DateTime.UtcNow.AddDays(-31);
            if(to.HasValue)
            {
                deleteTo = (DateTime)to;
            }

            var affectedRows = _uow.Database.ExecuteSqlCommand(
                "DELETE From [dbo].[ActivityLogs] WHERE ([ActionDate] = @p0)", deleteTo);
        }

        public int TotalLogsCount(string filterEmail)
        {
            var logsCount = _logs.AsQueryable();

            if (!string.IsNullOrEmpty(filterEmail))
            {
                logsCount = logsCount.Where(l => l.ActionBy.Contains(filterEmail) || l.Message.Contains(filterEmail));
            }

            return logsCount.Select(l => l.Id).Cacheable().Count();
        }
    }
}

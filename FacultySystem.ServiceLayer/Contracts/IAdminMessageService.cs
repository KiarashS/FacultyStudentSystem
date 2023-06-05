using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IAdminMessageService
    {
        IEnumerable<AdminMessageViewModel> GetListUserMessages(int userId, int startIndex = 0, int pageSize = 10);
        int GetListUserMessagesCount(int userId);
        IEnumerable<AdminMessageViewModel> GetListManagementMessages(string lastname, string pageId, string email, bool pendingOnly = false, int startIndex = 0, int pageSize = 20);
        int GetListManagementMessagesCount(string lastname, string pageId, string email, bool pendingOnly = false);
        AdminMessage CreateAdminMessage(int userId, AdminMessageViewModel message);
        void ReplyAdminMessage(long messageId, string replyContent);
        int CountOfPendingMessages();
        void DeleteAdminMessage(long messageId, int userId = 0, bool isAdmin = false);
    }
}

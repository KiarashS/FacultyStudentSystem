using ContentManagementSystem.ServiceLayer.Contracts;
using System.Collections.Generic;
using System.Linq;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.DataLayer.Context;
using System.Data.Entity;
using ContentManagementSystem.DomainClasses;
using EFSecondLevelCache;
using Z.EntityFramework.Plus;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFAdminMessageService: IAdminMessageService
    {
        IUnitOfWork _uow;
        readonly IDbSet<AdminMessage> _messages;
        //private readonly Lazy<Professor> _professorService;
        public EFAdminMessageService(IUnitOfWork uow)
        {
            _uow = uow;
            _messages = _uow.Set<AdminMessage>();
        }

        public IEnumerable<AdminMessageViewModel> GetListUserMessages(int userId, int startIndex = 0, int pageSize = 10)
        {
            var messageList = new List<AdminMessageViewModel>();
            var messages = _messages
                .Where(m => m.ProfessorId == userId)
                .OrderByDescending(m => m.Id)
                .Skip(startIndex)
                .Take(pageSize)
                .Cacheable()
                .ToList();

            foreach (var message in messages)
            {
                messageList.Add(new AdminMessageViewModel
                {
                    Id = message.Id,
                    Content = message.Content,
                    CreateDate = message.CreateDate,
                    ReplyContent = message.ReplyContent,
                    State = message.State,
                    Title = message.Title
                });
            }

            return messageList;
        }

        public int GetListUserMessagesCount(int userId)
        {
            return _messages
                .Where(m => m.ProfessorId == userId)
                .Cacheable()
                .Count();
        }

        public IEnumerable<AdminMessageViewModel> GetListManagementMessages(string lastname, string pageId, string email, bool pendingOnly = false, int startIndex = 0, int pageSize = 20)
        {
            var messageList = new List<AdminMessageViewModel>();
            var query = _messages
                        .Include(m => m.ProfessorDetails)
                        .Include(m => m.ProfessorDetails.UserDetails)
                        .AsQueryable();

            if (!string.IsNullOrEmpty(lastname))
            {
                query = query.Where(u => u.ProfessorDetails.UserDetails.LastName.Contains(lastname));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.ProfessorDetails.UserDetails.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(pageId))
            {
                query = query.Where(p => p.ProfessorDetails.PageId.Contains(pageId));
            }

            if (pendingOnly)
            {
                query = query.Where(m => m.State == AdminMessageState.Posted);
            }

            var messages = query
                            .OrderByDescending(m => m.Id)
                            .Skip(startIndex)
                            .Take(pageSize)
                            .Cacheable()
                            .ToList();
            foreach (var message in messages)
            {
                messageList.Add(new AdminMessageViewModel
                {
                    Id = message.Id,
                    UserId = message.ProfessorId,
                    Content = message.Content,
                    CreateDate = message.CreateDate,
                    ReplyContent = message.ReplyContent,
                    State = message.State,
                    Title = message.Title,
                    Email = message.ProfessorDetails.UserDetails.Email,
                    Firstname = message.ProfessorDetails.UserDetails.FirstName,
                    Lastname = message.ProfessorDetails.UserDetails.LastName,
                    PageId = message.ProfessorDetails.PageId,
                });
            }

            return messageList;
        }

        public int GetListManagementMessagesCount(string lastname, string pageId, string email, bool pendingOnly = false)
        {
            var query = _messages
                //.IncludeOptimized(m => m.ProfessorDetails)
                //.IncludeOptimized(m => m.ProfessorDetails.UserDetails)
                .AsQueryable();

            if (!string.IsNullOrEmpty(lastname))
            {
                query = query.Where(u => u.ProfessorDetails.UserDetails.LastName.Contains(lastname));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.ProfessorDetails.UserDetails.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(pageId))
            {
                query = query.Where(p => p.ProfessorDetails.PageId.Contains(pageId));
            }

            if (pendingOnly)
            {
                query = query.Where(m => m.State == AdminMessageState.Posted);
            }

            return query.Cacheable().Count();
        }

        public int CountOfPendingMessages()
        {
            return _messages
                .Where(m => m.State == AdminMessageState.Posted)
                .Cacheable()
                .Count();
        }

        public AdminMessage CreateAdminMessage(int userId, AdminMessageViewModel message)
        {
            var newMessage = _messages.Add(new AdminMessage
            {
                ProfessorId = userId,
                Title = message.Title,
                Content = message.Content
            });

            return newMessage;
        }

        public void ReplyAdminMessage(long messageId, string replyContent)
        {
            var message = _messages.Single(m => m.Id == messageId);

            message.ReplyContent = replyContent;
            message.State = AdminMessageState.Done;
        }

        public void DeleteAdminMessage(long messageId, int userId = 0, bool isAdmin = false)
        {
            var query = _messages.AsQueryable();
            if (isAdmin)
            {
                query = query.Where(m => m.Id == messageId);
            }
            else
            {
                query = query.Where(m => m.ProfessorId == userId && m.Id == messageId);
            }

            var message = query.Single();
            _messages.Remove(message);
        }
    }
}

using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using ContentManagementSystem.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Web.Areas.Dashboard.Controllers
{
    [SiteAuthorize(Roles = ConstantsUtil.AdminProfessorRoles)]
    public partial class AdminMessageController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IAdminMessageService _messageService;

        public AdminMessageController(IUnitOfWork uow, IAdminMessageService messageService)
        {
            _uow = uow;
            _messageService = messageService;
        }

        [HttpPost]
        [AjaxOnly]
        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        public virtual ActionResult List(int jtStartIndex = 0, int jtPageSize = 10)
        {
            var messages = _messageService.GetListUserMessages(CurrentUserId, jtStartIndex, jtPageSize);
            var totalRecordsCount = _messageService.GetListUserMessagesCount(CurrentUserId);

            return Json(new { Result = "OK", TotalRecordCount = totalRecordsCount, Records = messages });
        }

        [HttpPost]
        [AjaxOnly]
        [SiteAuthorize(Roles = ConstantsUtil.AdminRole)]
        public virtual ActionResult ListManagement(string filterLastname, string filterPageId, string filterEmail, bool filterPendingOnly = false, int jtStartIndex = 0, int jtPageSize = 20)
        {
            var messages = _messageService.GetListManagementMessages(filterLastname, filterPageId, filterEmail, filterPendingOnly, jtStartIndex, jtPageSize);
            var totalRecordsCount = _messageService.GetListManagementMessagesCount(filterLastname, filterPageId, filterEmail, filterPendingOnly);

            return Json(new { Result = "OK", TotalRecordCount = totalRecordsCount, Records = messages });
        }

        [HttpPost]
        [AjaxOnly]
        [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(AdminMessageViewModel message)
        {
            var newMessage = _messageService.CreateAdminMessage(CurrentUserId, message);
            _uow.SaveAllChanges();

            message.Id = newMessage.Id;

            return Json(new { Result = "OK", Record = message });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        [SiteAuthorize(Roles = ConstantsUtil.AdminRole)]
        public virtual ActionResult Update(AdminMessageViewModel message)
        {
            _messageService.ReplyAdminMessage(message.Id, message.ReplyContent);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            if (User.IsInRole(ConstantsUtil.AdminRole))
            {
                _messageService.DeleteAdminMessage(id, isAdmin: true);
            }
            else
            {
                _messageService.DeleteAdminMessage(id, userId: CurrentUserId, isAdmin: false);
            }
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
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
    [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
    public partial class MembershipController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IMembershipService _membershipService;
        private readonly IProfessorService _professorService;

        public MembershipController(IUnitOfWork uow, IMembershipService membershipService, IProfessorService professorService)
        {
            _uow = uow;
            _membershipService = membershipService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var memberships = _membershipService.GetMemberships(CurrentUserId);
            return Json(new { Result = "OK", Records = memberships });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(ProfessorMembershipViewModel membership)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newMembership = _membershipService.CreateMembership(CurrentUserId, membership);
            _uow.SaveAllChanges();

            membership.Id = newMembership.Id;

            return Json(new { Result = "OK", Record = membership });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(ProfessorMembershipViewModel membership)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _membershipService.UpdateMembership(CurrentUserId, membership);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _membershipService.DeleteMembership(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
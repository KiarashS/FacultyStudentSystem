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
    public partial class PublicationController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IPublicationService _publicationService;
        private readonly IProfessorService _professorService;

        public PublicationController(IUnitOfWork uow, IPublicationService publicationService, IProfessorService professorService)
        {
            _uow = uow;
            _publicationService = publicationService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var publications = _publicationService.GetPublications(CurrentUserId);
            return Json(new { Result = "OK", Records = publications });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(PublicationViewModel publication)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newPublication = _publicationService.CreatePublication(CurrentUserId, publication);
            _uow.SaveAllChanges();

            publication.Id = newPublication.Id;

            return Json(new { Result = "OK", Record = publication });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(PublicationViewModel publication)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _publicationService.UpdatePublication(CurrentUserId, publication);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _publicationService.DeletePublication(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
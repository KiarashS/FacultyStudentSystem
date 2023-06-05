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
    public partial class AddressController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IAddressService _addressService;
        private readonly IProfessorService _professorService;

        public AddressController(IUnitOfWork uow, IAddressService addressService, IProfessorService professorService)
        {
            _uow = uow;
            _addressService = addressService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var addresses = _addressService.GetListAddresses(CurrentUserId);
            return Json(new { Result = "OK", Records = addresses });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(AddressListViewModel address)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newAddress = _addressService.CreateAddress(CurrentUserId, address);
            _uow.SaveAllChanges();

            address.AddressId = newAddress.AddressId;

            return Json(new { Result = "OK", Record = address });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(AddressListViewModel address)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _addressService.UpdateAddress(CurrentUserId, address);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(int addressId)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _addressService.DeleteAddress(CurrentUserId, addressId);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}
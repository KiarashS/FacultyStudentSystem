using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IAddressService
    {
        IEnumerable<AddressListViewModel> GetListAddresses(int userId);
        Address CreateAddress(int userId, AddressListViewModel address);
        void UpdateAddress(int userId, AddressListViewModel newAddress);
        void DeleteAddress(int userId, int addressId);
    }
}

using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.DataLayer.Context;
using System.Data.Entity;
using ContentManagementSystem.DomainClasses;
using EFSecondLevelCache;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFAddressService : IAddressService
    {
        IUnitOfWork _uow;
        readonly IDbSet<Address> _addresses;
        //private readonly Lazy<Professor> _professorService;
        public EFAddressService(IUnitOfWork uow)
        {
            _uow = uow;
            _addresses = _uow.Set<Address>();
        }

        public IEnumerable<AddressListViewModel> GetListAddresses(int userId)
        {
            var addressList = new List<AddressListViewModel>();
            var addresses = _addresses
                .Where(a => a.ProfessorId == userId)
                .OrderByDescending(a => a.Order)
                .ThenBy(a => a.AddressId)
                .Cacheable()
                .ToList();

            foreach (var address in addresses)
            {
                addressList.Add(new AddressListViewModel
                {
                    AddressId = address.AddressId,
                    PostalAddress = address.PostalAddress,
                    PostalCode = address.PostalCode,
                    Tel = address.Tell,
                    Fax = address.Fax,
                    Link = address.Link,
                    Order = address.Order
                });
            }

            return addressList;
        }

        public Address CreateAddress(int userId, AddressListViewModel address)
        {
            var newAddress = _addresses.Add(new Address
            {
                ProfessorId = userId,
                PostalAddress = address.PostalAddress,
                PostalCode = address.PostalCode,
                Tell = address.Tel,
                Fax = address.Fax,
                Link = address.Link,
                Order = address.Order
            });

            return newAddress;
        }

        public void UpdateAddress(int userId, AddressListViewModel newAddress)
        {
            var address = _addresses.Single(a => a.ProfessorId == userId && a.AddressId == newAddress.AddressId);

            address.PostalAddress = newAddress.PostalAddress;
            address.PostalCode = newAddress.PostalCode;
            address.Tell = newAddress.Tel;
            address.Fax = newAddress.Fax;
            address.Link = newAddress.Link;
            address.Order = newAddress.Order;
        }

        public void DeleteAddress(int userId, int addressId)
        {
            var address = _addresses.Single(a => a.ProfessorId == userId && a.AddressId == addressId);
            _addresses.Remove(address);
        }

    }
}

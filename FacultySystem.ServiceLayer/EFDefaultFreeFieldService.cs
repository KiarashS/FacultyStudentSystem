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
    public class EFDefaultFreeFieldService : IDefaultFreeFieldService
    {
        IUnitOfWork _uow;
        readonly IDbSet<DefaultFreeField> _freeFields;
        //private readonly Lazy<Professor> _professorService;
        public EFDefaultFreeFieldService(IUnitOfWork uow)
        {
            _uow = uow;
            _freeFields = _uow.Set<DefaultFreeField>();
        }

        public IEnumerable<DefaultFreeFieldListViewModel> GetListFreeFields()
        {
            var freeFieldList = new List<DefaultFreeFieldListViewModel>();
            var freeFields = _freeFields
                .OrderByDescending(ff => ff.Order)
                .ThenBy(ff => ff.Id)
                .Cacheable()
                .ToList();

            foreach (var freeField in freeFields)
            {
                freeFieldList.Add(new DefaultFreeFieldListViewModel
                {
                    Id = freeField.Id,
                    Name = freeField.Name,
                    Value = freeField.Value,
                    Order = freeField.Order
                });
            }

            return freeFieldList;
        }

        public DefaultFreeField CreateFreeField(DefaultFreeFieldListViewModel freeField)
        {
            var newFreeField = _freeFields.Add(new DefaultFreeField
            {
                Name = freeField.Name,
                Value = freeField.Value,
                Order = freeField.Order
            });

            return newFreeField;
        }

        public void UpdateFreeField(DefaultFreeFieldListViewModel newFreeField)
        {
            var freeField = _freeFields.Single(ff => ff.Id == newFreeField.Id);

            freeField.Name = newFreeField.Name;
            freeField.Value = newFreeField.Value;
            freeField.Order = newFreeField.Order;
        }

        public void DeleteFreeField(int id)
        {
            var freeField = _freeFields.Single(ff => ff.Id == id);
            _freeFields.Remove(freeField);
        }
    }
}

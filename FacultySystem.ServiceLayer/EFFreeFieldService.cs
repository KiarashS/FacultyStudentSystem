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
using System.Activities.Statements;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFFreeFieldService : IFreeFieldService
    {
        IUnitOfWork _uow;
        readonly IDbSet<FreeField> _freeFields;
        //private readonly Lazy<Professor> _professorService;
        public EFFreeFieldService(IUnitOfWork uow)
        {
            _uow = uow;
            _freeFields = _uow.Set<FreeField>();
        }

        public IEnumerable<FreeFieldListViewModel> GetListFreeFields(int userId)
        {
            var freeFieldList = new List<FreeFieldListViewModel>();
            var freeFields = _freeFields
                .Where(ff => ff.ProfessorId == userId)
                .OrderByDescending(ff => ff.Order)
                .ThenBy(ff => ff.Id)
                .Cacheable()
                .ToList();

            foreach (var freeField in freeFields)
            {
                freeFieldList.Add(new FreeFieldListViewModel
                {
                    Id = freeField.Id,
                    Name = freeField.Name,
                    Value = freeField.Value,
                    Order = freeField.Order
                });
            }

            return freeFieldList;
        }

        public FreeField CreateFreeField(int userId, FreeFieldListViewModel freeField)
        {
            var newFreeField = _freeFields.Add(new FreeField
            {
                ProfessorId = userId,
                Name = freeField.Name,
                Value = freeField.Value,
                Order = freeField.Order
            });

            return newFreeField;
        }

        public void UpdateFreeField(int userId, FreeFieldListViewModel newFreeField)
        {
            var freeField = _freeFields.Single(ff => ff.ProfessorId == userId && ff.Id == newFreeField.Id);

            freeField.Name = newFreeField.Name;
            freeField.Value = newFreeField.Value;
            freeField.Order = newFreeField.Order;
        }

        public void DeleteFreeField(int userId, int freeFieldId)
        {
            var freeField = _freeFields.Single(ff => ff.ProfessorId == userId && ff.Id == freeFieldId);
            _freeFields.Remove(freeField);
        }

        public void AddFreeFieldToAll(string name, string value, int? order, IList<int> usersIds)
        {
            var newFields = new List<FreeField>();

            foreach (var userId in usersIds)
            {
                var newField = new FreeField
                {
                    ProfessorId = userId,
                    Name = name,
                    Value = value,
                    Order = order
                };

                newFields.Add(newField);
            }

            _uow.AddThisRange(newFields);
        }

        public void CreateFreeFields(int userId, IEnumerable<DefaultFreeFieldListViewModel> freeFields)
        {
            var newFields = new List<FreeField>();

            foreach (var field in freeFields)
            {
                var newField = new FreeField
                {
                    ProfessorId = userId,
                    Name = field.Name,
                    Value = field.Value,
                    Order = field.Order
                };

                newFields.Add(newField);
            }

            _uow.AddThisRange(newFields);
        }
    }
}

using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IPublicationService
    {
        IEnumerable<PublicationViewModel> GetPublications(int userId);
        Publication CreatePublication(int userId, PublicationViewModel publication);
        void UpdatePublication(int userId, PublicationViewModel newPublication);
        void DeletePublication(int userId, long id);
    }
}

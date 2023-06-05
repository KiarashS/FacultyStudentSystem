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
    public class EFPublicationService : IPublicationService
    {
        IUnitOfWork _uow;
        readonly IDbSet<Publication> _publications;
        public EFPublicationService(IUnitOfWork uow)
        {
            _uow = uow;
            _publications = _uow.Set<Publication>();
        }

        public IEnumerable<PublicationViewModel> GetPublications(int userId)
        {
            var publicationList = new List<PublicationViewModel>();
            var publications = _publications
                .Where(p => p.ProfessorId == userId)
                .OrderByDescending(p => p.Order)
                .ThenBy(p => p.Id)
                .Cacheable()
                .ToList();

            foreach (var publication in publications)
            {
                publicationList.Add(new PublicationViewModel
                {
                    Id = publication.Id,
                    Title = publication.Title,
                    Publisher = publication.Publisher,
                    Time = publication.Time,
                    Description = publication.Description,
                    Link = publication.Link,
                    Order = publication.Order
                });
            }

            return publicationList;
        }

        public Publication CreatePublication(int userId, PublicationViewModel publication)
        {
            var newPublication = _publications.Add(new Publication
            {
                ProfessorId = userId,
                Title = publication.Title,
                Publisher = publication.Publisher,
                Time = publication.Time,
                Description = publication.Description,
                Link = publication.Link,
                Order = publication.Order
            });

            return newPublication;
        }

        public void UpdatePublication(int userId, PublicationViewModel newPublication)
        {
            var publiaction = _publications.Single(p => p.ProfessorId == userId && p.Id == newPublication.Id);

            publiaction.Title = newPublication.Title;
            publiaction.Publisher = newPublication.Publisher;
            publiaction.Time = newPublication.Time;
            publiaction.Description = newPublication.Description;
            publiaction.Link = newPublication.Link;
            publiaction.Order = newPublication.Order;
        }

        public void DeletePublication(int userId, long id)
        {
            var publication = _publications.Single(p => p.ProfessorId == userId && p.Id == id);
            _publications.Remove(publication);
        }
    }
}

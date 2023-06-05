using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentManagementSystem.Models.ViewModels;
using EFSecondLevelCache;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFGalleryItemService: IGalleryItemService
    {
        IUnitOfWork _uow;
        readonly IDbSet<GalleryItem> _galleryItems;
        //private readonly Lazy<Professor> _professorService;
        public EFGalleryItemService(IUnitOfWork uow)
        {
            _uow = uow;
            _galleryItems = _uow.Set<GalleryItem>();
        }

        public IEnumerable<GalleryItemViewModel> GetGalleryItems(int userId, long galleryId)
        {
            var galleryItemList = new List<GalleryItemViewModel>();
            var galleryItems = _galleryItems
                .Where(g => g.ProfessorId == userId && g.GalleryId == galleryId)
                .OrderByDescending(g => g.Order)
                .ThenByDescending(g => g.Id)
                .Cacheable()
                .ToList();

            foreach (var galleryItem in galleryItems)
            {
                galleryItemList.Add(new GalleryItemViewModel
                {
                    UserId = galleryItem.ProfessorId,
                    GalleryId = galleryItem.GalleryId,
                    Id = galleryItem.Id,
                    Title = galleryItem.Title,
                    Description = galleryItem.Description,
                    CreateDate = galleryItem.CreateDate,
                    MediaFilename= galleryItem.MediaFilename,
                    MediaType = galleryItem.MediaType,
                    Link = galleryItem.Link,
                    Order = galleryItem.Order
                });
            }

            return galleryItemList;
        }

        public GalleryItem CreateGalleryItem(int userId, GalleryItemViewModel galleryItem)
        {
            var newGalleryItem = _galleryItems.Add(new GalleryItem
            {
                ProfessorId = userId,
                GalleryId = galleryItem.GalleryId,
                Title = galleryItem.Title,
                Description = galleryItem.Description,
                MediaFilename = galleryItem.MediaFilename,
                MediaType = galleryItem.MediaType,
                Link = galleryItem.Link,
                Order = galleryItem.Order
            });

            return newGalleryItem;
        }

        public void UpdateGalleryItem(int userId, GalleryItemViewModel newGalleryItem)
        {
            var galleryItem = _galleryItems.Single(g => g.ProfessorId == userId && g.Id == newGalleryItem.Id);

            galleryItem.Title = newGalleryItem.Title;
            galleryItem.Description = newGalleryItem.Description;
            galleryItem.MediaFilename = newGalleryItem.MediaFilename;
            galleryItem.MediaType = newGalleryItem.MediaType;
            galleryItem.Link = newGalleryItem.Link;
            galleryItem.Order = newGalleryItem.Order;
        }

        public void DeleteGalleryItem(int userId, long id)
        {
            var galleryItem = _galleryItems.Single(l => l.ProfessorId == userId && l.Id == id);
            _galleryItems.Remove(galleryItem);
        }

        public Tuple<string> GetFilename(int userId, long id)
        {
            var filenames = _galleryItems
                .Where(g => g.ProfessorId == userId && g.Id == id)
                .Select(g => new { g.MediaFilename })
                .SingleOrDefault();
            
            return new Tuple<string>(filenames.MediaFilename);
        }
    }
}

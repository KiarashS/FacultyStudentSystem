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
using Z.EntityFramework.Plus;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFGalleryService: IGalleryService
    {
        IUnitOfWork _uow;
        readonly IDbSet<Gallery> _galleries;
        //private readonly Lazy<Professor> _professorService;
        public EFGalleryService(IUnitOfWork uow)
        {
            _uow = uow;
            _galleries = _uow.Set<Gallery>();
        }

        public IEnumerable<GalleryViewModel> GetGalleries(int userId)
        {
            var galleryList = new List<GalleryViewModel>();
            var galleries = _galleries
                .Where(l => l.ProfessorId == userId)
                .OrderByDescending(l => l.Order)
                .ThenBy(l => l.Id)
                .Cacheable()
                .ToList();

            foreach (var gallery in galleries)
            {
                galleryList.Add(new GalleryViewModel
                {
                    Id = gallery.Id,
                    Title = gallery.Title,
                    CreateDate = gallery.CreateDate,
                    Description = gallery.Description,
                    IsActive = gallery.IsActive,
                    Link = gallery.Link,
                    Order = gallery.Order
                });
            }

            return galleryList;
        }

        public Gallery CreateGallery(int userId, GalleryViewModel gallery)
        {
            var newGallery = _galleries.Add(new Gallery
            {
                ProfessorId = userId,
                Title = gallery.Title,
                Description = gallery.Description,
                IsActive = gallery.IsActive,
                Link = gallery.Link,
                Order = gallery.Order
            });

            return newGallery;
        }

        public void UpdateGallery(int userId, GalleryViewModel newGallery)
        {
            var gallery = _galleries.Single(l => l.ProfessorId == userId && l.Id == newGallery.Id);

            gallery.Title = newGallery.Title;
            gallery.Description = newGallery.Description;
            gallery.IsActive = newGallery.IsActive;
            gallery.Link = newGallery.Link;
            gallery.Order = newGallery.Order;
        }

        public void DeleteGallery(int userId, long id)
        {
            var gallery = _galleries.Single(l => l.ProfessorId == userId && l.Id == id);
            _galleries.Remove(gallery);
        }

        public GalleryIndexViewModel GalleryIndex(int userId, long galleryId)
        {
            var gallery = _galleries
                .Where(l => l.ProfessorId == userId && l.Id == galleryId)
                .Select(l => new
                {
                    l.Title,
                    l.CreateDate,
                    l.Description,
                    l.IsActive,
                    l.Link,
                    l.Id,
                    HasGalleryItem = l.GalleryItems.Any()
                })
                .Cacheable()
                .SingleOrDefault();

            if (gallery == null)
            {
                return null;
            }

            return new GalleryIndexViewModel
            {
                GalleryId = gallery.Id,
                Title = gallery.Title,
                CreateDate = gallery.CreateDate,
                Description = gallery.Description,
                IsActive = gallery.IsActive,
                Link = gallery.Link,
                HasGalleryItem = gallery.HasGalleryItem
            };
        }

        public void DeleteAllGallery(int userId)
        {
            _galleries.Where(g => g.ProfessorId == userId).Delete();
        }
    }
}

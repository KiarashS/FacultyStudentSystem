using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IGalleryItemService
    {
        IEnumerable<GalleryItemViewModel> GetGalleryItems(int userId, long galleryId);
        GalleryItem CreateGalleryItem(int userId, GalleryItemViewModel galleryItem);
        void UpdateGalleryItem(int userId, GalleryItemViewModel newGalleryItem);
        void DeleteGalleryItem(int userId, long id);
        Tuple<string> GetFilename(int userId, long id);
    }
}

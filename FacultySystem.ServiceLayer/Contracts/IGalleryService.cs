using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.ServiceLayer.Contracts
{
    public interface IGalleryService
    {
        IEnumerable<GalleryViewModel> GetGalleries(int userId);
        Gallery CreateGallery(int userId, GalleryViewModel gallery);
        void UpdateGallery(int userId, GalleryViewModel newGallery);
        void DeleteGallery(int userId, long id);
        GalleryIndexViewModel GalleryIndex(int userId, long galleryId);
        void DeleteAllGallery(int userId);
    }
}

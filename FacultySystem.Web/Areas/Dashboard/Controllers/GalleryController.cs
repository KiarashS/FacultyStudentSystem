using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using ContentManagementSystem.Web.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Web.Areas.Dashboard.Controllers
{
    [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
    public partial class GalleryController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IProfessorService _professorService;
        private readonly IGalleryService _galleryService;
        private readonly IGalleryItemService _galleryItemService;

        public GalleryController(IUnitOfWork uow, IGalleryService galleryService, IGalleryItemService galleryItemService,
            IProfessorService professorService)
        {
            _uow = uow;
            _galleryService = galleryService;
            _galleryItemService = galleryItemService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var galleries = _galleryService.GetGalleries(CurrentUserId);
            return Json(new { Result = "OK", Records = galleries });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(GalleryViewModel gallery)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newGallery = _galleryService.CreateGallery(CurrentUserId, gallery);
            _uow.SaveAllChanges();

            gallery.Id = newGallery.Id;

            return Json(new { Result = "OK", Record = gallery });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(GalleryViewModel gallery)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _galleryService.UpdateGallery(CurrentUserId, gallery);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _galleryService.DeleteGallery(CurrentUserId, id);
            _uow.SaveAllChanges();

            var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\GalleryFiles\" + id.ToString();
            if (System.IO.Directory.Exists(path))
            {
                //var directoryInfo = new System.IO.DirectoryInfo(path);
                //directoryInfo.DeleteFilesWhere(System.IO.SearchOption.AllDirectories, f => f.Name.StartsWith("__" + id.ToString() + "__"));
                System.IO.Directory.Delete(path, true);
            }

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ListGalleryItems(long galleryId)
        {
            var galleryItems = _galleryItemService.GetGalleryItems(CurrentUserId, galleryId);
            var sanitizedFiles = galleryItems.Select(s => new { ItemId = s.Id, Text = s.FileText }).ToList();
            galleryItems.ForEach(s => {
                s.MediaFilename = null;
                s.UserId = 0;
                s.FileText = sanitizedFiles.Where(sc => sc.ItemId == s.Id).Select(sc => sc.Text).Single();
            });

            return Json(new { Result = "OK", Records = galleryItems });
        }

        [HttpPost]
        //[AjaxOnly]
        [AllowUploadSpecialFilesOnly(".jpg,.jpeg,.png,.rar,.zip,.pdf,.doc,.docx,.txt,.rtf,.ppt,.pptx,.xls,.xlsx,.csv,.mp3,.ogg,.wav,.mp4,.webm,.avi,.wmv,.mov,.flv")]
        [Demo(isJtableCaller: true, isAjaxCaller: true)]
        public virtual ActionResult CreateGalleryItem(GalleryItemViewModel galleryItem, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                if (ConvertBytesToMegabytes(file.ContentLength) > 100.0)
                {
                    return Json(new { Result = "ERROR", MESSAGE = "حجم فایل بیشتر از 100 مگابایت می باشد." });
                }

                var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\GalleryFiles\" + galleryItem.GalleryId.ToString() + @"\";
                var fileExtension = System.IO.Path.GetExtension(file.FileName);
                var filename = "__" + galleryItem.GalleryId.ToString() + "__" + System.IO.Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "-").Replace("_", "-").Replace(";#;", "-") + "__" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
                galleryItem.MediaFilename = filename + fileExtension;

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                file.SaveAs(path + filename + fileExtension);
            }
            else
            {
                return Json(new { Result = "ERROR", MESSAGE = "لطفاً فایل را انتخاب نمائید." });
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newGalleryItem = _galleryItemService.CreateGalleryItem(CurrentUserId, galleryItem);
            _uow.SaveAllChanges();

            galleryItem.Id = newGalleryItem.Id;

            return Json(new { Result = "OK", Record = galleryItem });
        }

        [HttpPost]
        //[AjaxOnly]
        [AllowUploadSpecialFilesOnly(".jpg,.jpeg,.png,.rar,.zip,.pdf,.doc,.docx,.txt,.rtf,.ppt,.pptx,.xls,.xlsx,.csv,.mp3,.ogg,.wav,.mp4,.webm,.avi,.wmv,.mov,.flv")]
        [Demo(isJtableCaller: true, isAjaxCaller: true)]
        public virtual ActionResult UpdateGalleryItem(GalleryItemViewModel galleryItem, HttpPostedFileBase file)
        {
            var currentFilename = _galleryItemService.GetFilename(CurrentUserId, galleryItem.Id);
            var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\GalleryFiles\" + galleryItem.GalleryId.ToString() + @"\";

            if (file != null && file.ContentLength > 0)
            {
                if (ConvertBytesToMegabytes(file.ContentLength) > 100.0)
                {
                    return Json(new { Result = "ERROR", MESSAGE = "حجم فایل بیشتر از 100 مگابایت می باشد." });
                }

                var fileExtension = System.IO.Path.GetExtension(file.FileName);
                var filename = "__" + galleryItem.GalleryId.ToString() + "__" + System.IO.Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "-").Replace("_", "-").Replace(";#;", "-") + "__" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
                galleryItem.MediaFilename = filename + fileExtension;

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                if (!string.IsNullOrEmpty(currentFilename.Item1) && System.IO.File.Exists(path + currentFilename.Item1))
                {
                    System.IO.File.Delete(path + currentFilename.Item1);
                }

                file.SaveAs(path + filename + fileExtension);
            }
            //else if (galleryItem.DeleteFile)
            //{
            //    if (!string.IsNullOrEmpty(currentFilename.Item1) && System.IO.File.Exists(path + currentFilename.Item1))
            //    {
            //        System.IO.File.Delete(path + currentFilename.Item1);
            //    }

            //    galleryItem.MediaFilename = null;
            //}
            else
            {
                galleryItem.MediaFilename = currentFilename.Item1;
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _galleryItemService.UpdateGalleryItem(CurrentUserId, galleryItem);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult DeleteGalleryItem(long galleryId, long id)
        {
            var currentFilename = _galleryItemService.GetFilename(CurrentUserId, id);
            var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\GalleryFiles\" + galleryId.ToString() + @"\";

            if (!string.IsNullOrEmpty(currentFilename.Item1) && System.IO.File.Exists(path + currentFilename.Item1))
            {
                System.IO.File.Delete(path + currentFilename.Item1);
            }
            
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _galleryItemService.DeleteGalleryItem(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        static double ConvertKilobytesToMegabytes(long kilobytes)
        {
            return kilobytes / 1024f;
        }

        private Image ResizeImage(Stream image, int width)
        {
            using (Image fromStream = Image.FromStream(image))
            {
                // calculate height based on the width parameter
                int newHeight = (int)(fromStream.Height / ((double)fromStream.Width / width));

                using (Bitmap resizedImg = new Bitmap(fromStream, width, newHeight))
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        resizedImg.Save(stream, fromStream.RawFormat);
                        return Image.FromStream(stream);
                    }
                }
            }
        }
    }
}

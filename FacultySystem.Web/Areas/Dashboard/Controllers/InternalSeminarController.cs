using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using ContentManagementSystem.Web.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Web.Areas.Dashboard.Controllers
{
    [SiteAuthorize(Roles = ConstantsUtil.ProfessorRole)]
    public partial class InternalSeminarController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IInternalSeminarService _seminarService;
        private readonly IProfessorService _professorService;

        public InternalSeminarController(IUnitOfWork uow, IInternalSeminarService seminarService, IProfessorService professorService)
        {
            _uow = uow;
            _seminarService = seminarService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List(string filterTitle, int jtStartIndex = 0, int jtPageSize = 20)
        {
            var seminars = _seminarService.GetListSeminars(CurrentUserId, filterTitle, jtStartIndex, jtPageSize);
            var sanitizedSeminarss = seminars.Select(s => new { FileId = s.Id, Text = s.FileText }).ToList();
            seminars.ForEach(s => {
                s.Filename = null;
                s.UserId = 0;
                s.FileText = sanitizedSeminarss.Where(sc => sc.FileId == s.Id).Select(sc => sc.Text).Single();
            });
            var totalRecordsCount = _seminarService.TotalCount(CurrentUserId, filterTitle);

            return Json(new { Result = "OK", TotalRecordCount = totalRecordsCount, Records = seminars });
        }

        [HttpPost]
        //[AjaxOnly]
        [AllowUploadSpecialFilesOnly(".jpg,.jpeg,.png,.rar,.zip,.pdf,.doc,.docx,.txt,.rtf,.ppt,.pptx,.xls,.xlsx,.csv")]
        [Demo(isJtableCaller: true, isAjaxCaller: true)]
        public virtual ActionResult Create(InternalSeminarRecordViewModel seminar, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                if (StaticUtils.ConvertBytesToMegabytes(file.ContentLength) > 100.0)
                {
                    return Json(new { Result = "ERROR", MESSAGE = "حجم فایل بیشتر از 100 مگابایت می باشد." });
                }

                var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\ResearchFiles\";
                var fileExtension = System.IO.Path.GetExtension(file.FileName);
                var filename = "__" + CurrentUserId.ToString() + "__" + System.IO.Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "-").Replace("_", "-").Replace(";#;", "-") + "__" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
                seminar.Filename = filename + fileExtension;

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                file.SaveAs(path + filename + fileExtension);
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newSeminar = _seminarService.CreateSeminar(CurrentUserId, seminar);
            _uow.SaveAllChanges();

            seminar.Id = newSeminar.Id;

            return Json(new { Result = "OK", Record = seminar });
        }

        [HttpPost]
        //[AjaxOnly]
        [AllowUploadSpecialFilesOnly(".jpg,.jpeg,.png,.rar,.zip,.pdf,.doc,.docx,.txt,.rtf,.ppt,.pptx,.xls,.xlsx,.csv")]
        [Demo(isJtableCaller: true, isAjaxCaller: true)]
        public virtual ActionResult Update(InternalSeminarRecordViewModel seminar, HttpPostedFileBase file, bool deleteFile = false)
        {
            var currentFilename = _seminarService.GetFilename(CurrentUserId, seminar.Id);
            var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\ResearchFiles\";

            if (file != null && file.ContentLength > 0)
            {
                if (StaticUtils.ConvertBytesToMegabytes(file.ContentLength) > 100.0)
                {
                    return Json(new { Result = "ERROR", MESSAGE = "حجم فایل بیشتر از 100 مگابایت می باشد." });
                }

                var fileExtension = System.IO.Path.GetExtension(file.FileName);
                var filename = "__" + CurrentUserId.ToString() + "__" + System.IO.Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "-").Replace("_", "-").Replace(";#;", "-") + "__" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
                seminar.Filename = filename + fileExtension;

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                if (!string.IsNullOrEmpty(currentFilename) && System.IO.File.Exists(path + currentFilename))
                {
                    System.IO.File.Delete(path + currentFilename);
                }

                file.SaveAs(path + filename + fileExtension);
            }
            else if (deleteFile)
            {
                if (!string.IsNullOrEmpty(currentFilename) && System.IO.File.Exists(path + currentFilename))
                {
                    System.IO.File.Delete(path + currentFilename);
                }

                seminar.Filename = null;
            }
            else
            {
                seminar.Filename = currentFilename;
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _seminarService.UpdateSeminar(CurrentUserId, seminar);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            var currentFilename = _seminarService.GetFilename(CurrentUserId, id);
            var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\ResearchFiles\";

            if (!string.IsNullOrEmpty(currentFilename) && System.IO.File.Exists(path + currentFilename))
            {
                System.IO.File.Delete(path + currentFilename);
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _seminarService.DeleteSeminar(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }
    }
}

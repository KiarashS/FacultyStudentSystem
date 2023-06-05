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
    public partial class LessonController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly ILessonService _lessonService;
        private readonly ILessonClassInfoService _lessonClassService;
        private readonly IPracticeClassInfoService _practiceClassService;
        private readonly ILessonImportantDateService _lessonDateService;
        private readonly ILessonNewsService _lessonNewsService;
        private readonly ILessonFileService _lessonFileService;
        private readonly ILessonPracticeService _lessonPracticeService;
        private readonly ILessonScoreService _lessonScoreService;
        private readonly IProfessorService _professorService;

        public LessonController(IUnitOfWork uow, ILessonService lessonService, IProfessorService professorService, 
            ILessonClassInfoService lessonClassService, IPracticeClassInfoService practiceClassService,
            ILessonImportantDateService lessonDateService, ILessonNewsService lessonNewsService,
            ILessonFileService lessonFileService, ILessonPracticeService lessonPracticeService,
            ILessonScoreService lessonScoreService)
        {
            _uow = uow;
            _lessonService = lessonService;
            _lessonClassService = lessonClassService;
            _practiceClassService = practiceClassService;
            _lessonDateService = lessonDateService;
            _lessonNewsService = lessonNewsService;
            _lessonFileService = lessonFileService;
            _lessonPracticeService = lessonPracticeService;
            _lessonScoreService = lessonScoreService;
            _professorService = professorService;
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult List()
        {
            var lessons = _lessonService.GetLessons(CurrentUserId);
            return Json(new { Result = "OK", Records = lessons });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Create(LessonViewModel lesson)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newLesson = _lessonService.CreateLesson(CurrentUserId, lesson);
            _uow.SaveAllChanges();

            lesson.Id = newLesson.Id;

            return Json(new { Result = "OK", Record = lesson });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Update(LessonViewModel lesson)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _lessonService.UpdateLesson(CurrentUserId, lesson);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult Delete(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _lessonService.DeleteLesson(CurrentUserId, id);
            _uow.SaveAllChanges();

            var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\LessonFiles\";
            if (System.IO.Directory.Exists(path))
            {
                var directoryInfo = new System.IO.DirectoryInfo(path);
                directoryInfo.DeleteFilesWhere(System.IO.SearchOption.AllDirectories, f => f.Name.StartsWith("__" + id.ToString() + "__"));
            }

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ListClass(long lessonId)
        {
            var lessonClasses = _lessonClassService.GetLessonClasses(CurrentUserId, lessonId);
            return Json(new { Result = "OK", Records = lessonClasses });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult CreateClass(LessonClassInfoViewModel lessonClass)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newLesson = _lessonClassService.CreateLessonClass(CurrentUserId, lessonClass);
            _uow.SaveAllChanges();

            lessonClass.Id = newLesson.Id;

            return Json(new { Result = "OK", Record = lessonClass });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult UpdateClass(LessonClassInfoViewModel lessonClass)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _lessonClassService.UpdateLessonClass(CurrentUserId, lessonClass);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult DeleteClass(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _lessonClassService.DeleteLessonClass(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ListPracticeClass(long lessonId)
        {
            var practiceClasses = _practiceClassService.GetPracticeClasses(CurrentUserId, lessonId);
            return Json(new { Result = "OK", Records = practiceClasses });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult CreatePracticeClass(PracticeClassInfoViewModel practiceClass)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newPracticeClass = _practiceClassService.CreatePracticeClass(CurrentUserId, practiceClass);
            _uow.SaveAllChanges();

            practiceClass.Id = newPracticeClass.Id;

            return Json(new { Result = "OK", Record = practiceClass });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult UpdatePracticeClass(PracticeClassInfoViewModel practiceClass)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _practiceClassService.UpdatePracticeClass(CurrentUserId, practiceClass);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult DeletePracticeClass(long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _practiceClassService.DeletePracticeClass(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ListFile(long lessonId)
        {
            var files = _lessonFileService.GetLessonFiles(CurrentUserId, lessonId);
            var sanitizedFiles = files.Select(s => new { FileId = s.Id, Text = s.FileText, TextCover = s.CoverText }).ToList();
            files.ForEach(s => {
                s.Filename = null;
                s.UserId = 0;
                s.FileText = sanitizedFiles.Where(sc => sc.FileId == s.Id).Select(sc => sc.Text).Single();
                s.CoverText = sanitizedFiles.Where(sc => sc.FileId == s.Id).Select(sc => sc.TextCover).Single();
            });

            return Json(new { Result = "OK", Records = files });
        }

        [HttpPost]
        //[AjaxOnly]
        [AllowUploadSpecialFilesOnly(".jpg,.jpeg,.png,.rar,.zip,.pdf,.doc,.docx,.txt,.rtf,.ppt,.pptx,.xls,.xlsx,.csv,.mp3,.ogg,.wav,.mp4,.webm,.avi,.wmv,.mov,.flv")]
        [Demo(isJtableCaller: true, isAjaxCaller: true)]
        public virtual ActionResult CreateFile(LessonFilesViewModel lessonFile, HttpPostedFileBase file, HttpPostedFileBase coverFile)
        {
            if (coverFile != null && coverFile.ContentLength > 0)
            {
                var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\LessonFiles\Files\";
                var fileExtension = System.IO.Path.GetExtension(coverFile.FileName);
                var filename = "__" + lessonFile.LessonId.ToString() + "__" + System.IO.Path.GetFileNameWithoutExtension(coverFile.FileName).Replace(" ", "-").Replace("_", "-").Replace(";#;", "-") + "__" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
                lessonFile.CoverFilename = filename + fileExtension;

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                var cover = ResizeImage(coverFile.InputStream, 150);
                var tempImage = new Bitmap(cover);
                tempImage.Save(path + filename + fileExtension);
            }

            if (file != null && file.ContentLength > 0)
            {
                if (StaticUtils.ConvertBytesToMegabytes(file.ContentLength) > 100.0)
                {
                    return Json(new { Result = "ERROR", MESSAGE = "حجم فایل بیشتر از 100 مگابایت می باشد." });
                }

                var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\LessonFiles\Files\";
                var fileExtension = System.IO.Path.GetExtension(file.FileName);
                var filename = "__" + lessonFile.LessonId.ToString() + "__" + System.IO.Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "-").Replace("_", "-").Replace(";#;", "-") + "__" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
                lessonFile.Filename = filename + fileExtension;

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                file.SaveAs(path + filename + fileExtension);
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newFile = _lessonFileService.CreateLessonFile(CurrentUserId, lessonFile);
            _uow.SaveAllChanges();

            lessonFile.Id = newFile.Id;

            return Json(new { Result = "OK", Record = lessonFile });
        }

        [HttpPost]
        //[AjaxOnly]
        [AllowUploadSpecialFilesOnly(".jpg,.jpeg,.png,.rar,.zip,.pdf,.doc,.docx,.txt,.rtf,.ppt,.pptx,.xls,.xlsx,.csv,.mp3,.ogg,.wav,.mp4,.webm,.avi,.wmv,.mov,.flv")]
        [Demo(isJtableCaller: true, isAjaxCaller: true)]
        public virtual ActionResult UpdateFile(LessonFilesViewModel lessonFile, HttpPostedFileBase file, HttpPostedFileBase coverFile)
        {
            var currentFilename = _lessonFileService.GetFilename(CurrentUserId, lessonFile.Id);
            var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\LessonFiles\Files\";

            if (coverFile != null && coverFile.ContentLength > 0)
            {
                var fileExtension = System.IO.Path.GetExtension(coverFile.FileName);
                var filename = "__" + lessonFile.LessonId.ToString() + "__" + System.IO.Path.GetFileNameWithoutExtension(coverFile.FileName).Replace(" ", "-").Replace("_", "-").Replace(";#;", "-") + "__" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
                lessonFile.CoverFilename = filename + fileExtension;

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                if (!string.IsNullOrEmpty(currentFilename.Item2) && System.IO.File.Exists(path + currentFilename.Item2))
                {
                    System.IO.File.Delete(path + currentFilename.Item2);
                }

                var cover = ResizeImage(coverFile.InputStream, 150);
                var tempImage = new Bitmap(cover);
                tempImage.Save(path + filename + fileExtension);
            }
            else if (lessonFile.DeleteCover)
            {
                if (!string.IsNullOrEmpty(currentFilename.Item2) && System.IO.File.Exists(path + currentFilename.Item2))
                {
                    System.IO.File.Delete(path + currentFilename.Item2);
                }

                lessonFile.CoverFilename = null;
            }
            else
            {
                lessonFile.CoverFilename = currentFilename.Item2;
            }

            if (file != null && file.ContentLength > 0)
            {
                if (StaticUtils.ConvertBytesToMegabytes(file.ContentLength) > 100.0)
                {
                    return Json(new { Result = "ERROR", MESSAGE = "حجم فایل بیشتر از 100 مگابایت می باشد." });
                }

                var fileExtension = System.IO.Path.GetExtension(file.FileName);
                var filename = "__" + lessonFile.LessonId.ToString() + "__" + System.IO.Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "-").Replace("_", "-").Replace(";#;", "-") + "__" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
                lessonFile.Filename = filename + fileExtension;

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
            else if (lessonFile.DeleteFile)
            {
                if (!string.IsNullOrEmpty(currentFilename.Item1) && System.IO.File.Exists(path + currentFilename.Item1))
                {
                    System.IO.File.Delete(path + currentFilename.Item1);
                }

                lessonFile.Filename = null;
            }
            else
            {
                lessonFile.Filename = currentFilename.Item1;
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _lessonFileService.UpdateLessonFile(CurrentUserId, lessonFile);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult DeleteFile(long lessonId, long id)
        {
            var currentFilename = _lessonFileService.GetFilename(CurrentUserId, id);
            var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\LessonFiles\Files\";

            if (!string.IsNullOrEmpty(currentFilename.Item1) && System.IO.File.Exists(path + currentFilename.Item1))
            {
                System.IO.File.Delete(path + currentFilename.Item1);
            }

            if (!string.IsNullOrEmpty(currentFilename.Item2) && System.IO.File.Exists(path + currentFilename.Item2))
            {
                System.IO.File.Delete(path + currentFilename.Item2);
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _lessonFileService.DeleteLessonFile(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ListNews(long lessonId)
        {
            var news = _lessonNewsService.GetLessonNews(CurrentUserId, lessonId);
            return Json(new { Result = "OK", Records = news });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult CreateNews(long lessonId, LessonNewsViewModel news)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newNews = _lessonNewsService.CreateLessonNews(CurrentUserId, news);
            _uow.SaveAllChanges();

            news.Id = newNews.Id;

            return Json(new { Result = "OK", Record = news });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult UpdateNews(long lessonId, LessonNewsViewModel news)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _lessonNewsService.UpdateLessonNews(CurrentUserId, news);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult DeleteNews(long lessonId, long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _lessonNewsService.DeleteLessonNews(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ListDate(long lessonId)
        {
            var dates = _lessonDateService.GetImportantDates(CurrentUserId, lessonId);
            return Json(new { Result = "OK", Records = dates });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult CreateDate(long lessonId, LessonImportantDateViewModel importantDate)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newDate = _lessonDateService.CreateImportantDate(CurrentUserId, importantDate);
            _uow.SaveAllChanges();

            importantDate.Id = newDate.Id;

            return Json(new { Result = "OK", Record = importantDate });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult UpdateDate(long lessonId, LessonImportantDateViewModel importantDate)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _lessonDateService.UpdateImportantDate(CurrentUserId, importantDate);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult DeleteDate(long lessonId, long id)
        {
            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _lessonDateService.DeleteImportantDate(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ListScore(long lessonId)
        {
            var scores = _lessonScoreService.GetLessonScores(CurrentUserId, lessonId);
            var sanitizedScores = scores.Select(s => new { ScoreId = s.Id, Text = s.FileText }).ToList();
            scores.ForEach(s => { s.Filename = null; s.UserId = 0; s.FileText = sanitizedScores.Where(sc => sc.ScoreId == s.Id).Select(sc => sc.Text).Single(); });

            return Json(new { Result = "OK", Records = scores });
        }

        [HttpPost]
        //[AjaxOnly]
        [AllowUploadSpecialFilesOnly(".jpg,.jpeg,.png,.rar,.zip,.pdf,.doc,.docx,.txt,.rtf,.ppt,.pptx,.xls,.xlsx,.csv,.mp3,.ogg,.wav,.mp4,.webm,.avi,.wmv,.mov,.flv")]
        [Demo(isJtableCaller: true, isAjaxCaller: true)]
        public virtual ActionResult CreateScore(LessonScoresViewModel score, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                if (StaticUtils.ConvertBytesToMegabytes(file.ContentLength) > 100.0)
                {
                    return Json(new { Result = "ERROR", MESSAGE = "حجم فایل بیشتر از 100 مگابایت می باشد." });
                }

                var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\LessonFiles\Scores\";
                var fileExtension = System.IO.Path.GetExtension(file.FileName);
                var filename = "__" + score.LessonId.ToString() + "__" + System.IO.Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "-").Replace("_", "-").Replace(";#;", "-") + "__" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
                score.Filename = filename + fileExtension;

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                file.SaveAs(path + filename + fileExtension);
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newScore = _lessonScoreService.CreateLessonScore(CurrentUserId, score);
            _uow.SaveAllChanges();

            score.Id = newScore.Id;

            return Json(new { Result = "OK", Record = score });
        }

        [HttpPost]
        //[AjaxOnly]
        [AllowUploadSpecialFilesOnly(".jpg,.jpeg,.png,.rar,.zip,.pdf,.doc,.docx,.txt,.rtf,.ppt,.pptx,.xls,.xlsx,.csv,.mp3,.ogg,.wav,.mp4,.webm,.avi,.wmv,.mov,.flv")]
        [Demo(isJtableCaller: true, isAjaxCaller: true)]
        public virtual ActionResult UpdateScore(LessonScoresViewModel score, HttpPostedFileBase file)
        {
            var currentFilename = _lessonScoreService.GetFilename(CurrentUserId, score.Id);
            var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\LessonFiles\Scores\";

            if (file != null && file.ContentLength > 0)
            {
                if (StaticUtils.ConvertBytesToMegabytes(file.ContentLength) > 100.0)
                {
                    return Json(new { Result = "ERROR", MESSAGE = "حجم فایل بیشتر از 100 مگابایت می باشد." });
                }

                var fileExtension = System.IO.Path.GetExtension(file.FileName);
                var filename = "__" + score.LessonId.ToString() + "__" + System.IO.Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "-").Replace("_", "-").Replace(";#;", "-") + "__" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
                score.Filename = filename + fileExtension;

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
            else if (score.DeleteFile)
            {
                if (!string.IsNullOrEmpty(currentFilename) && System.IO.File.Exists(path + currentFilename))
                {
                    System.IO.File.Delete(path + currentFilename);
                }

                score.Filename = null;
            }
            else
            {
                score.Filename = currentFilename;
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _lessonScoreService.UpdateLessonScore(CurrentUserId, score);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult DeleteScore(long lessonId, long id)
        {
            var currentFilename = _lessonScoreService.GetFilename(CurrentUserId, id);
            var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\LessonFiles\Scores\";

            if (!string.IsNullOrEmpty(currentFilename) && System.IO.File.Exists(path + currentFilename))
            {
                System.IO.File.Delete(path + currentFilename);
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _lessonScoreService.DeleteLessonScore(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        public virtual ActionResult ListPractice(long lessonId)
        {
            var practices = _lessonPracticeService.GetLessonPractices(CurrentUserId, lessonId);
            var sanitizedPractices = practices.Select(s => new { PracticeId = s.Id, Text = s.FileText }).ToList();
            practices.ForEach(s => { s.Filename = null; s.UserId = 0; s.FileText = sanitizedPractices.Where(sc => sc.PracticeId == s.Id).Select(sc => sc.Text).Single(); });

            return Json(new { Result = "OK", Records = practices });
        }

        [HttpPost]
        //[AjaxOnly]
        [AllowUploadSpecialFilesOnly(".jpg,.jpeg,.png,.rar,.zip,.pdf,.doc,.docx,.txt,.rtf,.ppt,.pptx,.xls,.xlsx,.csv,.mp3,.ogg,.wav,.mp4,.webm,.avi,.wmv,.mov,.flv")]
        [Demo(isJtableCaller: true, isAjaxCaller: true)]
        public virtual ActionResult CreatePractice(LessonPracticesViewModel practice, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                if (StaticUtils.ConvertBytesToMegabytes(file.ContentLength) > 100.0)
                {
                    return Json(new { Result = "ERROR", MESSAGE = "حجم فایل بیشتر از 100 مگابایت می باشد." });
                }

                var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\LessonFiles\Practices\";
                var fileExtension = System.IO.Path.GetExtension(file.FileName);
                var filename = "__" + practice.LessonId.ToString() + "__" + System.IO.Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "-").Replace("_", "-").Replace(";#;", "-") + "__" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
                practice.Filename = filename + fileExtension;

                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                file.SaveAs(path + filename + fileExtension);
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            var newPractice = _lessonPracticeService.CreateLessonPractice(CurrentUserId, practice);
            _uow.SaveAllChanges();

            practice.Id = newPractice.Id;

            return Json(new { Result = "OK", Record = practice });
        }

        [HttpPost]
        //[AjaxOnly]
        [AllowUploadSpecialFilesOnly(".jpg,.jpeg,.png,.rar,.zip,.pdf,.doc,.docx,.txt,.rtf,.ppt,.pptx,.xls,.xlsx,.csv,.mp3,.ogg,.wav,.mp4,.webm,.avi,.wmv,.mov,.flv")]
        [Demo(isJtableCaller: true, isAjaxCaller: true)]
        public virtual ActionResult UpdatePractice(LessonPracticesViewModel practice, HttpPostedFileBase file)
        {
            var currentFilename = _lessonPracticeService.GetFilename(CurrentUserId, practice.Id);
            var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\LessonFiles\Practices\";

            if (file != null && file.ContentLength > 0)
            {
                if (StaticUtils.ConvertBytesToMegabytes(file.ContentLength) > 100.0)
                {
                    return Json(new { Result = "ERROR", MESSAGE = "حجم فایل بیشتر از 100 مگابایت می باشد." });
                }

                var fileExtension = System.IO.Path.GetExtension(file.FileName);
                var filename = "__" + practice.LessonId.ToString() + "__" + System.IO.Path.GetFileNameWithoutExtension(file.FileName).Replace(" ", "-").Replace("_", "-").Replace(";#;", "-") + "__" + (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
                practice.Filename = filename + fileExtension;

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
            else if (practice.DeleteFile)
            {
                if (!string.IsNullOrEmpty(currentFilename) && System.IO.File.Exists(path + currentFilename))
                {
                    System.IO.File.Delete(path + currentFilename);
                }

                practice.Filename = null;
            }
            else
            {
                practice.Filename = currentFilename;
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _lessonPracticeService.UpdateLessonPractice(CurrentUserId, practice);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
        }

        [HttpPost]
        [AjaxOnly]
        [Demo(isJtableCaller: true)]
        public virtual ActionResult DeletePractice(long lessonId, long id)
        {
            var currentFilename = _lessonPracticeService.GetFilename(CurrentUserId, id);
            var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + CurrentUserId.ToString() + @"\LessonFiles\Practices\";

            if (!string.IsNullOrEmpty(currentFilename) && System.IO.File.Exists(path + currentFilename))
            {
                System.IO.File.Delete(path + currentFilename);
            }

            _professorService.UpdateLastUpdateTime(CurrentUserId);
            _lessonPracticeService.DeleteLessonPractice(CurrentUserId, id);
            _uow.SaveAllChanges();

            return Json(new { Result = "OK" });
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

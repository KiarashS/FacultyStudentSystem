using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ContentManagementSystem.Commons.Web;
using System.IO;
using System.Web;
using Lib.Web.Mvc;
using ContentManagementSystem.Web.Utils;
using ContentManagementSystem.Web.Infrastructure;
using System.Configuration;

namespace ContentManagementSystem.Web.Controllers
{
    public partial class HomeController : BaseController
    {
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;
        private readonly IProfessorService _professorService;
        private readonly ICollegeService _collegeService;
        private readonly IEducationalGroupService _educationalGroupService;
        private readonly INewsService _newsService;

        public HomeController(IUnitOfWork uow, IUserService userService, IProfessorService professorService, ICollegeService collegeService, IEducationalGroupService educationalGroupService, INewsService newsService)
        {
            _uow = uow;
            _userService = userService;
            _professorService = professorService;
            _collegeService = collegeService;
            _educationalGroupService = educationalGroupService;
            _newsService = newsService;
        }

        public virtual ActionResult Index(int page = 1)
        {
            //var x = new ArticlesFetcher();
            //var y = await x.SetProfessorOrcidArticles("https://scholar.google.com/citations?user=VMjnaEYAAAAJ&hl=en");

            var currentPageNumber = page >= 1 ? page : 1;
            var currentPageIndex = currentPageNumber - 1;
            var pageSize = 15;
            var totalIndexProfessorsCount = _professorService.TotalCount(p => p.IsSoftDelete == false);
            var users = _professorService.GetProfessorsList(p => p.IsSoftDelete == false, currentPageIndex, pageSize);

            if (Request.IsAuthenticated && User.IsInRole(ConstantsUtil.ProfessorRole))
            {
                ViewBag.PageId = _professorService.GetPageId(CurrentUserId);
            }

            ViewBag.Colleges = GetCollegeList(_collegeService.GetColleges());
            ViewBag.Groups = GetEducationalGroupList(_educationalGroupService.GetEducationalGroups());
            ViewBag.NoItemStyle = "display: none !important;";

            if (users == null || !users.Any())
            {
                ViewBag.NoItemStyle = "display: block !important;";
                return View();
            }

            var pageLinks = NextAndPreviousPages(totalIndexProfessorsCount, pageSize, currentPageNumber);
            ViewBag.PreviousPageLink = pageLinks.Item1;
            ViewBag.NextPageLink = pageLinks.Item2;
            ViewBag.CurrentPageIndex = currentPageIndex;
            ViewBag.TotalIndexProfessorsCount = totalIndexProfessorsCount;

            return View(users);
        }

        [HttpPost]
        [AjaxOnly]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public virtual ActionResult PagedIndex(int? page, string firstname, string lastname, string email, int college = 1, int educationalGroup = 1)
        {
            var pageNumber = page ?? 0;
            var pageSize = 15;

            firstname = (firstname != null && firstname.Trim().Length > 0) ? firstname.Trim() : null;
            lastname = (lastname != null && lastname.Trim().Length > 0) ? lastname.Trim() : null;
            email = (email != null && email.Trim().Length > 0) ? email.Trim() : null;

            var users = _professorService.GetPagedProfessorsList(firstname, lastname, email, college, educationalGroup, pageNumber, pageSize);
            if (users == null || !users.Any())
                return Content("no-more-info");

            ViewBag.CurrentPageIndex = pageNumber;
            ViewBag.FilterFirstname = firstname;
            ViewBag.FilterLastname = lastname;
            ViewBag.FilterEmail = email;
            ViewBag.FilterCollege = college;
            ViewBag.FilterEducationalGroup = educationalGroup;

            return PartialView(MVC.Home.Views._UsersList, users);
        }

        public virtual ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public virtual ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private Tuple<string, string> NextAndPreviousPages(int totalCount, int pageSize, int currentPage)
        {
            var totalPages = Math.Ceiling(decimal.Divide(totalCount, pageSize));
            string previousPageLink = null;
            string nextPageLink = null;

            if (currentPage == 1 && totalPages > currentPage)
            {
                nextPageLink = Url.Action(MVC.Home.Index(currentPage + 1));
            }
            else if (currentPage != 1 && currentPage == totalPages)
            {
                previousPageLink = Url.Action(MVC.Home.Index(currentPage - 1));
            }
            else if (currentPage > 1)
            {
                previousPageLink = Url.Action(MVC.Home.Index(currentPage - 1));
                nextPageLink = Url.Action(MVC.Home.Index(currentPage + 1));
            }

            return new Tuple<string, string>(previousPageLink, nextPageLink);
        }

        public virtual ActionResult UserAvatar(string code)
        {
            var avatarName = RijndaelManagedEncryption.DecryptRijndael(code);
            var avatarPath = Server.MapPath("~") + @"\App_Data\Avatars\" + avatarName;
            var contentTypes = new Dictionary<string, string>();
            contentTypes.Add(".jpg", "image/jpeg");
            contentTypes.Add(".png", "image/png");

            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(avatarName) || avatarName.Trim().ToLowerInvariant() == "empty avatar")
            {
                avatarName = "avatar.png";
                avatarPath = Server.MapPath("~") + @"\Content\admin\img\" + avatarName;
            }

            Response.AddHeader("Content-Disposition", "attachment; filename=myavatar" + Path.GetExtension(avatarName));
            return File(System.IO.File.ReadAllBytes(avatarPath), contentTypes[Path.GetExtension(avatarName)]);
        }

        public List<SelectListItem> GetEducationalGroupList(IEnumerable<EducationalGroupViewModel> groups)
        {
            var listItems = new List<SelectListItem>();
            foreach (var item in groups)
            {
                if (item.Name == "--")
                {
                    item.Name = "گروه آموزشی";
                }

                listItems.Add(new SelectListItem
                {
                    Selected = item.Name == "گروه آموزشی",
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }

            //listItems.RemoveAt(0); // Remove NotDefined
            return listItems;
        }

        public List<SelectListItem> GetCollegeList(IEnumerable<CollegeViewModel> colleges)
        {
            var listItems = new List<SelectListItem>();
            foreach (var item in colleges)
            {
                if (item.Name == "--")
                {
                    item.Name = "دانشکده";
                }

                listItems.Add(new SelectListItem
                {
                    Selected = item.Name == "دانشکده",
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }

            //listItems.RemoveAt(0); // Remove NotDefined
            return listItems;
        }

        [ChildActionOnly]
        public virtual ActionResult GetNewsTicker()
        {
            var newsTickerCount = Convert.ToInt32(ConfigurationManager.AppSettings["NewsTickerCount"]);
            var news = _newsService.GetNewsTicker(newsTickerCount);

            if (!news.Any())
            {
                return new EmptyResult();
            }

            return PartialView(MVC.Home.Views._GetNewsTicker, news);
        }

        public virtual ActionResult GetFile(string fileText, byte type)
        {
            var typesPath = new Dictionary<byte, string>
            {
                [1] = @"\LessonFiles\Files\",
                [2] = @"\LessonFiles\Practices\",
                [3] = @"\LessonFiles\Scores\",
                [4] = @"\Resume\",
                [5] = @"\GalleryFiles\",
                [6] = @"\ResearchFiles\",
            };

            var rawFileText = RijndaelManagedEncryption.DecryptRijndael(fileText);
            var userId = rawFileText.Split(";#;")[0];
            var filename = rawFileText.Split(";#;")[1];
            var pureFilename = filename.Split("__")[2]; //(DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond).ToString();
            var path = Server.MapPath("~") + @"\App_Data\UsersFiles\" + userId + typesPath[type] + filename;
            var fileInfo = new FileInfo(path);

            Response.AddHeader("Content-Disposition", "attachment; filename=" + pureFilename + Path.GetExtension(filename));
            Response.BufferOutput = false;

            return new RangeFilePathResult(MimeMapping.GetMimeMapping(filename), path,
                                           fileInfo.LastWriteTimeUtc, fileInfo.Length);
            //return File(path, MimeMapping.GetMimeMapping(filename));
        }

        private void TransmitFile(string fullPath, string contentType)
        {
            System.IO.Stream iStream = null;

            // Buffer to read 10K bytes in chunk
            byte[] buffer = new Byte[10000];

            // Length of the file
            int length;

            // Total bytes to read
            long dataToRead;

            // Identify the file to download including its path
            string filepath = fullPath;

            // Identify the file name
            string filename = System.IO.Path.GetFileName(filepath);

            try
            {
                // Open the file
                iStream = new System.IO.FileStream(filepath, System.IO.FileMode.Open,
                            System.IO.FileAccess.Read, System.IO.FileShare.Read);


                // Total bytes to read
                dataToRead = iStream.Length;

                Response.Clear();
                Response.ContentType = contentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
                Response.AddHeader("Content-Length", iStream.Length.ToString());

                // Read the bytes
                while (dataToRead > 0)
                {
                    // Verify that the client is connected
                    if (Response.IsClientConnected)
                    {
                        // Read the data in buffer
                        length = iStream.Read(buffer, 0, 10000);

                        // Write the data to the current output stream
                        Response.OutputStream.Write(buffer, 0, length);

                        // Flush the data to the output
                        Response.Flush();

                        buffer = new Byte[10000];
                        dataToRead = dataToRead - length;
                    }
                    else
                    {
                        //prevent infinite loop if user disconnects
                        dataToRead = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            finally
            {
                if (iStream != null)
                {
                    //Close the file.
                    iStream.Close();
                }
                Response.Close();
            }
        }
    }
}

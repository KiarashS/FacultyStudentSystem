using ContentManagementSystem.DataLayer.Context;
using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.ViewModels;
using ContentManagementSystem.ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFSecondLevelCache;
using System.Linq.Expressions;
using Z.EntityFramework.Plus;

namespace ContentManagementSystem.ServiceLayer
{
    public class EFProfessorService : IProfessorService
    {
        IUnitOfWork _uow;
        readonly IDbSet<Professor> _professors;
        private readonly Lazy<IUserService> _userService;
        private readonly Lazy<ICollegeService> _collegeService;
        private readonly Lazy<IEducationalGroupService> _educationalGroupService;
        private readonly Lazy<IEducationalDegreeService> _educationalDegreeService;
        private readonly Lazy<IAcademicRankService> _academicRankService;

        public EFProfessorService(IUnitOfWork uow, Lazy<IUserService> userService, Lazy<ICollegeService> collegeService, Lazy<IEducationalGroupService> educationalGroupService, Lazy<IEducationalDegreeService> educationalDegreeService, Lazy<IAcademicRankService> academicRankService)
        {
            _uow = uow;
            _professors = _uow.Set<Professor>();
            _userService = userService;
            _collegeService = collegeService;
            _educationalGroupService = educationalGroupService;
            _educationalDegreeService = educationalDegreeService;
            _academicRankService = academicRankService;
        }

        public BioViewModel GetBio(int userId)
        {
            var bio = _professors
                    .Where(p => p.UserId == userId)
                    .Select(p => new { p.Bio, p.IsActiveBio })
                    .Single();

            return new BioViewModel { BioText = bio.Bio, IsActiveBio = bio.IsActiveBio };
        }

        public void UpdateBio(int userId, string bio)
        {
            var professor = _professors.Single(p => p.UserId == userId);
            professor.Bio = bio;
        }

        public FreePageViewModel GetFreePage(string pageId)
        {
            var freePage = _professors
                    .IncludeOptimized(p => p.UserDetails)
                    .Where(p => p.PageId == pageId)
                    .Select(p => new { p.FreePage, p.IsActiveFreePage, p.FreePageLastUpdateTime,
                                       p.UserDetails.FirstName, p.UserDetails.LastName })
                    .Single();

            return new FreePageViewModel { FreePageText = freePage.FreePage, IsActiveFreePage = freePage.IsActiveFreePage,
                                           FreePageLastUpdateTime = freePage.FreePageLastUpdateTime, Firstname = freePage.FirstName,
                                           Lastname = freePage.LastName };
        }

        public FreePageViewModel GetFreePage(int userId)
        {
            var freePage = _professors
                    .Where(p => p.UserId == userId)
                    .Select(p => new { p.FreePage, p.IsActiveFreePage, p.FreePageLastUpdateTime })
                    .Single();

            return new FreePageViewModel { FreePageText = freePage.FreePage, IsActiveFreePage = freePage.IsActiveFreePage, FreePageLastUpdateTime = freePage.FreePageLastUpdateTime };
        }

        public void UpdateFreePage(int userId, string freePage)
        {
            var professor = _professors.Single(p => p.UserId == userId);

            professor.FreePage = freePage;
            professor.FreePageLastUpdateTime = DateTime.UtcNow;
        }

        public void UpdateAvatar(int userId, string imageFileName)
        {
            var professor = _professors.Single(p => p.UserId == userId);
            professor.Avatar = imageFileName; // with extension
        }

        public string GetAvatar(int userId)
        {
            return _professors
                   .Where(p => p.UserId == userId)
                   .Select(p => p.Avatar)
                   .SingleOrDefault();
        }

        public DetailsViewModel GetProfessorDashboardDetails(int userId)
        {
            var professorDetails = _professors.Where(p => p.UserId == userId).IncludeOptimized(p => p.UserDetails).Single();

            return new DetailsViewModel
            {
                FirstName = professorDetails.UserDetails.FirstName,
                LastName = professorDetails.UserDetails.LastName,
                Email = professorDetails.UserDetails.Email,
                AcademicRank = professorDetails.AcademicRankId,
                BirthDate = professorDetails.BirthDate,
                BirthPlace = professorDetails.BirthPlace,
                CommonAuthorPaperName = professorDetails.CommonAuthorPaperName,
                EducationalDegree = professorDetails.EducationalDegreeId,
                EducationalGroup = professorDetails.EducationalGroupId,
                GoogleScholarId = professorDetails.GoogleScholarId,
                Interests = professorDetails.Interests,
                Location = professorDetails.Location,
                MaritalStatus = professorDetails.MaritalStatus,
                MedLibId = professorDetails.MedLibId,
                Mobile = professorDetails.Mobile,
                OrcidId = professorDetails.OrcidId,
                PersonalWebPage = professorDetails.PersonalWebPage,
                PubMedId = professorDetails.PubMedId,
                ResearchFields = professorDetails.ResearchFields,
                ResearchGateId = professorDetails.ResearchGateId,
                ScopusId = professorDetails.ScopusId,
                SecondaryEmails = professorDetails.SecondaryEmails,
                Sex = professorDetails.Sex,
                ResearcherId = professorDetails.ResearcherId,
                College = professorDetails.CollegeId
            };
        }

        public void UpdateDetails(int userId, DetailsViewModel info)
        {
            var professor = _professors.Single(u => u.UserId == userId);

            professor.AcademicRankId = info.AcademicRank;
            professor.BirthDate = info.BirthDate;
            professor.BirthPlace = info.BirthPlace;
            professor.CommonAuthorPaperName = info.CommonAuthorPaperName;
            professor.EducationalDegreeId = info.EducationalDegree;
            professor.EducationalGroupId = info.EducationalGroup;
            professor.GoogleScholarId = info.GoogleScholarId;
            professor.Interests = info.Interests;
            professor.Location = info.Location;
            professor.MaritalStatus = info.MaritalStatus;
            professor.MedLibId = info.MedLibId;
            professor.Mobile = info.Mobile;
            professor.OrcidId = info.OrcidId;
            professor.PersonalWebPage = info.PersonalWebPage;
            professor.PubMedId = info.PubMedId;
            professor.ResearcherId = info.ResearcherId;
            professor.ResearchFields = info.ResearchFields;
            professor.ResearchGateId = info.ResearchGateId;
            professor.ScopusId = info.ScopusId;
            professor.SecondaryEmails = !string.IsNullOrEmpty(info.SecondaryEmails) ? info.SecondaryEmails.Trim().ToLowerInvariant() : null;
            professor.Sex = info.Sex;
            professor.CollegeId = info.College;
        }

        public void UpdateLastUpdateTime(int userId, bool updateHIndexAndCitationDate = false, bool updateExternalArticlesDate = false)
        {
            //var stub = new Professor { UserId = userId };
            //_uow.Entry(stub).State = EntityState.Modified;
            //stub.LastUpdateTime = DateTime.UtcNow;
            var user = _professors.Single(u => u.UserId == userId);
            var currentTime = DateTime.UtcNow;
            user.LastUpdateTime = currentTime;

            if (updateHIndexAndCitationDate)
            {
                user.HIndexAndDocumentCitationLastUpdateTime = currentTime;
            }

            if (updateExternalArticlesDate)
            {
                user.ExternalResearchLastUpdateTime = currentTime;
            }
        }

        public BanInfoViewModel GetBanInfo(int userId)
        {
            var professor =
                _professors.Where(p => p.UserId == userId)
                    .Select(p => new { p.IsBanned, p.BannedReason, p.BannedDate, p.IsSoftDelete })
                    .SingleOrDefault();

            if (professor == null)
            {
                return null;
            }

            return new BanInfoViewModel
            {
                IsBanned = professor.IsBanned,
                BannedReason = professor.BannedReason,
                BannedDate = professor.BannedDate,
                IsSoftDelete = professor.IsSoftDelete
            };
        }

        public ProfileValidationViewModel IsValidPageId(string pageId)
        {
            var professor = _professors.Where(p => p.PageId == pageId).IncludeOptimized(p => p.UserDetails.Roles).Select(p => new { p.UserId, p.PageId, p.IsSoftDelete, UserRoles = p.UserDetails.Roles.Select(r => r.Name) }).Cacheable().SingleOrDefault();

            if (professor == null)
            {
                return new ProfileValidationViewModel
                {
                    IsValidPageId = false
                };
            }

            //var userRoles = _userService.Value.GetRolesForUser(professor.UserId);
            return new ProfileValidationViewModel
            {
                PageId = professor.PageId,
                IsValidPageId = true,
                IsSoftDelete = professor.IsSoftDelete,
                Roles = professor.UserRoles
            };
        }

        public ProfileIntroViewModel GetProfileIntroInfos(string pageId)
        {
            var professor = _professors.
                Where(p => p.PageId == pageId).
                Include(p => p.UserDetails).
                Include(p => p.College).
                Include(p => p.EducationalGroup).
                Include(p => p.EducationalDegree).
                Include(p => p.AcademicRank).
                Select(p => new {
                    p.UserDetails.FirstName,
                    p.UserDetails.LastName,
                    p.UserDetails.Email,
                    p.CommonAuthorPaperName,
                    RankName = p.AcademicRank.Name,
                    p.Avatar,
                    p.Bio,
                    p.BirthDate,
                    p.BirthPlace,
                    DegreeName = p.EducationalDegree.Name,
                    GroupName = p.EducationalGroup.Name,
                    p.GoogleScholarId,
                    p.Interests,
                    p.LastUpdateTime,
                    p.Location,
                    p.MaritalStatus,
                    p.MedLibId,
                    p.Mobile,
                    p.PersianResumeFileName,
                    p.OrcidId,
                    p.PageId,
                    p.PersonalWebPage,
                    p.PubMedId,
                    p.ResearcherId,
                    p.ResearchFields,
                    p.ResearchGateId,
                    p.ScopusId,
                    p.SecondaryEmails,
                    p.Sex,
                    CollegeName = p.College.Name,
                    p.UserId,
                    p.IsActiveBio,
                    p.FreePage,
                    p.IsActiveFreePage,
                    p.IsActiveWeeklyProgram,
                    //p.ScopusCitations,
                    p.ScopusDocuments,
                    p.ScopusHIndex,
                    //p.ScopusTotalDocumentsCited,
                    p.GoogleCitations,
                    p.GoogleHIndex,
                    p.OtherNamesFormat,
                    p.ShowHIndexSection,
                    p.ShowScopusDocumentsCitationChart,
                    p.ShowGoogleDocumentsCitationChart,
                    p.HIndexAndDocumentCitationLastUpdateTime,
                    HasScopusChart = p.DocumentCitations.Any(dc => dc.Source == DocSource.Scopus),
                    HasGoogleChart = p.DocumentCitations.Any(dc => dc.Source == DocSource.Google),
                    HasAdministrationRecord = p.AdministrationRecords.Any(),
                    HasAddress = p.Addresses.Any(),
                    HasCourseAndWorkshop = p.CourseAndWorkshops.Any(),
                    HasStudingRecord = p.StudingRecords.Any(),
                    HasExternalResearchRecord = p.ExternalResearchRecords.Any(),
                    HasExternalSeminarRecord = p.ExternalSeminarRecords.Any(),
                    HasHonor = p.Honors.Any(),
                    HasInternalResearchRecord = p.InternalResearchRecords.Any(),
                    HasInternalSeminarRecord = p.InternalSeminarRecords.Any(),
                    HasPublication = p.Publications.Any(),
                    HasResearchRecord = p.ResearchRecords.Any(),
                    HasThesis = p.Thesis.Any(),
                    HasTrainingRecord = p.TrainingRecords.Any(),
                    HasFreeField = p.FreeFields.Any(),
                    HasLanguage = p.Languages.Any(),
                    HasLesson = p.Lessons.Any(),
                    HasMembership = p.Memberships.Any(),
                    HasGallery = p.Galleries.Any(g => g.IsActive),
                    HasWeeklyProgram = p.WeeklyPrograms.Any()
                }).Cacheable().Single();


            return new ProfileIntroViewModel
            {
                AcademicRank = professor.RankName,
                AvatarName = professor.Avatar,
                BirthDate = professor.BirthDate,
                BirthPlace = professor.BirthPlace,
                CommonAuthorPaperName = professor.CommonAuthorPaperName,
                Bio = professor.Bio,
                EducationalDegree = professor.DegreeName,
                EducationalGroup = professor.GroupName,
                Email = professor.Email,
                Firstname = professor.FirstName,
                Lastname = professor.LastName,
                GoogleScholarId = professor.GoogleScholarId,
                Interests = professor.Interests,
                LastUpdateTime = professor.LastUpdateTime,
                Location = professor.Location,
                MaritalStatus = professor.MaritalStatus,
                MedLibId = professor.MedLibId,
                ResearcherId = professor.ResearcherId,
                Mobile = professor.Mobile,
                PersianResumeFilename = professor.PersianResumeFileName,
                OrcidId = professor.OrcidId,
                PersonalWebPage = professor.PersonalWebPage,
                PubMedId = professor.PubMedId,
                ResearchFields = professor.ResearchFields,
                ResearchGateId = professor.ResearchGateId,
                ScopusId = professor.ScopusId,
                SecondaryEmails = professor.SecondaryEmails,
                Sex = professor.Sex,
                College = professor.CollegeName,
                UserId = professor.UserId,
                IsActiveBio = professor.IsActiveBio,
                FreePage = professor.FreePage,
                IsActiveFreePage = professor.IsActiveFreePage,
                IsActiveWeeklyProgram = professor.IsActiveWeeklyProgram,
                ScopusHIndex = professor.ScopusHIndex,
                //ScopusCitations = professor.ScopusCitations,
                ScopusDocuments = professor.ScopusDocuments,
                OtherNamesFormat = professor.OtherNamesFormat,
                //ScopusTotalDocumentsCited = professor.ScopusTotalDocumentsCited,
                GoogleCitations = professor.GoogleCitations,
                GoogleHIndex = professor.GoogleHIndex,
                ShowHIndexSection = professor.ShowHIndexSection,
                ShowScopusDocumentsCitationChart = professor.ShowScopusDocumentsCitationChart,
                ShowGoogleDocumentsCitationChart = professor.ShowGoogleDocumentsCitationChart,
                HIndexAndDocumentCitationLastUpdateTime = professor.HIndexAndDocumentCitationLastUpdateTime,
                HasScopusChart = professor.HasScopusChart,
                HasGoogleChart = professor.HasGoogleChart,
                HasAddress = professor.HasAddress,
                HasAdministrationRecord = professor.HasAdministrationRecord,
                HasCourseAndWorkshop = professor.HasCourseAndWorkshop,
                HasStudingRecord = professor.HasStudingRecord,
                HasExternalResearchRecord = professor.HasExternalResearchRecord,
                HasExternalSeminarRecord = professor.HasExternalSeminarRecord,
                HasHonor = professor.HasHonor,
                HasInternalResearchRecord = professor.HasInternalResearchRecord,
                HasInternalSeminarRecord = professor.HasInternalSeminarRecord,
                HasPublication = professor.HasPublication,
                HasResearchRecord = professor.HasResearchRecord,
                HasThesis = professor.HasThesis,
                HasTrainingRecord = professor.HasTrainingRecord,
                HasFreeField = professor.HasFreeField,
                HasLanguage = professor.HasLanguage,
                HasLesson = professor.HasLesson,
                HasMembership = professor.HasMembership,
                HasGallery = professor.HasGallery,
                HasWeeklyProgram = professor.HasWeeklyProgram
            };
        }

        public string GetAvatar(string pageId)
        {
            return _professors
                       .Where(p => p.PageId == pageId)
                       .Select(p => p.Avatar)
                       .Single();
        }

        public IEnumerable<AddressListViewModel> GetListAddresses(string pageId)
        {
            var professorAddresses = _professors.
                Where(p => p.PageId == pageId).
                Include(p => p.Addresses).
                Select(p => p.Addresses.Select(a => new { a.AddressId, a.PostalAddress, a.PostalCode, a.Tell, a.Fax, a.Link, a.Order }))
                .Cacheable()
                .Single();

            professorAddresses = professorAddresses.OrderByDescending(a => a.Order).ThenBy(a => a.AddressId);
            var addresses = new List<AddressListViewModel>();
            
            foreach (var address in professorAddresses)
            {
                addresses.Add(new AddressListViewModel
                {
                    PostalAddress = string.IsNullOrEmpty(address.PostalAddress) ? string.Empty : address.PostalAddress,
                    PostalCode = string.IsNullOrEmpty(address.PostalCode) ? null : address.PostalCode,
                    Tel = string.IsNullOrEmpty(address.Tell) ? null : address.Tell,
                    Fax = string.IsNullOrEmpty(address.Fax) ? null : address.Fax,
                    Link = address.Link
                });
            }

            return addresses;
        }

        public void DeleteProfessor(int userId)
        {
            //var professor = new Professor { UserId = userId };
            Professor professor = _professors.Find(userId);
            if (professor != null)
            {
                _professors.Attach(professor);
            }
            else
            {
                professor = new Professor { UserId = userId };
            }

            _uow.Entry(professor).State = EntityState.Deleted;
        }

        public void CreateProfessor(UserListViewModel user, bool updateSoftDeleteDate = true)
        {
            var newProfessor = new Professor();
            var defaultCollegeId = _collegeService.Value.GetIdByName("--");
            var defaultEducationalGroupId = _educationalGroupService.Value.GetIdByName("--");
            var defaultEducationalDegreeId = _educationalDegreeService.Value.GetIdByName("--");
            var defaultAcademicRankId = _academicRankService.Value.GetIdByName("--");

            newProfessor.UserId = user.UserId;
            newProfessor.PageId = user.PageId.Trim().ToLowerInvariant();
            newProfessor.IsBanned = user.IsBanned;
            newProfessor.IsSoftDelete = user.IsSoftDelete;
            newProfessor.BannedDate = null;
            newProfessor.BannedReason = null;
            newProfessor.CollegeId = defaultCollegeId;
            newProfessor.EducationalGroupId = defaultEducationalGroupId;
            newProfessor.EducationalDegreeId = defaultEducationalDegreeId;
            newProfessor.AcademicRankId = defaultAcademicRankId;

            if(updateSoftDeleteDate)
            {
                newProfessor.SoftDeleteDate = DateTime.UtcNow;
            }

            if (user.IsBanned)
            {
                newProfessor.BannedDate = DateTime.UtcNow;
                newProfessor.BannedReason = user.BannedReason;
            }

            _professors.Add(newProfessor);
        }

        public bool IsUniquePageId(int userId, string pageId)
        {
            return !_professors.Any(u => u.UserId != userId && u.PageId == pageId);
        }

        public bool ExistProfessor(int userId)
        {
            return _professors.Any(p => p.UserId == userId);
        }

        public int TotalCount(Expression<Func<Professor, bool>> predicate)
        {
            if (predicate == null)
            {
                return _professors.Cacheable().Count();
            }

            return _professors.Where(predicate).Cacheable().Count();
        }

        public IList<IndexUsersListViewModel> GetProfessorsList(Expression<Func<Professor, bool>> predicate, int pageIndex = 0, int pageSize = 20)
        {
            var skipRecords = pageIndex * pageSize;
            var professors = new List<IndexUsersListViewModel>();
            var list = _professors
                .Include(p => p.UserDetails)
                .Include(p => p.College)
                .Include(p => p.EducationalGroup)
                .Include(p => p.EducationalDegree)
                .Where(predicate)
                .Select(p => new { p.UserDetails.FirstName, p.UserDetails.LastName, p.UserDetails.Email, p.PageId, p.LastUpdateTime, p.Avatar, CollegeName = p.College.Name, EducationalDegreeName = p.EducationalDegree.Name, EducationalGroupName = p.EducationalGroup.Name })
                .OrderByDescending(p => p.LastUpdateTime).ThenBy(p => p.LastName)
                .Skip(skipRecords)
                .Take(pageSize)
                .Cacheable()
                .ToList();

            foreach (var professor in list)
            {
                professors.Add(new IndexUsersListViewModel
                {
                    Firstname = professor.FirstName,
                    Lastname = professor.LastName,
                    LastUpdateTime = professor.LastUpdateTime,
                    EducationalGroup = professor.EducationalGroupName,
                    EducationalDegree = professor.EducationalDegreeName,
                    AvatarName = professor.Avatar,
                    College = professor.CollegeName,
                    Email = professor.Email,
                    PageId = professor.PageId,
                });
            }

            return professors;
        }

        public IList<IndexUsersListViewModel> GetPagedProfessorsList(string firstname, string lastname, string email, int college, int educationalGroup, int pageIndex = 0, int pageSize = 20)
        {
            var skipRecords = pageIndex * pageSize;
            var professors = new List<IndexUsersListViewModel>();
            var query = _professors.Where(p => p.IsSoftDelete == false).AsQueryable();

            if (!string.IsNullOrEmpty(firstname) || !string.IsNullOrEmpty(lastname) || !string.IsNullOrEmpty(email))
            {
                query = query.IncludeOptimized(p => p.UserDetails);
            }

            if (!string.IsNullOrEmpty(firstname))
            {
                query = query.Where(p => p.UserDetails.FirstName.Contains(firstname));
            }

            if (!string.IsNullOrEmpty(lastname))
            {
                query = query.Where(p => p.UserDetails.LastName.Contains(lastname));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(p => p.UserDetails.Email.Contains(email) || p.SecondaryEmails.Contains(email));
            }

            if (college != _collegeService.Value.GetIdByName("--"))
            {
                query = query.Where(p => p.CollegeId == college);
            }

            if (educationalGroup != _educationalGroupService.Value.GetIdByName("--"))
            {
                query = query.Where(p => p.EducationalGroupId == educationalGroup);
            }

            var list = query
                .Include(p => p.UserDetails)
                .Include(p => p.College)
                .Include(p => p.EducationalGroup)
                .Include(p => p.EducationalDegree)
                .Select(p => new { p.UserDetails.FirstName, p.UserDetails.LastName, p.UserDetails.Email, p.PageId, p.LastUpdateTime, p.Avatar, CollegeName = p.College.Name, EducationalDegreeName = p.EducationalDegree.Name, EducationalGroupName = p.EducationalGroup.Name })
                .OrderByDescending(p => p.LastUpdateTime).ThenBy(p => p.LastName)
                .Skip(skipRecords)
                .Take(pageSize)
                .Cacheable()
                .ToList();

            foreach (var professor in list)
            {
                professors.Add(new IndexUsersListViewModel
                {
                    Firstname = professor.FirstName,
                    Lastname = professor.LastName,
                    LastUpdateTime = professor.LastUpdateTime,
                    EducationalGroup = professor.EducationalGroupName,
                    EducationalDegree = professor.EducationalDegreeName,
                    AvatarName = professor.Avatar,
                    College = professor.CollegeName,
                    Email = professor.Email,
                    PageId = professor.PageId,
                });
            }

            return professors;
        }

        public IEnumerable<FreeFieldListViewModel> GetFreeFields(string pageId)
        {
            var professorFreeFields = _professors.
                Where(p => p.PageId == pageId).
                Include(p => p.FreeFields).
                Select(p => p.FreeFields.Select(a => new { a.Id, a.Name, a.Value, a.Order }))
                .Cacheable()
                .Single();

            professorFreeFields = professorFreeFields.OrderByDescending(a => a.Order).ThenBy(a => a.Id);
            var freeFields = new List<FreeFieldListViewModel>();

            foreach (var freeField in professorFreeFields)
            {
                freeFields.Add(new FreeFieldListViewModel
                {
                    Name = freeField.Name,
                    Value = freeField.Value
                });
            }

            return freeFields;
        }

        public string GetPageId(int userId)
        {
            return _professors.Where(p => p.UserId == userId).Select(p => p.PageId).Cacheable().Single();
        }

        public IEnumerable<ResearchViewModel> GetListResearchs(string pageId)
        {
            var professorResearchs = _professors.
                Where(p => p.PageId == pageId).
                Include(p => p.ResearchRecords).
                Select(p => p.ResearchRecords.Select(r => new { r.Id, r.Title, r.Place, r.StartTime, r.EndTime, r.Description, r.Link, r.Order }))
                .Cacheable()
                .Single();

            professorResearchs = professorResearchs.OrderByDescending(r => r.Order).ThenBy(r => r.Id);
            var researchs = new List<ResearchViewModel>();

            foreach (var professor in professorResearchs)
            {
                researchs.Add(new ResearchViewModel
                {
                    Id = professor.Id,
                    Title = professor.Title,
                    Place = professor.Place,
                    StartTime = professor.StartTime,
                    EndTime = professor.EndTime,
                    Description = professor.Description,
                    Link = professor.Link
                });
            }

            return researchs;
        }

        public IEnumerable<HonorViewModel> GetListHonors(string pageId)
        {
            var professorHonors = _professors.
                Where(p => p.PageId == pageId).
                Include(p => p.Honors).
                Select(p => p.Honors.Select(h => new { h.Id, h.Title, h.Time, h.Link, h.Order }))
                .Cacheable()
                .Single();

            professorHonors = professorHonors.OrderByDescending(h => h.Order).ThenBy(h => h.Id);
            var honors = new List<HonorViewModel>();

            foreach (var honor in professorHonors)
            {
                honors.Add(new HonorViewModel
                {
                    Id = honor.Id,
                    Title = honor.Title,
                    Time = honor.Time,
                    Link = honor.Link
                });
            }

            return honors;
        }

        public IEnumerable<PublicationViewModel> GetListPublications(string pageId)
        {
            var professorPublications = _professors.
                Where(p => p.PageId == pageId).
                Include(p => p.Publications).
                Select(p => p.Publications.Select(pub => new { pub.Id, pub.Title, pub.Publisher, pub.Time, pub.Description, pub.Link, pub.Order }))
                .Cacheable()
                .Single();

            professorPublications = professorPublications.OrderByDescending(p => p.Order).ThenBy(p => p.Id);
            var publications = new List<PublicationViewModel>();

            foreach (var publication in professorPublications)
            {
                publications.Add(new PublicationViewModel
                {
                    Id = publication.Id,
                    Title = publication.Title,
                    Publisher = publication.Publisher,
                    Description = publication.Description,
                    Time = publication.Time,
                    Link = publication.Link
                });
            }

            return publications;
        }

        public IEnumerable<ThesisViewModel> GetListTheses(string pageId)
        {
            var professorTheses = _professors.
                Where(p => p.PageId == pageId).
                Include(p => p.Thesis).
                Select(p => p.Thesis.Select(t => new { t.Id, t.Title, t.ThesisPost, t.ThesisGrade, t.ThesisState, t.ThesisType, t.Executers, t.University, t.Time, t.Description, t.Link, t.Order }))
                .Cacheable()
                .Single();

            professorTheses = professorTheses.OrderByDescending(t => t.Order).ThenBy(t => t.Id);
            var theses = new List<ThesisViewModel>();

            foreach (var thesis in professorTheses)
            {
                theses.Add(new ThesisViewModel
                {
                    Id = thesis.Id,
                    Title = thesis.Title,
                    ThesisPost = thesis.ThesisPost,
                    ThesisType = thesis.ThesisType,
                    ThesisGrade = thesis.ThesisGrade,
                    ThesisState = thesis.ThesisState,
                    Executers = thesis.Executers,
                    University = thesis.University,
                    Description = thesis.Description,
                    Time = thesis.Time,
                    Link = thesis.Link
                });
            }

            return theses;
        }

        public IEnumerable<WorkshopViewModel> GetListWorkshops(string pageId)
        {
            var professorWorkshops = _professors.
                Where(p => p.PageId == pageId).
                Include(p => p.CourseAndWorkshops).
                Select(p => p.CourseAndWorkshops.Select(w => new { w.Id, w.Title, w.StartTime, w.EndTime, w.Host, w.Place, w.Description, w.Link, w.Order }))
                .Cacheable()
                .Single();

            professorWorkshops = professorWorkshops.OrderByDescending(w => w.Order).ThenBy(w => w.Id);
            var workshops = new List<WorkshopViewModel>();

            foreach (var workshop in professorWorkshops)
            {
                workshops.Add(new WorkshopViewModel
                {
                    Id = workshop.Id,
                    Title = workshop.Title,
                    Host = workshop.Host,
                    Place = workshop.Place,
                    StartTime = workshop.StartTime,
                    EndTime = workshop.EndTime,
                    Description = workshop.Description,
                    Link = workshop.Link
                });
            }

            return workshops;
        }

        public IEnumerable<TrainingViewModel> GetListTrainings(string pageId)
        {
            var professorTrainings = _professors.
                Where(p => p.PageId == pageId).
                Include(p => p.TrainingRecords).
                Select(p => p.TrainingRecords.Select(t => new { t.Id, t.Title, t.Time, t.FromTime, t.ToTime, t.Teacher, t.Participant, t.Secretary, t.Place, t.Link, t.Order }))
                .Cacheable()
                .Single();

            professorTrainings = professorTrainings.OrderByDescending(t => t.Order).ThenBy(t => t.Id);
            var trainings = new List<TrainingViewModel>();

            foreach (var training in professorTrainings)
            {
                trainings.Add(new TrainingViewModel
                {
                    Id = training.Id,
                    Title = training.Title,
                    Place = training.Place,
                    Time = training.Time,
                    FromTime = training.FromTime,
                    ToTime = training.ToTime,
                    Teacher = training.Teacher,
                    Participant = training.Participant,
                    Secretary = training.Secretary,
                    Link = training.Link
                });
            }

            return trainings;
        }

        public IEnumerable<StudingViewModel> GetListStudings(string pageId)
        {
            var professorStudings = _professors.
                Where(p => p.PageId == pageId).
                Include(p => p.StudingRecords).
                Select(p => p.StudingRecords.Select(s => new { s.Id, s.Grade, s.Field, s.Trend, s.University, s.StartTime, s.EndTime, s.ThesisTitle, s.ThesisSupervisors, s.ThesisAdvisors, s.Link, s.Order }))
                .Cacheable()
                .Single();

            professorStudings = professorStudings.OrderByDescending(s => s.Order).ThenBy(s => s.Id);
            var studings = new List<StudingViewModel>();

            foreach (var studing in professorStudings)
            {
                studings.Add(new StudingViewModel
                {
                    Id = studing.Id,
                    Grade = studing.Grade,
                    Field = studing.Field,
                    Trend = studing.Trend,
                    University = studing.University,
                    StartTime = studing.StartTime,
                    EndTime = studing.EndTime,
                    ThesisTitle = studing.ThesisTitle,
                    ThesisSupervisors = studing.ThesisSupervisors,
                    ThesisAdvisors = studing.ThesisAdvisors,
                    Link = studing.Link
                });
            }

            return studings;
        }

        public IEnumerable<AdministrationViewModel> GetListAdministrations(string pageId)
        {
            var professorAdministrations = _professors.
                Where(p => p.PageId == pageId).
                Include(p => p.AdministrationRecords).
                Select(p => p.AdministrationRecords.Select(a => new { a.Id, a.Post, a.Place, a.StartTime, a.EndTime, a.Description, a.Link, a.Order }))
                .Cacheable()
                .Single();

            professorAdministrations = professorAdministrations.OrderByDescending(a => a.Order).ThenBy(a => a.Id);
            var administrations = new List<AdministrationViewModel>();

            foreach (var administration in professorAdministrations)
            {
                administrations.Add(new AdministrationViewModel
                {
                    Id = administration.Id,
                    Post = administration.Post,
                    Place = administration.Place,
                    StartTime = administration.StartTime,
                    EndTime = administration.EndTime,
                    Description = administration.Description,
                    Link = administration.Link
                });
            }

            return administrations;
        }

        public IEnumerable<LanguageViewModel> GetListLanguages(string pageId)
        {
            var professorLanguages = _professors.
                Where(p => p.PageId == pageId).
                Include(p => p.Languages).
                Select(p => p.Languages.Select(l => new { l.Id, l.Name, l.Level, l.Description, l.Link, l.Order }))
                .Cacheable()
                .Single();

            professorLanguages = professorLanguages.Where(l => l.Name != ProfessorLanguageName.NotDefined).OrderByDescending(l => l.Order).ThenBy(l => l.Id);
            var languages = new List<LanguageViewModel>();

            foreach (var language in professorLanguages)
            {
                languages.Add(new LanguageViewModel
                {
                    Id = language.Id,
                    Name = language.Name,
                    Level = language.Level,
                    Description = language.Description,
                    Link = language.Link
                });
            }

            return languages;
        }

        public void UpdateEducationalDegreeToDefault(int currentDegreeId, int newDegreeId)
        {
            _professors.Where(p => p.EducationalDegreeId == currentDegreeId)
                       .Update(p => new Professor { EducationalDegreeId = newDegreeId });
        }

        public void UpdateEducationalGroupToDefault(int currentGroupId, int newGroupId)
        {
            _professors.Where(p => p.EducationalGroupId == currentGroupId)
                       .Update(p => new Professor { EducationalGroupId = newGroupId });
        }

        public void UpdateCollegeToDefault(int currentCollegeId, int newCollegeId)
        {
            _professors.Where(p => p.CollegeId == currentCollegeId)
                       .Update(p => new Professor { CollegeId = newCollegeId });
        }

        public void UpdateAcademicRankToDefault(int currentAcademicRankId, int newAcademicRankId)
        {
            _professors.Where(p => p.AcademicRankId == currentAcademicRankId)
                       .Update(p => new Professor { AcademicRankId = newAcademicRankId });
        }

        public IList<int> GetAllUsersIds()
        {
            return _professors.Select(p => p.UserId).ToList();
        }

        public int GetUserIdByPageId(string pageId)
        {
            return _professors
                .Where(p => p.PageId == pageId)
                .Select(p => p.UserId)
                .Cacheable()
                .Single();
        }

        public string GetResumeFilename(int userId)
        {
            return _professors
                .Where(p => p.UserId == userId)
                .Select(p => p.PersianResumeFileName)
                .SingleOrDefault();
        }

        public void UpdateResumeFilename(int userId, string resumeFilename)
        {
            var professor = _professors.Single(p => p.UserId == userId);

            professor.PersianResumeFileName = resumeFilename;
        }

        public int LessonsCount(string pageId)
        {
            var count = _professors
                .IncludeOptimized(p => p.Lessons)
                .Where(p => p.PageId == pageId)
                .Select(p => new { LessonsCount = p.Lessons.Count() })
                .Cacheable()
                .Single();

            return count.LessonsCount;
        }

        public ProfileTopMenuViewModel ProfileTopMenu(string pageId)
        {
            var details = _professors
                .IncludeOptimized(p => p.UserDetails)
                .IncludeOptimized(p => p.Lessons)
                .IncludeOptimized(p => p.Galleries.Where(g => g.IsActive))
                .Where(p => p.PageId == pageId)
                .Select(p => new { p.UserId, p.UserDetails.FirstName, p.UserDetails.LastName, p.IsActiveWeeklyProgram, HasWeeklyProgram = p.WeeklyPrograms.Any(), p.IsActiveFreePage, p.FreePage, 
                    Lessons = p.Lessons.Select(l => new { LessonId = l.Id, LessonName = l.LessonName, l.Order }).OrderByDescending(l => l.Order).ThenBy(l => l.LessonId),
                    Galleries = p.Galleries.Where(g => g.IsActive).Select(g => new { GalleryId = g.Id, GalleryTitle = g.Title, g.Order }).OrderByDescending(g => g.Order).ThenBy(g => g.GalleryId)
                })
                .Cacheable()
                .Single();

            var profileTopMenu = new ProfileTopMenuViewModel();
            profileTopMenu.Id = details.UserId;
            profileTopMenu.Firstname = details.FirstName;
            profileTopMenu.Lastname = details.LastName;
            profileTopMenu.IsActiveFreePage = details.IsActiveFreePage;
            profileTopMenu.HasFreePage = details.FreePage != null;
            profileTopMenu.IsActiveWeeklyProgram = details.IsActiveWeeklyProgram;
            profileTopMenu.HasWeeklyProgram = details.HasWeeklyProgram;

            foreach (var lesson in details.Lessons)
            {
                profileTopMenu.LessonNames.Add(lesson.LessonId, lesson.LessonName);
            }

            foreach (var gallery in details.Galleries)
            {
                profileTopMenu.GalleryNames.Add(gallery.GalleryId, gallery.GalleryTitle);
            }

            return profileTopMenu;
        }

        public LessonsIndexViewModel LessonsIndex(string pageId)
        {
            var infos = _professors
                .IncludeOptimized(p => p.UserDetails)
                .IncludeOptimized(p => p.Lessons)
                .Where(p => p.PageId == pageId)
                .Select(p => new { p.UserDetails.FirstName, p.UserDetails.LastName, LessonsCount = p.Lessons.Count(), Lessons = p.Lessons })
                .Cacheable()
                .Single();

            var sortedLessons = infos.Lessons.OrderByDescending(i => i.Order).ThenBy(i => i.Id).ToList();
            var lessons = new List<UserLessonsViewModel>();

            foreach (var lesson in sortedLessons)
            {
                lessons.Add(new UserLessonsViewModel
                {
                    AcademicYear = lesson.AcademicYear,
                    Description = lesson.Description,
                    Field = lesson.Field,
                    LessonGrade = lesson.LessonGrade,
                    LessonId = lesson.Id,
                    LessonName = lesson.LessonName,
                    Semester = lesson.Semester,
                    Trend = lesson.Trend,
                    Link = lesson.Link,
                    CreateDate = lesson.CreateDate
                });
            }

            return new LessonsIndexViewModel
            {
                Firstname = infos.FirstName,
                Lastname = infos.LastName,
                LessonsCount = infos.LessonsCount,
                UserLessons = lessons
            };
        }

        public GalleriesIndexViewModel GalleriesIndex(string pageId)
        {
            var infos = _professors
                .IncludeOptimized(p => p.UserDetails)
                .IncludeOptimized(p => p.Galleries.Where(g => g.IsActive))
                .Where(p => p.PageId == pageId)
                .Select(p => new { p.UserDetails.FirstName, p.UserDetails.LastName, GalleriesCount = p.Galleries.Count(g => g.IsActive), Galleries = p.Galleries.Where(g => g.IsActive) })
                .Cacheable()
                .Single();

            var sortedGalleries = infos.Galleries.OrderByDescending(i => i.Order).ThenBy(i => i.Id).ToList();
            //sortedGalleries = sortedGalleries.Except(sortedGalleries.Where(g => !g.IsActive).ToList()).ToList();
            var galleries = new List<UserGalleriesViewModel>();

            foreach (var gallery in sortedGalleries)
            {
                galleries.Add(new UserGalleriesViewModel
                {
                    GalleryId = gallery.Id,
                    Title = gallery.Title,
                    Description = gallery.Description,
                    Link = gallery.Link,
                    CreateDate = gallery.CreateDate
                });
            }

            return new GalleriesIndexViewModel
            {
                Firstname = infos.FirstName,
                Lastname = infos.LastName,
                GalleriessCount = infos.GalleriesCount,
                UserGalleries = galleries
            };
        }

        public ProfessorNameAndIdViewModel NameAndId(string pageId)
        {
            var infos = _professors
                .Where(p => p.PageId == pageId)
                .IncludeOptimized(p => p.UserDetails)
                .Select(p => new
                {
                    p.UserId,
                    p.UserDetails.FirstName,
                    p.UserDetails.LastName
                })
                .Cacheable()
                .Single();

            return new ProfessorNameAndIdViewModel {
                UserId = infos.UserId,
                Firstname = infos.FirstName,
                Lastname = infos.LastName
            };
        }

        public void AddScopusDetails(int userId, int hIndex, int documents, string otherNamesFormat)
        {
            var professor = _professors.Single(u => u.UserId == userId);

            professor.ScopusHIndex = hIndex;
            //professor.ScopusCitations = citations;
            professor.ScopusDocuments = documents;
            //professor.ScopusTotalDocumentsCited = totalDocumentsCited;
            professor.OtherNamesFormat = otherNamesFormat;
        }

        public void AddGoogleDetails(int userId, int hIndex, int citations)
        {
            var professor = _professors.Single(u => u.UserId == userId);

            professor.GoogleHIndex = hIndex;
            professor.GoogleCitations = citations;
        }

        public IEnumerable<ProfessorMembershipViewModel> GetListMemberships(string pageId)
        {
            var professorMemberships = _professors.
                Where(p => p.PageId == pageId).
                IncludeOptimized(p => p.Memberships).
                Select(p => p.Memberships.Select(m => new { m.Id, m.Post, m.CommitteeTitle, m.StartTime, m.EndTime, m.Description, m.Link, m.Order }))
                .Cacheable()
                .Single();

            professorMemberships = professorMemberships.OrderByDescending(m => m.Order).ThenBy(m => m.Id);
            var memberships = new List<ProfessorMembershipViewModel>();

            foreach (var membership in professorMemberships)
            {
                memberships.Add(new ProfessorMembershipViewModel
                {
                    Id = membership.Id,
                    Post = membership.Post,
                    CommitteeTitle = membership.CommitteeTitle,
                    StartTime = membership.StartTime,
                    EndTime = membership.EndTime,
                    Description = membership.Description,
                    Link = membership.Link
                });
            }

            return memberships;
        }

        public UserSettingsViewModel GetSettings(int userId)
        {
            var settings = _professors
                .Where(p => p.UserId == userId)
                .Select(p => new { p.IsActiveBio, p.IsActiveFreePage, p.ShowHIndexSection, p.ShowScopusDocumentsCitationChart, p.ShowGoogleDocumentsCitationChart, p.IsActiveWeeklyProgram })
                .Cacheable()
                .Single();

            return new UserSettingsViewModel
            {
                IsActiveBio = settings.IsActiveBio,
                IsActiveFreePage = settings.IsActiveFreePage,
                ShowHIndexSection = settings.ShowHIndexSection,
                ShowScopusDocumentsCitationChart = settings.ShowScopusDocumentsCitationChart,
                ShowGoogleDocumentsCitationChart = settings.ShowGoogleDocumentsCitationChart,
                IsActiveWeeklyProgram = settings.IsActiveWeeklyProgram
            };
        }

        public void UpdateSettings(int userId, UserSettingsViewModel settings)
        {
            var professor = _professors.Single(p => p.UserId == userId);

            professor.IsActiveBio = settings.IsActiveBio;
            professor.IsActiveFreePage = settings.IsActiveFreePage;
            professor.ShowHIndexSection = settings.ShowHIndexSection;
            professor.ShowScopusDocumentsCitationChart = settings.ShowScopusDocumentsCitationChart;
            professor.ShowGoogleDocumentsCitationChart = settings.ShowGoogleDocumentsCitationChart;
            professor.IsActiveWeeklyProgram = settings.IsActiveWeeklyProgram;
        }

        public IList<HIndexManagementViewModel> GetHIndexList(string lastname, string pageId, string email, int startIndex = 0, int pageSize = 20)
        {
            var hIndexList = new List<HIndexManagementViewModel>();
            var query = _professors
                        .Include(p => p.UserDetails)
                        .AsQueryable();

            if (!string.IsNullOrEmpty(lastname))
            {
                query = query.Where(u => u.UserDetails.LastName.Contains(lastname));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.UserDetails.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(pageId))
            {
                query = query.Where(p => p.PageId.Contains(pageId));
            }

            var hIndexes = query
                            .OrderByDescending(m => m.UserId)
                            .Skip(startIndex)
                            .Take(pageSize)
                            .Cacheable()
                            .ToList();
            foreach (var hIndex in hIndexes)
            {
                hIndexList.Add(new HIndexManagementViewModel
                {
                    GoogleCitations = hIndex.GoogleCitations,
                    GoogleHIndex = hIndex.GoogleHIndex,
                    Email = hIndex.UserDetails.Email,
                    UserId = hIndex.UserId,
                    Firstname = hIndex.UserDetails.FirstName,
                    Lastname = hIndex.UserDetails.LastName,
                    OtherNamesFormat = hIndex.OtherNamesFormat,
                    PageId = hIndex.PageId,
                    //ScopusCitations = hIndex.ScopusCitations,
                    ScopusDocuments = hIndex.ScopusDocuments,
                    ScopusHIndex = hIndex.ScopusHIndex,
                    //ScopusTotalDocumentsCited = hIndex.ScopusTotalDocumentsCited
                });
            }

            return hIndexList;
        }

        public int GetHIndexCount(string lastname, string pageId, string email)
        {
            var query = _professors.AsQueryable();

            if (!string.IsNullOrEmpty(lastname))
            {
                query = query.Where(u => u.UserDetails.LastName.Contains(lastname));
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(u => u.UserDetails.Email.Contains(email));
            }

            if (!string.IsNullOrEmpty(pageId))
            {
                query = query.Where(p => p.PageId.Contains(pageId));
            }

            return query.Cacheable().Count();
        }

        public void UpdateHIndex(int userId, HIndexManagementViewModel hIndex)
        {
            var professor = _professors.Single(p => p.UserId == userId);

            professor.GoogleCitations = hIndex.GoogleCitations;
            professor.GoogleHIndex = hIndex.GoogleHIndex;
            professor.OtherNamesFormat = hIndex.OtherNamesFormat;
            //professor.ScopusCitations = hIndex.ScopusCitations;
            professor.ScopusDocuments = hIndex.ScopusDocuments;
            professor.ScopusHIndex = hIndex.ScopusHIndex;
            //professor.ScopusTotalDocumentsCited = hIndex.ScopusTotalDocumentsCited;

            professor.LastUpdateTime = professor.HIndexAndDocumentCitationLastUpdateTime = DateTime.UtcNow;
        }

        public WeeklyProgramDetailsViewModel GetWeeklyProgramDetails(int userId)
        {
            var programDetails = _professors.Where(p => p.UserId == userId).Cacheable().Single();

            return new WeeklyProgramDetailsViewModel
            {
                StartDate = programDetails.WeeklyProgramStartDate,
                EndDate = programDetails.WeeklyProgramEndDate,
                Description = programDetails.WeeklyProgramDescription
            };
        }

        public void UpdateWeeklyProgramDetails(int userId, WeeklyProgramDetailsViewModel details)
        {
            var professor = _professors.Single(p => p.UserId == userId);

            professor.WeeklyProgramStartDate = details.StartDate;
            professor.WeeklyProgramEndDate = details.EndDate;
            professor.WeeklyProgramDescription = details.Description;

            professor.LastUpdateTime = DateTime.UtcNow;
        }

        public WeeklyProgramIndexViewModel GetWeeklyProgramIndex(string pageId)
        {
            var weeklyProgram = new WeeklyProgramIndexViewModel();
            var infos = _professors
                .Where(p => p.PageId == pageId)
                .IncludeOptimized(p => p.UserDetails)
                .IncludeOptimized(p => p.WeeklyPrograms)
                .Select(p => new
                {
                    p.IsActiveWeeklyProgram,
                    p.WeeklyProgramStartDate,
                    p.WeeklyProgramEndDate,
                    p.WeeklyProgramDescription,
                    p.UserDetails.FirstName,
                    p.UserDetails.LastName,
                    p.WeeklyPrograms
                })
                .Cacheable()
                .Single();

            weeklyProgram.Firstname = infos.FirstName;
            weeklyProgram.Lastname = infos.LastName;
            weeklyProgram.IsActiveWeeklyProgram = infos.IsActiveWeeklyProgram;
            weeklyProgram.StartDate = infos.WeeklyProgramStartDate;
            weeklyProgram.EndDate = infos.WeeklyProgramEndDate;
            weeklyProgram.Description = infos.WeeklyProgramDescription;

            var programsList = infos.WeeklyPrograms
                .OrderBy(wp => wp.DayOfProgram)
                .ThenBy(wp => wp.StartTime)
                .ThenBy(wp => wp.EndTime)
                .ToList();

            foreach (var program in programsList)
            {
                weeklyProgram.WeeklyPrograms.Add(new WeeklyProgramViewModel {
                    Id = program.Id,
                    DayOfProgram = program.DayOfProgram,
                    StartTime = program.StartTime,
                    EndTime = program.EndTime,
                    Description = program.Description
                });
            }

            return weeklyProgram;
        }

        public ProfessorDashboardStatisticViewModel GetDashboardStatistics(int userId)
        {
            var statistics = _professors
                .Where(p => p.UserId == userId)
                .Select(p => new
                {
                    LessonCount = p.Lessons.Count(),
                    GalleryCount = p.Galleries.Count(),
                    ExternalArticleCount = p.ExternalResearchRecords.Count(),
                    ExternalSeminarCount = p.ExternalSeminarRecords.Count()
                })
                .Cacheable()
                .Single();

            return new ProfessorDashboardStatisticViewModel
            {
                NumberOfLessons = statistics.LessonCount,
                NumberOfGalleries = statistics.GalleryCount,
                NumberOfExternalArticles = statistics.ExternalArticleCount,
                NumberOfExternalSeminars = statistics.ExternalSeminarCount
            };
        }

        public DateTime? GetLastExternalResearchUpdateTime(string pageId)
        {
            return _professors
                .Where(p => p.PageId == pageId)
                .Select(p => p.ExternalResearchLastUpdateTime)
                .Single();
        }

        public void UpdateUserHIndexInfos(int userId, int? ScopusHIndex, int? ScopusDocuments, string OtherNamesFormat, int? GoogleHIndex, int? GoogleCitations, bool updateScopus, bool updateGoogle)
        {
            var professor = _professors.Single(p => p.UserId == userId);

            if (updateScopus)
            {
                professor.ScopusHIndex = ScopusHIndex;
                professor.ScopusDocuments = ScopusDocuments;
                professor.OtherNamesFormat = OtherNamesFormat;
            }

            if (updateGoogle)
            {
                professor.GoogleHIndex = GoogleHIndex;
                professor.GoogleCitations = GoogleCitations;
            }
        }

        public Tuple<string, string> GetOrcidAndResearcherId(int userId)
        {
            var ids = _professors
                .Where(p => p.UserId == userId)
                .Select(p => new { p.OrcidId, p.ResearcherId })
                .Single();

            return new Tuple<string, string>(ids.OrcidId, ids.ResearcherId);
        }

        public bool IsExistProfessor(string pageId)
        {
            return _professors.Any(p => p.PageId == pageId);
        }

        public bool ExistProfessor(string secondaryEmail)
        {
            return _professors.Any(p => p.SecondaryEmails.Contains(secondaryEmail));
        }

        public int DisableUsersCount()
        {
            return _professors.Count(p => p.IsSoftDelete == true && p.SoftDeleteDate == null);
        }

        public void SetSoftDelete(int userId, bool state)
        {
            var professor = _professors.Single(p => p.UserId == userId);

            professor.IsSoftDelete = state;

            if (state)
            {
                professor.SoftDeleteDate = DateTime.UtcNow;
            }
        }

        public bool IsSoftDeleted(int userId)
        {
            return _professors.Where(p => p.UserId == userId).Select(p => p.IsSoftDelete).Single();
        }

        //public IEnumerable<ExternalResearchRecordViewModel> GetProfessorExternalResearchs(string pageId, int pageIndex = 0, int pageSize = 20)
        //{
        //    var skipRecords = pageIndex * pageSize;
        //    var researchList = new List<ExternalResearchRecordViewModel>();
        //    var researchs = _professors
        //        .Where(p => p.PageId == pageId)
        //        .Include(p => p.ExternalResearchRecords)
        //        //.OrderByDescending(s => s.ExternalResearchRecords.Order)
        //        //.ThenBy(s => s.Id)
        //        .Skip(skipRecords)
        //        .Take(pageSize)
        //        .Cacheable()
        //        .ToList();

        //    foreach (var research in researchs)
        //    {
        //        researchList.Add(new ExternalResearchRecordViewModel
        //        {
        //            Id = research.Id,
        //            Abstract = research.Abstract,

        //            Authors = research.Authors,
        //            Journal = research.Journal,
        //            Doi = research.Doi,
        //            Issue = research.Issue,
        //            Pages = research.Pages,
        //            Volume = research.Volume,
        //            TimesCited = research.TimesCited,
        //            Year = research.Year,
        //            Description = research.Description,
        //            Filename = research.Filename,
        //            Title = research.Title,
        //            Link = research.Link,
        //            Order = research.Order
        //        });
        //    }

        //    return researchList;
        //}

        //public IEnumerable<ExternalSeminarRecordViewModel> GetProfessorExternalSeminars(string pageId, int pageIndex = 0, int pageSize = 20)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<InternalResearchRecordViewModel> GetProfessorInternalResearchs(string pageId, int pageIndex = 0, int pageSize = 20)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<InternalSeminarRecordViewModel> GetProfessorInternalSeminars(string pageId, int pageIndex = 0, int pageSize = 20)
        //{
        //    throw new NotImplementedException();
        //}
    }
}

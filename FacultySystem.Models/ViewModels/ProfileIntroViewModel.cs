using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class ProfileIntroViewModel
    {
        public int UserId { get; set; }
        public string AvatarName { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string CommonAuthorPaperName { get; set; }
        public string Bio { get; set; }
        public string SecondaryEmails { get; set; }
        public string College { get; set; }
        public Sex Sex { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string AcademicRank { get; set; }
        public string EducationalDegree { get; set; }
        public string EducationalGroup { get; set; }
        public string Mobile { get; set; }
        public string PersianResumeFilename { get; set; }
        public string Location { get; set; }
        public string ResearchFields { get; set; }
        public string Interests { get; set; }
        public string PersonalWebPage { get; set; }
        public string ScopusId { get; set; }
        public string OrcidId { get; set; }
        public string ResearchGateId { get; set; }
        public string GoogleScholarId { get; set; }
        public string ResearcherId { get; set; }
        public string PubMedId { get; set; }
        public string MedLibId { get; set; }
        public string BirthPlace { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public bool IsActiveBio { get; set; }
        public string FreePage { get; set; }
        public bool IsActiveFreePage { get; set; }
        public bool IsActiveWeeklyProgram { get; set; }
        public int? ScopusHIndex { get; set; }
        public int? GoogleHIndex { get; set; }
        //public int? ScopusCitations { get; set; }
        public int? ScopusDocuments { get; set; }
        //public int? ScopusTotalDocumentsCited { get; set; }
        public int? GoogleCitations { get; set; }
        public string OtherNamesFormat { get; set; }
        public bool ShowScopusDocumentsCitationChart { get; set; }
        public bool ShowGoogleDocumentsCitationChart { get; set; }
        public bool ShowHIndexSection { get; set; }
        public DateTime? HIndexAndDocumentCitationLastUpdateTime { get; set; }
        public IEnumerable<SectionsOrderViewModel> SectionsOrder { get; set; }

        public bool HasAddress { get; set; }
        public bool HasTrainingRecord { get; set; }
        public bool HasResearchRecord { get; set; }
        public bool HasStudingRecord { get; set; }
        public bool HasAdministrationRecord { get; set; }
        public bool HasInternalResearchRecord { get; set; }
        public bool HasExternalResearchRecord { get; set; }
        public bool HasInternalSeminarRecord { get; set; }
        public bool HasExternalSeminarRecord { get; set; }
        public bool HasThesis { get; set; }
        public bool HasCourseAndWorkshop { get; set; }
        public bool HasHonor { get; set; }
        public bool HasPublication { get; set; }
        public bool HasFreeField { get; set; }
        public bool HasLanguage { get; set; }
        public bool HasLesson { get; set; }
        public bool HasMembership { get; set; }
        public bool HasGallery { get; set; }
        public bool HasWeeklyProgram { get; set; }
        public bool HasScopusChart { get; set; }
        public bool HasGoogleChart { get; set; }

        public bool HasHIndexInformations
        {
            get
            {
                return ScopusHIndex.HasValue ||
                       //ScopusCitations.HasValue ||
                       ScopusDocuments.HasValue ||
                       //ScopusTotalDocumentsCited.HasValue ||
                       GoogleHIndex.HasValue ||
                       GoogleCitations.HasValue ||
                       !string.IsNullOrEmpty(OtherNamesFormat);
            }
        }

        public string Fullname
        {
            get
            {
                if (!string.IsNullOrEmpty(Firstname) && !string.IsNullOrEmpty(Lastname))
                {
                    return $"{Firstname} {Lastname}";
                }

                if (!string.IsNullOrEmpty(Firstname))
                {
                    return Firstname;
                }

                if (!string.IsNullOrEmpty(Lastname))
                {
                    return Lastname;
                }

                return null;
            }
        }

        public List<string> SecondaryEmailsList
        {
            get
            {
                var emailsList = new List<string>();
                var emails = SecondaryEmails.Split(',');

                foreach (var email in emails)
                {
                    emailsList.Add(email.Trim().ToLowerInvariant());
                }

                return emailsList;
            }
        }

        public List<string> ResearchFieldsList
        {
            get
            {
                var researchFieldsList = new List<string>();
                var researchFields = ResearchFields.Split(',');

                foreach (var researchField in researchFields)
                {
                    researchFieldsList.Add(researchField.Trim());
                }

                return researchFieldsList;
            }
        }

        public List<string> InterestsList
        {
            get
            {
                var interestsList = new List<string>();
                var interests = Interests.Split(',');

                foreach (var interest in interests)
                {
                    interestsList.Add(interest.Trim());
                }

                return interestsList;
            }
        }

        //public string CollegeText
        //{
        //    get
        //    {
        //        return EnumExtensions.GetDescription(College);
        //    }
        //}

        public string SexText
        {
            get
            {
                return EnumExtensions.GetDescription(Sex);
            }
        }

        public string MaritalStatusText
        {
            get
            {
                return EnumExtensions.GetDescription(MaritalStatus);
            }
        }

        //public string EducationalDegreeText
        //{
        //    get
        //    {
        //        return EnumExtensions.GetDescription(EducationalDegree);
        //    }
        //}

        //public string EducationalGroupText
        //{
        //    get
        //    {
        //        return EnumExtensions.GetDescription(EducationalGroup);
        //    }
        //}

        //public string AcademicRankText
        //{
        //    get
        //    {
        //        return EnumExtensions.GetDescription(AcademicRank);
        //    }
        //}

        public string GetRelativeLastUpdateTime
        {
            get
            {
                if (LastUpdateTime == null)
                {
                    return string.Empty;
                }

                return (LastUpdateTime ?? DateTime.UtcNow).UtcToLocalDateTime().CalculateRelativeTime();
            }
        }

        public string GetRelativeHIndexAndCitationLastUpdateTime
        {
            get
            {
                if (HIndexAndDocumentCitationLastUpdateTime == null)
                {
                    return string.Empty;
                }

                return (HIndexAndDocumentCitationLastUpdateTime ?? DateTime.UtcNow).UtcToLocalDateTime().CalculateRelativeTime();
            }
        }

        public string JalaiLastUpdateTime
        {
            get
            {
                var persianDate = "";

                if (LastUpdateTime == null)
                {
                    return persianDate;
                }

                Persia.SolarDate solarDate = Persia.Calendar.ConvertToPersian(((System.DateTime)LastUpdateTime).UtcToLocalDateTime());
                return $"{solarDate.ToString("M")} <span dir=\"ltr\">{solarDate.ToString("R")}</span>";
            }
        }

        public string JalaliBirthDate
        {
            get
            {
                var persianDate = "";

                if (BirthDate == null)
                {
                    return persianDate;
                }

                PersianCalendar pc = new PersianCalendar();
                persianDate = string.Format("{0}/{1}/{2}", pc.GetYear((System.DateTime)BirthDate), pc.GetMonth((System.DateTime)BirthDate), pc.GetDayOfMonth((System.DateTime)BirthDate));
                return persianDate;
            }
        }
    }
}

using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ContentManagementSystem.Models.ViewModels
{
    public class DetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CommonAuthorPaperName { get; set; }
        public string SecondaryEmails { get; set; }
        public int College { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public Sex Sex { get; set; }
        public int AcademicRank { get; set; }
        public int EducationalDegree { get; set; }
        public int EducationalGroup { get; set; }
        public string Mobile { get; set; }
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
        public IEnumerable<CollegeViewModel> Colleges { get; set; }
        public IEnumerable<EducationalGroupViewModel> Groups { get; set; }
        public IEnumerable<EducationalDegreeViewModel> Degrees { get; set; }
        public IEnumerable<AcademicRankViewModel> Ranks { get; set; }

        public List<SelectListItem> GetCollegeList()
        {
            var listItems = new List<SelectListItem>();
            foreach (var item in Colleges)
            {
                listItems.Add(new SelectListItem
                {
                    Selected = item.Id == College,
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }

            //listItems.RemoveAt(0); // Remove NotDefined
            return listItems;
        }

        public List<SelectListItem> GetSexList()
        {
            var items = EnumExtensions.EnumToList<Sex>();
            var listItems = new List<SelectListItem>();
            foreach (var item in items)
            {
                listItems.Add(new SelectListItem
                {
                    Selected = item.stringValueMember == Sex.ToString(),
                    Text = item.DisplayMember,
                    Value = item.intValueMember.ToString()
                });
            }

            return listItems;
        }

        public List<SelectListItem> GetMaritalStatusList()
        {
            var items = EnumExtensions.EnumToList<MaritalStatus>();
            var listItems = new List<SelectListItem>();
            foreach (var item in items)
            {
                listItems.Add(new SelectListItem
                {
                    Selected = item.stringValueMember == MaritalStatus.ToString(),
                    Text = item.DisplayMember,
                    Value = item.intValueMember.ToString()
                });
            }

            return listItems;
        }

        public List<SelectListItem> GetAcademicRankList()
        {
            var listItems = new List<SelectListItem>();
            foreach (var item in Ranks)
            {
                listItems.Add(new SelectListItem
                {
                    Selected = item.Id == AcademicRank,
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }

            return listItems;
        }

        public List<SelectListItem> GetEducationalDegreeList()
        {
            var listItems = new List<SelectListItem>();
            foreach (var item in Degrees)
            {
                listItems.Add(new SelectListItem
                {
                    Selected = item.Id == EducationalDegree,
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }

            return listItems;
        }

        public List<SelectListItem> GetEducationalGroupList()
        {
            var listItems = new List<SelectListItem>();
            foreach (var item in Groups)
            {
                listItems.Add(new SelectListItem
                {
                    Selected = item.Id == EducationalGroup,
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }

            //listItems.RemoveAt(0); // Remove NotDefined
            return listItems;
        }

        public string GetPersianBirthDate()
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

using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.Utils;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ContentManagementSystem.Models.ViewModels
{
    public class IndexUsersListViewModel
    {
        //public int Id { set; get; }
        public string PageId { set; get; }
        public string Firstname { set; get; }
        public string Lastname { get; set; }
        public string AvatarName { get; set; }
        public string Email { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string College { get; set; }
        public string EducationalDegree { get; set; }
        public string EducationalGroup { get; set; }
        //public IEnumerable<CollegeViewModel> Colleges { get; set; }
        //public IEnumerable<EducationalGroupViewModel> Groups { get; set; }


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

        //public string CollegeText
        //{
        //    get
        //    {
        //        return EnumExtensions.GetDescription(College);
        //    }
        //}

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

        //public List<SelectListItem> GetEducationalGroupList()
        //{
        //    var listItems = new List<SelectListItem>();
        //    foreach (var item in Groups)
        //    {
        //        listItems.Add(new SelectListItem
        //        {
        //            Selected = item.Name == "--",
        //            Text = item.Name,
        //            Value = item.Id.ToString()
        //        });
        //    }

        //    //listItems.RemoveAt(0); // Remove NotDefined
        //    return listItems;
        //}

        //public List<SelectListItem> GetCollegeList()
        //{
        //    var listItems = new List<SelectListItem>();
        //    foreach (var item in Colleges)
        //    {
        //        listItems.Add(new SelectListItem
        //        {
        //            Selected = item.Name == "--",
        //            Text = item.Name,
        //            Value = item.Id.ToString()
        //        });
        //    }

        //    //listItems.RemoveAt(0); // Remove NotDefined
        //    return listItems;
        //}
    }
}

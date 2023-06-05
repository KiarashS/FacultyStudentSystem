using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.Utils;
using System;
using System.Globalization;

namespace ContentManagementSystem.Models.ViewModels
{
    public class HIndexManagementViewModel
    {
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PageId { get; set; }
        public string Email { get; set; }
        public int? ScopusHIndex { get; set; }
        public int? GoogleHIndex { get; set; }
        //public int? ScopusCitations { get; set; }
        public int? ScopusDocuments { get; set; }
        //public int? ScopusTotalDocumentsCited { get; set; }
        public int? GoogleCitations { get; set; }
        public string OtherNamesFormat { get; set; }

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
    }
}

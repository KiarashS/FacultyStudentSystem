using ContentManagementSystem.DomainClasses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentManagementSystem.Models.Utils;

namespace ContentManagementSystem.Models.ViewModels
{
    public class UserListViewModel
    {
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PageId { get; set; }
        public bool IsSoftDelete { get; set; }
        public bool IsBanned { get; set; }
        public DateTime? BannedDate { get; set; }
        public string BannedReason { get; set; }
        public Dictionary<string, string> Roles { get; set; }

        public string PersianBannedDate
        {
            get
            {
                var persianDate = "-";

                if (BannedDate == null)
                {
                    return persianDate;
                }

                PersianCalendar pc = new PersianCalendar();
                var localDate = ((DateTime)BannedDate).UtcToLocalDateTime();
                persianDate = string.Format("{0}/{1}/{2}", pc.GetYear(localDate), pc.GetMonth(localDate), pc.GetDayOfMonth(localDate));
                return persianDate;
            }
        }

        public string Fullname
        {
            get { return $"{Firstname} {Lastname}"; }
        }

    }
}

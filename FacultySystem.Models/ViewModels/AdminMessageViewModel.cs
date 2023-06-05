using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.Utils;
using System;
using System.Globalization;

namespace ContentManagementSystem.Models.ViewModels
{
    public class AdminMessageViewModel
    {
        public AdminMessageViewModel()
        {
            CreateDate = DateTime.UtcNow;
        }

        public long Id { get; set; }
        public int UserId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PageId { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ReplyContent { get; set; }
        public AdminMessageState State { get; set; }
        public DateTime CreateDate { get; set; }

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

        public string StateText
        {
            get
            {
                return EnumExtensions.GetDescription(State);
            }
        }

        public string CreateDateText
        {
            get
            {
                var persianDate = "-";
                PersianCalendar pc = new PersianCalendar();
                var localDate = ((DateTime)CreateDate).UtcToLocalDateTime();
                persianDate = string.Format("{0}/{1}/{2}", pc.GetYear(localDate), pc.GetMonth(localDate), pc.GetDayOfMonth(localDate));
                return persianDate;
            }
        }
    }
}

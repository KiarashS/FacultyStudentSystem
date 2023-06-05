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
    public class GalleriesIndexViewModel
    {
        public int GalleriessCount { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public IList<UserGalleriesViewModel> UserGalleries { get; set; }

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

    public class UserGalleriesViewModel
    {
        public long GalleryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string Link { get; set; }

        public string UrlSlug
        {
            get
            {
                return Utils.Extensions.GenerateSlug(Title);
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

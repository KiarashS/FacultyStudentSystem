using ContentManagementSystem.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ContentManagementSystem.Models.ViewModels
{
    public class FreePageViewModel
    {
        [AllowHtml]
        public string FreePageText { get; set; }
        public bool IsActiveFreePage { get; set; }
        public DateTime? FreePageLastUpdateTime { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string LastUpdateTimeText
        {
            get
            {
                if (FreePageLastUpdateTime == null)
                {
                    return null;
                }

                Persia.SolarDate solarDate = Persia.Calendar.ConvertToPersian(((System.DateTime)FreePageLastUpdateTime).UtcToLocalDateTime());
                return $"{solarDate.ToString("M")} <span dir=\"ltr\">{solarDate.ToString("R")}</span>";
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
    }
}

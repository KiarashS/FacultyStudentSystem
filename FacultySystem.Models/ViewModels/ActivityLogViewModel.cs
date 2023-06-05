using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.Utils;
using System;

namespace ContentManagementSystem.Models.ViewModels
{
    public class ActivityLogViewModel
    {
        public long Id { get; set; }
        public string SourceAddress { get; set; }
        public string ActionBy { get; set; }
        public string ActionType { get; set; }
        public string Message { get; set; }
        public ActionLevel ActionLevel { get; set; }
        public DateTime ActionDate { get; set; }
        public string Url { get; set; }

        public string ActionDateText
        {
            get
            {
                Persia.SolarDate solarDate = Persia.Calendar.ConvertToPersian(((System.DateTime)ActionDate).UtcToLocalDateTime());
                return $"{solarDate.ToString("M")} <span dir=\"ltr\">{solarDate.ToString("R")}</span>";
            }
        }
    }
}

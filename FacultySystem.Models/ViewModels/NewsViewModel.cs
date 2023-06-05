using ContentManagementSystem.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class NewsViewModel
    {
        public NewsViewModel()
        {
            CreateDate = DateTimeOffset.UtcNow;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset CreateDate { get; set; }

        public string CreateDateText
        {
            get
            {
                Persia.SolarDate solarDate = Persia.Calendar.ConvertToPersian(((DateTimeOffset)CreateDate).DateTime.UtcToLocalDateTime());
                return $"{solarDate.ToString("M")} <span dir=\"ltr\">{solarDate.ToString("R")}</span>";
            }
        }
    }
}

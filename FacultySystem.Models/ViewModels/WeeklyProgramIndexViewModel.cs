using System.Collections.Generic;
using ContentManagementSystem.Models.Utils;

namespace ContentManagementSystem.Models.ViewModels
{
    public class WeeklyProgramIndexViewModel
    {
        public WeeklyProgramIndexViewModel()
        {
            WeeklyPrograms = new List<WeeklyProgramViewModel>();
        }

        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Description { get; set; }
        public bool IsActiveWeeklyProgram { get; set; }
        public IList<WeeklyProgramViewModel> WeeklyPrograms { get; set; }

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

        public string FullTime
        {
            get
            {
                if (StartDate != null && EndDate == null)
                {
                    return $"از تاریخ {StartDate}";
                }

                if (StartDate == null && EndDate != null)
                {
                    return $"تا تاریخ {EndDate}";
                }

                if (StartDate != null && EndDate != null)
                {
                    return $"از تاریخ {StartDate} تا تاریخ {EndDate}";
                }

                return null;
            }
        }
    }
}

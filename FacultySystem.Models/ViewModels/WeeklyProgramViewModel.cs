using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class WeeklyProgramViewModel
    {
        public long Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public DayOfProgram DayOfProgram { get; set; }

        public string DayOfProgramText
        {
            get
            {
                return EnumExtensions.GetDescription(DayOfProgram);
            }
        }

        public string FullTime
        {
            get
            {
                return $"{StartTime} - {EndTime}";
            }
        }

        public string DescriptionSummary(int length)
        {
            if (string.IsNullOrEmpty(Description))
            {
                return null;
            }

            return Description.TruncateAtWord(length);
        }
    }
}

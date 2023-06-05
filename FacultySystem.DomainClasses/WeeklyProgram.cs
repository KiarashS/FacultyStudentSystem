﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class WeeklyProgram: DomainClassBase
    {
        public long Id { get; set; }
        [ForeignKey("ProfessorDetails")]
        public int ProfessorId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Description { get; set; }
        public DayOfProgram DayOfProgram { get; set; }

        #region Navigations
        public virtual Professor ProfessorDetails { get; set; }
        #endregion
    }

    public enum DayOfProgram: byte
    {
        [Description("شنبه")]
        Saturday = 1,
        [Description("یکشنبه")]
        Sunday = 2,
        [Description("دوشنبه")]
        Monday = 3,
        [Description("سه شنبه")]
        Tuesday = 4,
        [Description("چهارشنبه")]
        Wednesday = 5,
        [Description("پنجشنبه")]
        Thursday = 6,
        [Description("جمعه")]
        Friday = 7
    }
}

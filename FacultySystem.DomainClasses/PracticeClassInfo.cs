﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class PracticeClassInfo : DomainClassBase
    {
        public PracticeClassInfo()
        {
            CreateDate = DateTime.UtcNow;
        }

        public long Id { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string TeacherName { get; set; }
        public PracticeClassDay PracticeClassDay { get; set; }
        [ForeignKey("ProfessorDetails")]
        public int ProfessorId { get; set; }
        [ForeignKey("LessonDetails")]
        public long LessonId { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        #region Navigations
        public virtual Professor ProfessorDetails { get; set; }
        public virtual Lesson LessonDetails { get; set; }
        #endregion
    }

    public enum PracticeClassDay : byte
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
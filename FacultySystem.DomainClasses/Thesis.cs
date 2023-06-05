using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class Thesis : DomainClassBase
    {
        public Thesis()
        {
            ThesisPost = ThesisPost.NotDefined;
            ThesisState = ThesisState.Doing;
        }

        public long Id { get; set; }
        public int ProfessorId { get; set; }
        public string Title { get; set; }
        public ThesisState ThesisState { get; set; }
        public ThesisPost ThesisPost { get; set; }
        public string Executers { get; set; }
        public ThesisGrade ThesisGrade { get; set; }
        public ThesisType ThesisType { get; set; }
        public int? Time { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }
        public string University { get; set; }

        [ForeignKey("ProfessorId")]
        public virtual Professor ProfessorProfile { get; set; }
    }

    public enum ThesisPost: byte
    {
        [Description("--")]
        NotDefined = 1,
        [Description("استاد راهنما")]
        Supervisor = 2,
        [Description("استاد مشاور")]
        Advisor = 3
    }

    public enum ThesisState : byte
    {
        //[Description("--")]
        //NotDefined = 1,
        [Description("در حال انجام")]
        Doing = 1,
        [Description("پایان یافته")]
        Finished = 2
    }

    public enum ThesisGrade : byte
    {
        [Description("--")]
        NotDefined = 1,
        [Description("کاردانی")]
        Kardani = 2,
        [Description("کارشناسی")]
        Karshenasi = 3,
        [Description("پزشکی عمومی")]
        PezeshkiOmoomi = 4,
        [Description("کارشناسی ارشد")]
        KarshenasiArshad = 5,
        [Description("دکترا")]
        Doctora = 6,
        [Description("دکترای تخصصی پزشکی")]
        DrTakhassosiPezeshki = 7,
        [Description("دکترای حرفه ای")]
        DoctorayeHerfeei = 8,
        [Description("دکترای فوق تخصصی بالینی")]
        DrTakhassosiBalini = 9,
        [Description("دکترای تکمیلی تخصصی (فلوشیپ)")]
        Fellowship = 10,
        [Description("دکترای تخصصی دندانپزشکی")]
        DrTakhassosiDandanpezeshki = 11,
        [Description("دکترای تخصصی (PhD) داروسازی")]
        DrTakhassosiDaroosazi = 12,
        [Description("دکترای تخصصی (PhD)")]
        DrTakhassosi = 13,
        [Description("دستیاری تخصصی (علوم پایه پزشکی، داروسازی و دندانپزشکی)")]
        DastyariTakhassosi = 14,
        [Description("دستیاری تخصصی بالینی")]
        DastyariTakhassosiBalini = 15,
        [Description("پسادکترا")]
        PasaDoctora = 16,
        [Description("دوره MPH")]
        Mph = 17,
        [Description("دانشوری")]
        Daneshvari = 18
    }

    public enum ThesisType : byte
    {
        [Description("--")]
        NotDefined = 1,
        [Description("تحقیقاتی")]
        Tahghighati = 2,
        [Description("کاربردی")]
        Karbordi = 3,
        [Description("تحقیقاتی-کاربردی")]
        TahghighatiKarbordi = 4,
        [Description("بنیادی")]
        Bonyadi = 5,
        [Description("توسعه ای")]
        Toseei = 6,
        //[Description("صنعتی")]
        //Sanati = 7
    }
}

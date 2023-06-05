using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class GalleryItem : DomainClassBase
    {
        public GalleryItem()
        {
            CreateDate = DateTime.UtcNow;
        }

        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string MediaFilename { get; set; }
        //public string MediaLink { get; set; }
        //public string MediaDate { get; set; }
        public MediaType MediaType { get; set; }
        [ForeignKey("ProfessorDetails")]
        public int ProfessorId { get; set; }
        [ForeignKey("GalleryDetails")]
        public long GalleryId { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        #region Navigations
        public virtual Professor ProfessorDetails { get; set; }
        public virtual Gallery GalleryDetails { get; set; }
        #endregion
    }

    public enum MediaType : byte
    {
        [Description("--")]
        NotDefined = 1,
        [Description("تصویر")]
        Image = 2,
        [Description("ویدئو")]
        Video = 3,
        [Description("فایل صوتی")]
        Audio = 4,
        [Description("غیره")]
        Etcetera = 5
    }
}

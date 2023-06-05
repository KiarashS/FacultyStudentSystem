using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.DomainClasses
{
    public class Gallery : DomainClassBase
    {
        public Gallery()
        {
            CreateDate = DateTime.UtcNow;
            IsActive = true;
        }

        public long Id { get; set; }
        [ForeignKey("ProfessorDetails")]
        public int ProfessorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        //public string CoverFilename { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        #region Navigations
        public virtual Professor ProfessorDetails { get; set; }
        public virtual ICollection<GalleryItem> GalleryItems { get; set; }
        #endregion
    }
}

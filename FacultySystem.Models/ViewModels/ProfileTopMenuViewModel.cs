using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class ProfileTopMenuViewModel
    {
        public ProfileTopMenuViewModel()
        {
            LessonNames = new Dictionary<long, string>();
            GalleryNames = new Dictionary<long, string>();
        }

        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool IsActiveWeeklyProgram { get; set; }
        public bool HasWeeklyProgram { get; set; }
        public bool IsActiveFreePage { get; set; }
        public bool HasFreePage { get; set; }
        public Dictionary<long, string> LessonNames { get; set; }
        public Dictionary<long, string> GalleryNames { get; set; }

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

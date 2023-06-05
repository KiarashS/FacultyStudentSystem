using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.ViewModels
{
    public class LanguageViewModel
    {
        public long Id { get; set; }
        public ProfessorLanguageName Name { get; set; }
        public ProfessorLanguageLevel Level { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public int? Order { get; set; }

        public string NameText
        {
            get
            {
                return EnumExtensions.GetDescription(Name);
            }
        }

        public string LevelText
        {
            get
            {
                return EnumExtensions.GetDescription(Level);
            }
        }

        public string CssClassText
        {
            get
            {
                return DomainEnumExtensions.CssClass(Name);
            }
        }
    }
}

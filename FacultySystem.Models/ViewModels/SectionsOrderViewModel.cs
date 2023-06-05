using ContentManagementSystem.DomainClasses;
using ContentManagementSystem.Models.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ContentManagementSystem.Models.ViewModels
{
    public class SectionsOrderViewModel
    {
        public long Id { get; set; }
        public SectionName SectionName { get; set; }
        public int Order { get; set; }

        public string SectionNameText
        {
            get
            {
                return EnumExtensions.GetDescription(SectionName);
            }
        }
    }
}

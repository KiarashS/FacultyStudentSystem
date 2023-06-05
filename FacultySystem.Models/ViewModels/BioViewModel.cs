using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ContentManagementSystem.Models.ViewModels
{
    public class BioViewModel
    {
        [AllowHtml]
        public string BioText { get; set; }
        public bool IsActiveBio { get; set; }
    }
}

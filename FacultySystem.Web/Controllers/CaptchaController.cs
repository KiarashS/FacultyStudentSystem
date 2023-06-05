using ContentManagementSystem.Commons.Web.Captcha;
using ContentManagementSystem.Commons.Web.Filters;
using System.Web.Mvc;
using System.Web.UI;

namespace ContentManagementSystem.Web.Controllers
{
    public partial class CaptchaController : BaseController
    {
        [NoBrowserCache]
        [OutputCache(Location = OutputCacheLocation.None, NoStore = true, Duration = 0, VaryByParam = "None")]
        public virtual CaptchaImageResult CaptchaImage(string seed)
        {
            return new CaptchaImageResult();
        }
    }
}
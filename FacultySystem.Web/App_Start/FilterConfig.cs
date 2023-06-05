using ContentManagementSystem.Commons.Web.Filters;
using System.Configuration;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new ElmahHandledErrorLoggerFilter());
            //filters.Add(new HandleErrorAttribute());
            //filters.Add(new ForceWww(ConfigurationManager.AppSettings["SystemRootUrl"]));
            filters.Add(new ElmahRequestValidationErrorFilter());
        }
    }
}

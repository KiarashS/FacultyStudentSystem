using System;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Routing;
using ContentManagementSystem.IocConfig;
using ContentManagementSystem.ServiceLayer.Contracts;
using System.Linq;

namespace ContentManagementSystem.Web.Utils
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidatePageIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }

            var pageId = (string)filterContext.RouteData.Values["pageId"];

            if (string.IsNullOrEmpty(pageId))
            {
                filterContext.HttpContext.Response.StatusCode = 404;
                filterContext.Result = new System.Web.Mvc.RedirectResult("/error/notfound?aspxerrorpath=" + filterContext.HttpContext.Request.Url.PathAndQuery);
                //filterContext.HttpContext.Response.Redirect("/error/notfound?aspxerrorpath=" + filterContext.HttpContext.Request.Url.PathAndQuery, true);
            }

            var professorService = SmObjectFactory.Container.GetInstance<IProfessorService>();
            var pageValidation = professorService.IsValidPageId(pageId);

            if (!pageValidation.IsValidPageId ||
                pageValidation.IsSoftDelete ||
                (!pageValidation.IsSoftDelete && pageValidation.Roles.Contains(ConstantsUtil.AdminRole) && !pageValidation.Roles.Contains(ConstantsUtil.ProfessorRole)) ||
                string.IsNullOrEmpty(pageValidation.PageId))
            {
                filterContext.HttpContext.Response.StatusCode = 404;
                filterContext.Result = new System.Web.Mvc.RedirectResult("/error/notfound?aspxerrorpath=" + filterContext.HttpContext.Request.Url.PathAndQuery);
                //filterContext.HttpContext.Response.Redirect("/error/notfound?aspxerrorpath=" + filterContext.HttpContext.Request.Url.PathAndQuery, true);
            }
        }
    }
}
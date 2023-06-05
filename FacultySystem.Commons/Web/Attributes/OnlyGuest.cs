using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace ContentManagementSystem.Commons.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OnlyGuest : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult
                (
                    new RouteValueDictionary
                        (
                            new
                            {
                                controller = "Home",
                                action = "Index"
                            }
                        )
                );
            }
        }
    }
}
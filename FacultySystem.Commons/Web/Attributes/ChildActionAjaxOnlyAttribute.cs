using System;
using System.Web.Mvc;

namespace ContentManagementSystem.Commons.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ChildActionAjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var parentContext = filterContext.Controller.ControllerContext;
            if ((filterContext.HttpContext.Request.IsAjaxRequest() && string.Equals("post", filterContext.HttpContext.Request.RequestType, StringComparison.OrdinalIgnoreCase)) || filterContext.IsChildAction)
            {
                base.OnActionExecuting(filterContext);
            }
            else
            {
                throw new InvalidOperationException("This operation can only be accessed via Ajax requests");
            }
        }
    }
}
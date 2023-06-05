using System;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;

namespace ContentManagementSystem.Commons.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class SiteAuthorizeAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                throw new UnauthorizedAccessException(); //to avoid multiple redirects
            }
            else
            {
                HandleAjaxRequest(filterContext);
                base.HandleUnauthorizedRequest(filterContext);
            }
        }

        private static void HandleAjaxRequest(AuthorizationContext filterContext)
        {
            var ctx = filterContext.HttpContext;
            if (!ctx.Request.IsAjaxRequest())
                return;

            filterContext.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            //ctx.Response.StatusCode = (int)HttpStatusCode.Forbidden; //Error: Server cannot set status after HTTP headers have been sent.
            //ctx.Response.End();
        }
    }
}
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ContentManagementSystem.Commons.Web.Attributes
{
    public class ValidateJsonAntiForgeryTokenAttribute : ActionFilterAttribute
    {
        //public override void OnActionExecuting(HttpActionContext actionContext)
        //{
        //    try
        //    {
        //        var cookieName = AntiForgeryConfig.CookieName;
        //        var headers = actionContext.Request.Headers;
        //        var cookie = headers
        //            .GetCookies()
        //            .Select(c => c[AntiForgeryConfig.CookieName])
        //            .FirstOrDefault();
        //        var rvt = headers.GetValues("__RequestVerificationToken").FirstOrDefault();
        //        AntiForgery.Validate(cookie != null ? cookie.Value : null, rvt);
        //    }
        //    catch
        //    {
        //        actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Unauthorized request.");
        //    }
        //}
    }
}
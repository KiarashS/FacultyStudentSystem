using System.Web;
using System.Web.Mvc;
using Elmah;
using StackExchange.Exceptional;

namespace ContentManagementSystem.Commons.Web.Filters
{
    public class ElmahRequestValidationErrorFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is HttpRequestValidationException)
                ErrorStore.LogException(context.Exception, HttpContext.Current, true, true);
                //ErrorLog.GetDefault(HttpContext.Current).Log(new Error(context.Exception));
        }
    }
}
using System;
using System.Web;
using System.Web.Mvc;
using Elmah;
using StackExchange.Exceptional;

namespace ContentManagementSystem.Commons.Web.Filters
{
    public class ElmahHandledErrorLoggerFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            //if (!context.ExceptionHandled)
            //    return;

            //var articleLink = context.Exception.Data["articlelink"] as string;
            //var articleDoi = context.Exception.Data["doi"] as string;
            //Exception customException;

            //if (articleLink != null)
            //{
            //    customException = new Exception(articleLink, context.Exception);
            //    ErrorSignal.FromCurrentContext().Raise(customException);
            //}
            //else if (articleDoi != null)
            //{
            //    customException = new Exception(articleDoi, context.Exception);
            //    ErrorSignal.FromCurrentContext().Raise(customException);
            //}
            //else
            //    ErrorSignal.FromCurrentContext().Raise(context.Exception);

            if (context.ExceptionHandled)
                ErrorStore.LogException(context.Exception, HttpContext.Current, true, true);
                //ErrorSignal.FromCurrentContext().Raise(context.Exception);
            // all other exceptions will be caught by ELMAH anyway
        }
    }
}
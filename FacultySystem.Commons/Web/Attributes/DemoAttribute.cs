using ContentManagementSystem.Commons.ActionResults;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ContentManagementSystem.Commons.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DemoAttribute : ActionFilterAttribute
    {
        private bool _isJtableCaller;
        private bool _isAjaxCaller;
        private string _errorType;
        public DemoAttribute(bool isJtableCaller = false, bool isAjaxCaller = false, string errorType = "danger")
        {
            _isJtableCaller = isJtableCaller;
            _isAjaxCaller = isAjaxCaller;
            _errorType = errorType;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isActiveDemo = ConfigurationManager.AppSettings["IsActiveDemo"].ToLowerInvariant() == "true";

            if (!isActiveDemo || 
                filterContext.HttpContext.Request.HttpMethod == "GET" || 
                filterContext.IsChildAction)
            {
                base.OnActionExecuting(filterContext);
            }
            else if (filterContext.HttpContext.Request.IsAjaxRequest() || _isAjaxCaller)
            {
                if(_isJtableCaller)
                {
                    filterContext.Result = new JsonResult { Data = new { Result = "ERROR", Message = "در نسخه نمایشی قادر به انجام این کار نیستید." } };
                    return;
                }
                filterContext.Result = new CamelCaseJsonResult { Data = new { Type = _errorType, Title = "در نسخه نمایشی قادر به انجام این کار نیستید." } };
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary { {"area", null }, {"controller", "Error"}, {"action", "Demo"} });
            }
        }
    }
}

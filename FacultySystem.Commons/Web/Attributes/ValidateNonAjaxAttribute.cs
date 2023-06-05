using System;
using System.Web.Mvc;

namespace ContentManagementSystem.Commons.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ValidateNonAjaxAttribute : ActionFilterAttribute
    {
        public string Action { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //ControllerBase controllerBase;
            var controllerBase = filterContext.IsChildAction ? filterContext.ParentActionViewContext.Controller : filterContext.Controller;
            var viewData = controllerBase.ViewData;

            if (!string.IsNullOrWhiteSpace(Action))
                controllerBase.ControllerContext.RouteData.Values["action"] = Action;

            if (!viewData.ModelState.IsValid)
            {
                viewData.ModelState.SetModelValue("CaptchaInputText", 
                    new ValueProviderResult(string.Empty, string.Empty, System.Threading.Thread.CurrentThread.CurrentCulture));

                if (!string.IsNullOrWhiteSpace(Action))
                {
                    filterContext.Controller.ViewData = viewData;
                    filterContext.Controller.TempData = controllerBase.TempData;
                    filterContext.Result = new RedirectToRouteResult(controllerBase.ControllerContext.RouteData.Values);
                }
                else
                    filterContext.Result = new ViewResult
                    {
                        ViewData = viewData,
                        TempData = controllerBase.TempData
                    };
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
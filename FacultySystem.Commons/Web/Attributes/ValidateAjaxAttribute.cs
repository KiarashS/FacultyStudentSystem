using System.Linq;
using System.Web.Mvc;
using ContentManagementSystem.Commons.Web.ActionResults;
using System.Collections.Generic;
using ContentManagementSystem.Commons.ActionResults;

namespace ContentManagementSystem.Commons.Web.Attributes
{
    public class ValidateAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                return;

            var modelState = filterContext.Controller.ViewData.ModelState;

            if (modelState.IsValid) return;

            var errorModel =
                from x in modelState.Keys
                where modelState[x].Errors.Count > 0
                select new ExceptionMessageModel
                {
                    Key = x,
                    ResultType = ResultType.Information,
                    Messages = modelState[x].Errors.
                        Select(y => y.ErrorMessage).
                        ToArray()
                };

            var result = new ExceptionModel { Results = errorModel.ToArray()};

            filterContext.Result = new CamelCaseJsonResult
            {
                Data = result
            };
        }
    }

    public class ExceptionModel
    {
        public ExceptionMessageModel[] Results { get; set; }
    }

    public class ExceptionMessageModel
    {
        public ExceptionMessageModel()
        {
            ResultType = ResultType.Information;
        }

        public ResultType ResultType { get; set; }
        public string Key { get; set; }
        public IEnumerable<string> Messages { get; set; }
    }

    public enum ResultType
    {
        Alert,
        Success,
        Error,
        Warning,
        Information
    }

}
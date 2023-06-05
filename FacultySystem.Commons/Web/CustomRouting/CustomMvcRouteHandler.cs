using System.Web.Mvc;

namespace ContentManagementSystem.Commons.Web.CustomRouting
{
    /// <summary>
    /// Customize MvcRouteHandler to return CustomMvcHandler instead of 
    /// default IHttpHandler
    /// </summary>
    public class CustomMvcRouteHandler : MvcRouteHandler
    {
        public CustomMvcRouteHandler()
            : base()
        {
        }

        public CustomMvcRouteHandler(IControllerFactory controllerFactory)
            : base(controllerFactory)
        {
        }

        public IRouteValueLookupService RouteValueService { get; set; }

        protected override System.Web.IHttpHandler GetHttpHandler(System.Web.Routing.RequestContext requestContext)
        {
            requestContext.HttpContext.SetSessionStateBehavior(GetSessionStateBehavior(requestContext));
            return new CustomMvcHandler(requestContext)
            {
                RouteHandler = this,
                RouteValueService = this.RouteValueService
            };
        }
    }
}

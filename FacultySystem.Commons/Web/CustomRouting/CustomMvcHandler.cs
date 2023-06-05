using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace ContentManagementSystem.Commons.Web.CustomRouting
{
    /// <summary>
    /// Extend MvcHandler to sort the Optional Parameters before
    /// Processing the request
    /// </summary>
    class CustomMvcHandler : MvcHandler
    {
        /// <summary>
        /// Do not hide the base-constructor
        /// </summary>
        /// <param name="requestContext"></param>
        public CustomMvcHandler(RequestContext requestContext)
            : base(requestContext)
        {
        }

        public IRouteValueLookupService RouteValueService { get; set; }
        public IRouteHandler RouteHandler { get; set; }

        protected override void ProcessRequest(System.Web.HttpContext httpContext)
        {
            if (!this.IsControllerExist())
            {
                UseAlternativeRoute();
            }

            this.SortOptionalParameters();
            base.ProcessRequest(httpContext);
        }

        protected override IAsyncResult BeginProcessRequest(System.Web.HttpContextBase httpContext, AsyncCallback callback, object state)
        {
            if (!this.IsControllerExist())
            {
                UseAlternativeRoute();
            }

            this.SortOptionalParameters();
            return base.BeginProcessRequest(httpContext, callback, state);
        }

        protected bool IsControllerExist()
        {
            CustomRoute customRoute = this.RequestContext.RouteData.Route as CustomRoute;

            // if AlternativeRoute is null, there is no need to do anything
            if (customRoute.AlternativeRoute == null)
            {
                return true;
            }

            // Get the controller type
            string controllerName = RequestContext.RouteData.GetRequiredString("controller");

            //ICP.CustomRouting.FromMvcLibrary.DefaultControllerFactory factory = new ICP.CustomRouting.FromMvcLibrary.DefaultControllerFactory();
            CustomControllerFactory factory = new CustomControllerFactory();
            var type = factory.GetControllerType(this.RequestContext, controllerName);
            return (type != null);
        }

        protected void UseAlternativeRoute()
        {
            CustomRoute customRoute = this.RequestContext.RouteData.Route as CustomRoute;
            this.RequestContext.RouteData = customRoute.AlternativeRoute.GetRouteData(this.RequestContext.HttpContext);
        }

        protected void SortOptionalParameters()
        {
            //if (this.RequestContext.RouteData.Route.Equals(null))
            //{
            //    return;
            //}

            CustomRoute customRoute = this.RequestContext.RouteData.Route as CustomRoute;

            if (customRoute == null || customRoute.OptionalParameters == null)
            {
                return;
            }

            RouteValueDictionary routeValues = this.RequestContext.RouteData.Values;
            RouteValueDictionary temp = new RouteValueDictionary();

            var toDelete = new List<string>();

            foreach (var key in routeValues.Keys)
            {
                if (customRoute.OptionalParameters.Count(c => c == key) > 0)
                {
                    var value = routeValues[key].ToString();
                    var routeParameterName = this.RouteValueService.GetRouteTypeName(value);
                    temp[routeParameterName] = value;
                    toDelete.Add(key);
                }
            }

            foreach (var item in toDelete)
            {
                routeValues.Remove(item);
            }

            foreach (var item in temp)
            {
                routeValues[item.Key] = temp[item.Key];
            }
        }
    }
}

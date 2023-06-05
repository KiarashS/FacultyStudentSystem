using System.Collections.Generic;
using System.Web.Routing;

namespace ContentManagementSystem.Commons.Web.CustomRouting
{
    public class CustomRoute : Route
    {
        /// <summary>
        /// it shouldn't hide the base constructors.
        /// </summary>
        public CustomRoute(string url, IRouteHandler routeHandler)
            : base(url, routeHandler)
        {
            this.DefaultUrl = url;
            this.DataTokens = new RouteValueDictionary();
        }

        /// <summary>
        /// it shouldn't hide the base constructors.
        /// </summary>
        public CustomRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler)
            : base(url, defaults, routeHandler)
        {
            this.DefaultUrl = url;
            this.DataTokens = new RouteValueDictionary();
        }

        public CustomRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, IRouteHandler routeHandler)
            : base(url, defaults, constraints, routeHandler)
        {
            this.DefaultUrl = url;
            this.DataTokens = new RouteValueDictionary();
        }

        public CustomRoute(string url, RouteValueDictionary defaults, RouteValueDictionary constraints, RouteValueDictionary dataTokens, IRouteHandler routeHandler)
            : base(url, defaults, constraints, dataTokens, routeHandler)
        {
            this.DefaultUrl = url;
            this.DataTokens = new RouteValueDictionary();
        }

        protected string DefaultUrl { get; set; }
        public List<string> OptionalParameters { get; set; }

        /// <summary>
        /// The routing will use this route, if it couldn't find a controller for current route 
        /// </summary>
        public Route AlternativeRoute { get; set; }

        /// <summary>
        /// Remove parameters that don't have any value
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            this.Url = this.DefaultUrl;
            VirtualPathData path = base.GetVirtualPath(requestContext, values);

            if (OptionalParameters != null)
            {
                foreach (var key in OptionalParameters)
                {
                    if (values[key] == null)
                    {
                        this.Url = this.Url.Replace(string.Format("/{{{0}}}", key), string.Empty);
                    }
                }
            }

            if (path != null)
            {
                string virtualPath = path.VirtualPath;
                var lastIndexOf = virtualPath.LastIndexOf("?");

                if (lastIndexOf != 0)
                {
                    if (lastIndexOf > 0)
                    {
                        string leftPart = virtualPath.Substring(0, lastIndexOf).ToLowerInvariant();
                        string queryPart = virtualPath.Substring(lastIndexOf);
                        path.VirtualPath = leftPart + queryPart;
                    }
                    else
                    {
                        path.VirtualPath = path.VirtualPath.ToLowerInvariant();
                    }
                }
            }

            return path;
        }
    }
}

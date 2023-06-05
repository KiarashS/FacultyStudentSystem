using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Web.Http.Routing;
using ContentManagementSystem.Commons.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ContentManagementSystem.Commons.Web.CustomRouting;

namespace ContentManagementSystem.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.RouteExistingFiles = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("content/{*pathInfo}");
            routes.IgnoreRoute("scripts/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("{resource}.ico");
            routes.IgnoreRoute("{resource}.png");
            routes.IgnoreRoute("{resource}.jpg");
            routes.IgnoreRoute("{resource}.gif");
            routes.IgnoreRoute("{resource}.txt");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            //routes.IgnoreRoute("{*map}", new { map = @"(.*/)?.map(/.*)?" });
            routes.IgnoreRoute("fonts/{*pathInfo}");

            //#if DEBUG
            //routes.IgnoreRoute("{*browserlink}", new { browserlink = @".*/arterySignalR/ping" });
            //#endif

            var defaultRoute = routes.MapReplaceableRoute(
                                    name: "Default",
                                    url: "{controller}/{action}/{id}/{title}",
                                    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional, title = UrlParameter.Optional },
                                    namespaces: new[] { "ContentManagementSystem.Web.Controllers" }
                                    );
            
            var profileRoute = routes.MapRouteLowercase(
                                    name: "Profile",
                                    url: "{pageId}/{action}/{id}/{title}",
                                    defaults: new { controller = "Profile", action = "Index", id = UrlParameter.Optional, title = UrlParameter.Optional },
                                    namespaces: new[] { "ContentManagementSystem.Web.Controllers" }
                                );

            defaultRoute.AlternativeRoute = profileRoute;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace ContentManagementSystem.Commons.Web.CustomRouting
{
    /// <summary>
    /// Extend RouteCollection to support using CustomRoute, With LowerCase Url Support
    /// </summary>
    public static class CustomRouteCollectionExtensions
    {
        /// <summary>
        /// Create an instance of CustomRoute and register it in the given route collection
        /// </summary>
        public static Route MapRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces, string[] lookupParameters, IRouteValueLookupService lookupService)
        {
            CustomRoute route = new CustomRoute(url, new CustomMvcRouteHandler()
            {
                RouteValueService = lookupService
            })
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary(namespaces),
            };

            if (defaults != null)
            {
                route.Defaults = CreateRouteValueDictionary(defaults);
            }

            if (lookupParameters != null)
            {
                route.OptionalParameters = lookupParameters.ToList();
            }

            if (namespaces != null && namespaces.Length > 0)
            {
                route.DataTokens["Namespaces"] = namespaces;
            }

            routes.Add(name, route);

            return route;
        }


        public static CustomRoute MapReplaceableRoute(this RouteCollection routes, string name, string url)
        {
            return MapReplaceableRoute(routes, name, url, null, null, null);
        }

        public static CustomRoute MapReplaceableRoute(this RouteCollection routes, string name, string url, object defaults)
        {
            return MapReplaceableRoute(routes, name, url, defaults, null, null);
        }

        public static CustomRoute MapReplaceableRoute(this RouteCollection routes, string name, string url, string[] namespaces)
        {
            return MapReplaceableRoute(routes, name, url, null, null, namespaces);
        }

        public static CustomRoute MapReplaceableRoute(this RouteCollection routes, string name, string url, object defaults, object constraints)
        {
            return MapReplaceableRoute(routes, name, url, defaults, constraints, null);
        }

        public static CustomRoute MapReplaceableRoute(this RouteCollection routes, string name, string url, object defaults, string[] namespaces)
        {
            return MapReplaceableRoute(routes, name, url, defaults, null, namespaces);
        }

        /// <summary>
        /// Create an instance of CustomRoute and register it in the given route collection
        /// </summary>
        public static CustomRoute MapReplaceableRoute(this RouteCollection routes, string name, string url, object defaults, object constraints, string[] namespaces)
        {
            if (routes == null)
            {
                throw new ArgumentNullException("routes");
            }

            if (url == null)
            {
                throw new ArgumentNullException("url");
            }

            CustomRoute route = new CustomRoute(url, new CustomMvcRouteHandler())
            {
                Defaults = new RouteValueDictionary(defaults),
                Constraints = new RouteValueDictionary(constraints),
                DataTokens = new RouteValueDictionary(namespaces),
            };

            if (defaults != null)
            {
                route.Defaults = CreateRouteValueDictionary(defaults);
            }

            if (namespaces != null && namespaces.Length > 0)
            {
                route.DataTokens["Namespaces"] = namespaces;
            }

            routes.Add(name, route);

            return route;
        }


        private static RouteValueDictionary CreateRouteValueDictionary(object values)
        {
            var dictionary = values as IDictionary<string, object>;
            if (dictionary != null)
            {
                return new RouteValueDictionary(dictionary);
            }

            return new RouteValueDictionary(values);
        }
    }
}

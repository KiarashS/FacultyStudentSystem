using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace ContentManagementSystem.Commons.Web.CustomRouting
{
    /// <summary>
    /// The access modifier of the GetControllerType in DefaultControllerFactory is
    /// protected. So we have to inherit from it and make it public
    /// </summary>
    public class CustomControllerFactory : DefaultControllerFactory
    {
        public Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            return base.GetControllerType(requestContext, controllerName);
        }
    }
}

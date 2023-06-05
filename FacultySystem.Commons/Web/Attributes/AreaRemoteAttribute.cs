using System;
using System.Web.Mvc;

namespace ContentManagementSystem.Commons.Web.Attributes
{
    public class AreaRemoteAttribute : CustomRemoteAttribute
    {
        public AreaRemoteAttribute(string action, string controller, string area)
            : base(action, controller, area)
        {
            this.RouteData["area"] = area;
        }
    }

    public class CustomRemoteAttribute : RemoteAttribute
    {
        public CustomRemoteAttribute(string action, string controller, string areaName)
        {
            if (string.IsNullOrWhiteSpace(action))
            {
                throw new ArgumentException("action name is null", "action");
            }
            if (string.IsNullOrWhiteSpace(controller))
            {
                throw new ArgumentException("controller name is null", "controller");
            }
            this.RouteData["controller"] = controller;
            this.RouteData["action"] = action;
            if (!string.IsNullOrWhiteSpace(areaName))
            {
                this.RouteData["area"] = areaName;
            }
        }
    }
}
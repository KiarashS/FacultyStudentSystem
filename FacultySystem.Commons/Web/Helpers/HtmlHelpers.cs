using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;

namespace ContentManagementSystem.Commons.Web.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString MenuLinkBootstrap(this HtmlHelper helper, string text, string action, string controller, bool isPjax = false)
        {
            var routeData = helper.ViewContext.RouteData.Values;
            var currentController = routeData["controller"];
            var currentAction = routeData["action"];

            if (String.Equals(action, currentAction as string, StringComparison.OrdinalIgnoreCase) &&
                String.Equals(controller, currentController as string, StringComparison.OrdinalIgnoreCase))
            {
                return isPjax ? new MvcHtmlString("<li class=\"active\">" + helper.ActionLink(text, action, controller, null, new { data_withpjax = "with-pjax" }) + "</li>") : new MvcHtmlString("<li class=\"active\">" + helper.ActionLink(text, action, controller) + "</li>");
            }

            return isPjax ? new MvcHtmlString("<li>" + helper.ActionLink(text, action, controller, null, new { data_withpjax = "with-pjax" }) + "</li>") : new MvcHtmlString("<li>" + helper.ActionLink(text, action, controller) + "</li>");
        }

        public static MvcHtmlString MenuLinkUserProfile(this HtmlHelper helper, string text, string action, string controller, string iconClass, string col)
	    {
	        var routeData = helper.ViewContext.RouteData.Values;
	        var currentController = routeData["controller"];
	        var currentAction = routeData["action"];
	        var linkTag = new TagBuilder("a");
	        var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
	
	        linkTag.InnerHtml = "<i class=\"" + iconClass + "\"></i><p>" + text + "</p>";
	        linkTag.MergeAttribute("href", urlHelper.Action(action, controller));
	
	        if (String.Equals(action, currentAction as string, StringComparison.OrdinalIgnoreCase) &&
	            String.Equals(controller, currentController as string, StringComparison.OrdinalIgnoreCase))
	            linkTag.AddCssClass($"col-md-{col} active");
	        else
	            linkTag.AddCssClass($"col-md-{col}");
	
	        return new MvcHtmlString(linkTag.ToString());
	    }

        public static MvcHtmlString AdminSideMenu(this HtmlHelper helper, string text, string action, string controller, string iconClass)
        {
            var routeData = helper.ViewContext.RouteData.Values;
            var currentController = routeData["controller"];
            var currentAction = routeData["action"];
            var linkTag = new TagBuilder("a");
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);

            linkTag.InnerHtml = "<i class=\"" + iconClass + "\"></i> " + text + "";
            linkTag.MergeAttribute("href", urlHelper.Action(action, controller));

            if (String.Equals(action, currentAction as string, StringComparison.OrdinalIgnoreCase) &&
                String.Equals(controller, currentController as string, StringComparison.OrdinalIgnoreCase))
            {
                return new MvcHtmlString("<li class=\"active\">" + linkTag + "</li>");
            }

            return new MvcHtmlString("<li>" + linkTag + "</li>");
        }

        public static HtmlString ApplicationVersion(this HtmlHelper helper)
        {
            var revision = ConfigurationManager.AppSettings["Revision"];

            if (revision != null)
            {
                return new HtmlString(string.Format("<span class=\"rev\">rev {0}</span>", revision));
            }

            return new HtmlString("");
        }

        public static MvcHtmlString Script(this HtmlHelper htmlHelper, Func<object, HelperResult> template)
        {
            htmlHelper.ViewContext.HttpContext.Items["_script_" + Guid.NewGuid()] = template;
            return MvcHtmlString.Empty;
        }

        public static IHtmlString RenderScripts(this HtmlHelper htmlHelper)
        {
            foreach (object key in htmlHelper.ViewContext.HttpContext.Items.Keys)
            {
                if (key.ToString().StartsWith("_script_"))
                {
                    var template = htmlHelper.ViewContext.HttpContext.Items[key] as Func<object, HelperResult>;
                    if (template != null)
                    {
                        htmlHelper.ViewContext.Writer.Write(template(null));
                    }
                }
            }
            return MvcHtmlString.Empty;
        }

        private class ScriptBlock : IDisposable
        {
            private const string scriptsKey = "scripts";
            public static List<string> pageScripts
            {
                get
                {
                    if (HttpContext.Current.Items[scriptsKey] == null)
                        HttpContext.Current.Items[scriptsKey] = new List<string>();
                    return (List<string>)HttpContext.Current.Items[scriptsKey];
                }
            }

            WebViewPage webPageBase;

            public ScriptBlock(WebViewPage webPageBase)
            {
                this.webPageBase = webPageBase;
                this.webPageBase.OutputStack.Push(new StringWriter());
            }

            public void Dispose()
            {
                pageScripts.Add(webPageBase.OutputStack.Pop().ToString());
            }
        }

        public static IDisposable BeginScripts(this HtmlHelper helper)
        {
            return new ScriptBlock((WebViewPage)helper.ViewDataContainer);
        }

        public static MvcHtmlString PageScripts(this HtmlHelper helper)
        {
            return MvcHtmlString.Create(string.Join(Environment.NewLine, ScriptBlock.pageScripts.Select(s => s.ToString(CultureInfo.InvariantCulture))));
        }

        public static string ConvertImageToBase64(this HtmlHelper helper, string imagePath)
        {
            using (var image = Image.FromFile(imagePath))
            {
                using (var m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    var imageBytes = m.ToArray();

                    // Convert byte[] to Base64 String
                    var base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }

        public static string IsSelected(this HtmlHelper html, string controllers = "", string actions = "", string cssClass = "current_section")
        {
            ViewContext viewContext = html.ViewContext;
            bool isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

            if (isChildAction)
                viewContext = html.ViewContext.ParentActionViewContext;

            RouteValueDictionary routeValues = viewContext.RouteData.Values;
            string currentAction = routeValues["action"].ToString();
            string currentController = routeValues["controller"].ToString();

            if (String.IsNullOrEmpty(actions))
                actions = currentAction;

            if (String.IsNullOrEmpty(controllers))
                controllers = currentController;

            string[] acceptedActions = actions.Trim().Split(',').Distinct().ToArray();
            string[] acceptedControllers = controllers.Trim().Split(',').Distinct().ToArray();

            return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
                cssClass : String.Empty;
        }
    }
}
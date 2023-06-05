using System;
using System.Web;

namespace ContentManagementSystem.Commons.Web.Modules
{
    public class IpRestrictAccess : IHttpModule
    {
        public IpRestrictAccess() { }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(Application_BeginRequest);
        }

        private void Application_BeginRequest(object source, EventArgs e)
        {
            var context = ((HttpApplication)source).Context;
            var ipAddress = context.Request.UserHostAddress;

            if (IsValidIpAddress(ipAddress)) return;

            context.Response.StatusCode = 403;  // (Forbidden)
            context.Response.SuppressContent = true;
            context.Response.End();
        }

        private static bool IsValidIpAddress(string ipAddress)
        {
            return (ipAddress == "147.0.0.1");
        }

        //private async Task<String> GetCountryName(string ipAddress)
        //{
        //    var requestUrl = string.Format("http://www.iptolatlng.com/?type=json&ip=" + ipAddress);

        //    var request = (HttpWebRequest)WebRequest.Create(requestUrl);
        //    var response = (HttpWebResponse)await request.GetResponseAsync().WithTimeout(10000);

        //    if (response == null)
        //        return "ir";

        //    //response.
        //    return "ir";
        //}

        public void Dispose() { }
    }
}
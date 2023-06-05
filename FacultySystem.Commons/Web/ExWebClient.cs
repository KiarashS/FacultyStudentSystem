using System;
using System.Net;

namespace ContentManagementSystem.Commons.Web
{
    public class ExWebClient : WebClient
    {
        public string Method
        {
            get;
            set;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var webRequest = base.GetWebRequest(address);

            if (!string.IsNullOrEmpty(Method))
                webRequest.Method = Method;

            return webRequest;
        }
    }
}
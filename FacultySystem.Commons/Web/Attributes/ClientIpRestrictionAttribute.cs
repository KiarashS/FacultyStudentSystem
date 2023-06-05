//using System;
//using System.Web;
//using System.Web.Mvc;
//using ContentManagementSystem.ServiceLayer.Contracts;
//using ContentManagementSystem.Commons.Web.ActionResults;
////using MaxMind.GeoIP2;
//using StructureMap;

//namespace ContentManagementSystem.Commons.Web.Attributes
//{
//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//    public class ClientIpRestrictionAttribute : ActionFilterAttribute
//    {

//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            if (HttpContext.Current.Request.IsLocal)
//                return;

//            //var clientIpAddress = HttpContext.Current.Request.UserHostAddress;

//            var clientIpAddress = HttpContext.Current.Request.Headers["X-Real-IP"];
//            if (clientIpAddress == null || clientIpAddress.Length <= 0)
//            {
//                clientIpAddress = HttpContext.Current.Request.UserHostAddress;
//            }

//            if (string.IsNullOrWhiteSpace(clientIpAddress))
//                filterContext.Result = new EmptyResult();

//            var readerInstance = IpReaderSingleton.GetInstance;
//            var reader = readerInstance.DatabaseReader;
//            var country = reader.Country(clientIpAddress);
//            var countryName = country.Country.Name;

//            if (countryName == null || country == null)
//                return;
//            //filterContext.Result = NotificationResult(new[]
//            //{
//            //    new ExceptionMessageModel
//            //    {
//            //        Key = "vpn",
//            //        ResultType = ResultType.Warning,
//            //        Messages = new[]
//            //        {
//            //            string.Format("کاربر گرامی شما از کشوری غیر از ایران وارد سامانه شده اید."),
//            //            "در صورت استفاده از V.P.N لطفاً ارتباط خود را با V.P.N قطع کرده و با IP ایران وارد سامانه شوید."
//            //        }
//            //    }
//            //});

//            if (countryName.ToLowerInvariant() != "iran")
//            {
//                var ipService = ObjectFactory.GetInstance<IIpService>();
//                var ipNo = Ip2Decimal(clientIpAddress);
//                if (ipService.IsFromIran(ipNo))
//                    return;
//            }

//            if (filterContext.HttpContext.Request.HttpMethod == "GET" && countryName.ToLowerInvariant() != "iran")
//                filterContext.Result = new ViewResult { ViewName = MVC.Shared.Views.IpRestriction, ViewData = new ViewDataDictionary { { "CountryName", countryName } } };
//            else if (filterContext.HttpContext.Request.HttpMethod == "POST" && countryName.ToLowerInvariant() != "iran")
//                filterContext.Result = NotificationResult(new[]
//                {
//                    new ExceptionMessageModel
//                    {
//                        Key = "vpn",
//                        ResultType = ResultType.Warning,
//                        Messages = new[]
//                        {
//                            string.Format("کاربر گرامی شما از کشور {0} وارد سامانه شده اید.", countryName),
//                            "در صورت استفاده از V.P.N لطفاً ارتباط خود را با V.P.N قطع کرده و با IP ایران وارد سامانه شوید."
//                        }
//                    }
//                });
//        }

//        private static ActionResult NotificationResult(ExceptionMessageModel[] messages)
//        {
//            var jsonResult = new ExceptionModel
//            {
//                Results = messages
//            };

//            return new CamelCaseJsonResult { Data = jsonResult, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
//        }

//        private long Ip2Decimal(String dottedIp)
//        {
//            long intResult = 0;
//            try
//            {
//                var arrConvert = dottedIp.Split('.');
//                int i;
//                for (i = arrConvert.Length - 1; i >= 0; i--)
//                {
//                    intResult = intResult + ((long.Parse(arrConvert[i]) % 256) * long.Parse(Math.Pow(256, 3 - i).ToString()));
//                }
//                return intResult;
//            }
//            catch (Exception ex)
//            {
//                return 0;
//            }
//        }

//    }

//    public sealed class IpReaderSingleton
//    {
//        public readonly DatabaseReader DatabaseReader = new DatabaseReader(HttpContext.Current.Server.MapPath("~/App_Data/GeoLite2-Country.mmdb"));
//        static readonly IpReaderSingleton Instance = new IpReaderSingleton();

//        private IpReaderSingleton()
//        { }

//        public static IpReaderSingleton GetInstance
//        {
//            get
//            {

//                return Instance;
//            }
//        }
//    }
//}
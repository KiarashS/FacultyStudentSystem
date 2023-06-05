//using System.Web.Mvc;
//using Journals.Web.Utils.Providers;

//[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(AppStart), "Start")]
//namespace Journals.Web.Utils.Providers
//{
//    public class AppStart
//    {
//        public static void Start()
//        {
//            var currentFactory = ControllerBuilder.Current.GetControllerFactory();
//            ControllerBuilder.Current.SetControllerFactory(new BrockAllen.CookieTempData.CookieTempDataProvider(currentFactory));
//        }
//    }
//}

using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace ContentManagementSystem.Commons.Web.Providers.CookieTempData
{
    public class CookieTempDataControllerFactory : IControllerFactory
    {
        IControllerFactory _inner;
        public CookieTempDataControllerFactory(IControllerFactory inner)
        {
            _inner = inner;
        }

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            // pass-thru to the normal factory
            var controllerInterface = _inner.CreateController(requestContext, controllerName);
            var controller = controllerInterface as Controller;
            if (controller != null)
            {
                // if we get a MVC controller then add the cookie-based tempdata provider
                controller.TempDataProvider = new CookieTempDataProvider();
            }
            return controller;
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            return _inner.GetControllerSessionBehavior(requestContext, controllerName);
        }

        public void ReleaseController(IController controller)
        {
            _inner.ReleaseController(controller);
        }
    }
}
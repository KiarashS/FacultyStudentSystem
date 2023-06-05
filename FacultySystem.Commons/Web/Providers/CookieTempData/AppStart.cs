using System.Web.Mvc;
using ContentManagementSystem.Commons.Web.Providers.CookieTempData;
using WebActivatorEx;

[assembly: PostApplicationStartMethod(typeof(AppStart), "Start")]
namespace ContentManagementSystem.Commons.Web.Providers.CookieTempData
{
    public class AppStart
    {
        public static void Start()
        {
            //DynamicModuleUtility.RegisterModule(typeof(SetFactoryModule));
            var currentFactory = ControllerBuilder.Current.GetControllerFactory();
            if (!(currentFactory is CookieTempDataControllerFactory))
            {
                ControllerBuilder.Current.SetControllerFactory(new CookieTempDataControllerFactory(currentFactory));
            }

        }
    }

    //public class SetFactoryModule : IHttpModule
    //{
    //    static SetFactoryModule()
    //    {
    //        var currentFactory = ControllerBuilder.Current.GetControllerFactory();
    //        if (!(currentFactory is CookieTempDataControllerFactory))
    //        {
    //            ControllerBuilder.Current.SetControllerFactory(new CookieTempDataControllerFactory(currentFactory));
    //        }
    //    }
        
    //    public void Init(HttpApplication app)
    //    {
    //    }

    //    public void Dispose()
    //    {
    //    }
    //}
}

//using System.Web.Mvc;
//using Journals.Web.Utils.Providers.CookieTempData;

//[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(AppStart), "Start")]
//namespace Journals.Web.Utils.Providers.CookieTempData
//{
//    public class AppStart
//    {
//        public static void Start()
//        {
//            var currentFactory = ControllerBuilder.Current.GetControllerFactory();
//            ControllerBuilder.Current.SetControllerFactory(new CookieTempDataControllerFactory(currentFactory));
//        }
//    }
//}
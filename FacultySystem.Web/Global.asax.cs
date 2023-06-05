using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
//using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Data.Entity;
using InteractivePreGeneratedViews;
using ContentManagementSystem.DataLayer.Context;
using StructureMap.Web.Pipeline;
using ContentManagementSystem.IocConfig;

namespace ContentManagementSystem.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            try
            {
                //Disable WebForms(aspx) view engine
                ViewEngines.Engines.Clear();
                ViewEngines.Engines.Add(new RazorViewEngine().DisableVbhtml());

                MvcHandler.DisableMvcResponseHeader = true;

                //profiler initialize
                //MiniProfilerEF6.Initialize();

                //entity framework database initializer
                //prevent auto initialize DB
                //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
                //Database.SetInitializer<ApplicationDbContext>(null);
                //using (var context = new ApplicationDbContext())
                //{
                //    context.Database.Initialize(force: true);
                //}

                using (var ctx = new ApplicationDbContext())
                {
                    var viewPath = Server.MapPath("~/App_Data/EFViews.xml");

                    InteractiveViews.SetViewCacheFactory(ctx, new FileViewCacheFactory(viewPath));
                }

                //WebApiConfig.Register(GlobalConfiguration.Configuration);

                AreaRegistration.RegisterAllAreas();
                FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                //BundleConfig.RegisterBundles(BundleTable.Bundles);

                setDbInitializer();
                //Set current Controller factory as StructureMapControllerFactory
                ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
                //Microsoft.AspNet.SignalR.GlobalHost.DependencyResolver = SmObjectFactory.Container.GetInstance<Microsoft.AspNet.SignalR.IDependencyResolver>();
            }
            catch
            {
                HttpRuntime.UnloadAppDomain();
                throw;
            }
        }

        protected void Application_OnAuthenticateRequest(Object src, EventArgs e)
        {
            if (HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    if (HttpContext.Current.User.Identity is FormsIdentity)
                    {
                        var id = HttpContext.Current.User.Identity as FormsIdentity;
                        FormsAuthenticationTicket ticket = id.Ticket;
                        string userData = ticket.UserData;
                        // Roles is a helper class which places the roles of the
                        // currently logged on user into a string array
                        // accessable via the value property.
                        //Roles userRoles = new Roles(userData);
                        HttpContext.Current.User = new GenericPrincipal(id, userData.Split(','));
                    }
                }
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //if (Request.IsLocal)
            //{
            //    MiniProfiler.Start();
            //    //HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();
            //}
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            //MiniProfiler.Stop();

            HttpContextLifecycle.DisposeAndClearAll();
        }

        //void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        //{
        //    var app = sender as HttpApplication;
        //    if (app == null) return;

        //    var acceptEncoding = app.Request.Headers["Accept-Encoding"];
        //    var prevUncompressedStream = app.Response.Filter;

        //    if (!(app.Context.CurrentHandler is Page ||
        //      app.Context.CurrentHandler.GetType().Name == "SyncSessionlessHandler") ||
        //    app.Request["HTTP_X_MICROSOFTAJAX"] != null)
        //        return;

        //    if (string.IsNullOrEmpty(acceptEncoding))
        //        return;

        //    acceptEncoding = acceptEncoding.ToLower();

        //    if (acceptEncoding.Contains("deflate") || acceptEncoding == "*")
        //    {
        //        // defalte
        //        app.Response.Filter = new DeflateStream(prevUncompressedStream, CompressionMode.Compress);
        //        app.Response.AppendHeader("Content-Encoding", "deflate");
        //    }
        //    else if (acceptEncoding.Contains("gzip"))
        //    {
        //        // gzip
        //        app.Response.Filter = new GZipStream(prevUncompressedStream, CompressionMode.Compress);
        //        app.Response.AppendHeader("Content-Encoding", "gzip");
        //    }
        //}

        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            if (app == null || !app.Request.IsLocal || app.Context == null)
                return;
            var headers = app.Context.Response.Headers;
            headers.Remove("Server");
            headers.Set("Server", "Professors Server");
            headers.Add("Server", "Professors Server");
        }

        private static void setDbInitializer()
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
            Database.SetInitializer<ApplicationDbContext>(null);
            SmObjectFactory.Container.GetInstance<IUnitOfWork>().ForceDatabaseInitialize();
        }

        public class StructureMapControllerFactory : DefaultControllerFactory
        {
            protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
            {
                if (controllerType == null)
                    throw new InvalidOperationException(string.Format("Page not found: {0}", requestContext.HttpContext.Request.RawUrl));
                return SmObjectFactory.Container.GetInstance(controllerType) as Controller;
            }
        }

    }
}

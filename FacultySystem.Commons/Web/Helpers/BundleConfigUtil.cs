using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;

namespace ContentManagementSystem.Commons.Web.Helpers
{
    /// <summary>
    /// A custom bundle orderer (IBundleOrderer) that will ensure bundles are
    /// included in the order you register them.
    /// </summary>
    public class AsIsBundleOrderer : IBundleOrderer
    {
        // for V1.0.0 of Microsoft.AspNet.Web.Optimization
        //public IEnumerable<FileInfo> OrderFiles(BundleContext context, IEnumerable<FileInfo> files)
        //{
        //    return files;
        //}

        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }

    public static class BundleConfig
    {
        private static void AddBundle(string virtualPath, bool isCss, params string[] files)
        {
            //BundleTable.EnableOptimizations = false;
            BundleTable.EnableOptimizations = true;

            var existing = BundleTable.Bundles.GetBundleFor(virtualPath);
            if (existing != null)
                return;

            var newBundle = isCss ? new Bundle(virtualPath, new CssMinify()) : new Bundle(virtualPath, new JsMinify());
            newBundle.Orderer = new AsIsBundleOrderer();

            foreach (var file in files)
                newBundle.Include(file);

            BundleTable.Bundles.Add(newBundle);
        }

        public static IHtmlString AddScripts(string virtualPath, bool enableAsync, params string[] files)
        {
            if (enableAsync)
                Scripts.DefaultTagFormat = @"<script src=""{0}"" async></script>";

            AddBundle(virtualPath, false, files);
            return Scripts.Render(virtualPath);
        }

        public static IHtmlString AddStyles(string virtualPath, params string[] files)
        {
            AddBundle(virtualPath, true, files);
            return Styles.Render(virtualPath);
        }

        public static IHtmlString AddScriptUrl(string virtualPath, params string[] files)
        {
            AddBundle(virtualPath, false, files);
            return Scripts.Url(virtualPath);
        }

        public static IHtmlString AddStyleUrl(string virtualPath, params string[] files)
        {
            AddBundle(virtualPath, true, files);
            return Styles.Url(virtualPath);
        }

    }
}
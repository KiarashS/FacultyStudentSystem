using System;
using System.Web.Optimization;
using Microsoft.Ajax.Utilities;
using System.Web.Mvc;

namespace ContentManagementSystem.Commons
{
	public static class OptimizationExtensions
	{
		public static MvcHtmlString JsMinify(this HtmlHelper helper, Func<object, object> markup)
		{
			if (helper == null || markup == null)
			{
				return MvcHtmlString.Empty;
			}

			var sourceJs = (markup.DynamicInvoke(helper.ViewContext) ?? String.Empty).ToString();

			if (!BundleTable.EnableOptimizations)
			{
				return new MvcHtmlString(sourceJs);
			}

			var minifier = new Minifier();

			var minifiedJs = minifier.MinifyJavaScript(sourceJs, new CodeSettings
			{
				EvalTreatment = EvalTreatment.MakeImmediateSafe,
				PreserveImportantComments = false
			});

			return new MvcHtmlString(minifiedJs);
		}

		public static MvcHtmlString CssMinify(this HtmlHelper helper, Func<object, object> markup)
		{
			if (helper == null || markup == null)
			{
				return MvcHtmlString.Empty;
			}

			var sourceCss = (markup.DynamicInvoke(helper.ViewContext) ?? String.Empty).ToString();

			if (!BundleTable.EnableOptimizations)
			{
				return new MvcHtmlString(sourceCss);
			}

			var minifier = new Minifier();

			var minifiedCss = minifier.MinifyStyleSheet(sourceCss, new CssSettings
			{
				CommentMode = CssComment.None
			});

			return new MvcHtmlString(minifiedCss);
		}
	}
}
/*

<script type="text/javascript">@(Html.JsMinify(@<text>
	//  JS code here
</text>))</script>

or

<style type="text/css">@(Html.CssMinify(@<text>
	/*  CSS rules here * /
</text>))</style>

*/
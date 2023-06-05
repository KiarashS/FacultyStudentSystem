using System.Web;
using System.Web.Mvc;
using ContentManagementSystem.Commons.Web.ActionResults;
using ContentManagementSystem.Commons.Web.Attributes;
using ContentManagementSystem.Commons.ActionResults;

namespace ContentManagementSystem.Commons.Web.Filters
{
    public class ExceptionHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var httpException = filterContext.Exception as HttpException;
            
            if (httpException == null)
                return;

            var httpCode =  httpException.GetHttpCode();

            filterContext.ExceptionHandled = true;

            //var articleLink = filterContext.HttpContext.Request["ArticleLink"];
            //var doi = filterContext.HttpContext.Request["ArticleDoi"];

            //if (articleLink != null)
            //    filterContext.Exception.Data.Add("articlelink", articleLink);

            //if (doi != null)
            //    filterContext.Exception.Data.Add("doi", doi);

            switch (httpCode)
            {
                case 725:
                    filterContext.Result = new CamelCaseJsonResult { Data = new ExceptionModel { Results = new[] { new ExceptionMessageModel { Messages = new[] { "لطفاً دقایقی بعد مجدداً درخواست خود را ارسال نمائید." } } } } };
                    break;
                case 726:
                    filterContext.Result = new CamelCaseJsonResult { Data = new ExceptionModel { Results = new[] { new ExceptionMessageModel { Messages = new[] { "دسترسی به مقاله مورد نظر از طریق لینک مقاله امکان پذیر نمی باشد.", "لطفاً لینک را بررسی نمائید یا از طریق شناسه دیجیتال اقدام نمائید." } } } } };
                    break;
                case 727:
                    filterContext.Result = new CamelCaseJsonResult { Data = new ExceptionModel { Results = new[] { new ExceptionMessageModel { Key = "offline-request", Messages = new[] { "در حال حاضر قادر به دریافت این مقاله نیستید.", "<a href=\"#\" style=\"color: #fff;\" data-source=\"" + httpException.Message.Split(";#;")[0].Trim() + "\" data-titlee=\"" + httpException.Message.Split(";#;")[1].Trim() + "\" data-role=\"offline-request\">درخواست آفلاین مقاله</a>" } } } } };
                    break;
                case 728:
                    filterContext.Result = new CamelCaseJsonResult { Data = new ExceptionModel { Results = new[] { new ExceptionMessageModel { Messages = new[] { "شناسه دیجیتال وارد شده معتبر نمی باشد.", "در صورتی که از صحت شناسه دیجیتال وارد شده اطمینان دارید، لطفا از طریق لینک مفاله اقدام نمائید." } } } } };
                    break;
                case 729:
                    filterContext.Result = new CamelCaseJsonResult { Data = new ExceptionModel { Results = new[] { new ExceptionMessageModel { Messages = new[] { "در حال حاضر پایگاه مربوط به شناسه دیجیتال وارد شده توسط سامانه پشتیبانی نمی شود.", "در صورتی که پایگاه شناسه دیجیتال وارد شده در لیست منابع سامانه موجود است، لطفا از طریق لینک مفاله اقدام نمائید." } } } } };
                    break;
                case 730:
                    filterContext.Result = new CamelCaseJsonResult { Data = new ExceptionModel { Results = new[] { new ExceptionMessageModel { Key = "offline-ebook-request", Messages = new[] { "کتاب درخواستی شما به صورت آفلاین قابل دریافت است.", "<a href=\"#\" style=\"color: #fff;\" data-source=\"" + httpException.Message.Split(";#;")[0].Trim() + "\" data-titlee=\"" + httpException.Message.Split(";#;")[1].Trim() + "\" data-role=\"offline-ebook-request\">درخواست آفلاین کتاب</a>" } } } } }; break;
                case 731:
                    filterContext.Result = new CamelCaseJsonResult { Data = new ExceptionModel { Results = new[] { new ExceptionMessageModel { Messages = new[] { "در حال حاضر دسترسی به پایان نامه مورد نظر مقدور نمی باشد." } } } } };
                    break;
                default:
                    filterContext.Result = new CamelCaseJsonResult { Data = new ExceptionModel { Results = new[] { new ExceptionMessageModel { Messages = new[] { "در حال حاضر سرور قادر به پاسخگویی نیست." } } } } };
                    break;
            }
        }
    }
}
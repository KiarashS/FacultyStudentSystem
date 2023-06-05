using System;
using System.Configuration;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Commons.Web.Captcha
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ValidateCaptchaAttribute : ActionFilterAttribute
    {
        #region Fields (7)
        
        public string ErrorWasHappened { get; set; }     
        public string CaptchaCodeIsRequired { get; set; }
        public string CaptchaCodeIsIncorrect { get; set; }
        public string TimeIsExpired { get; set; }
        public string CookieMustEnabled { get; set; }
        public int ExpireTimeCaptchaCodeBySeconds { get; set; }
        private const string DecryptionKey = "AdflDpdcmfiZSjnSPEQNjFD5lsdf43DdimnqwejdksfSDaA4HSsasdS12qweW3Fod48a9sdfkwe";

        #endregion

        #region Ctors (2)

        public ValidateCaptchaAttribute(int expireTimeCaptchaCodeBySeconds = 60)
        {
            ErrorWasHappened = "خطایی اتفاق افتاده است.";
            CaptchaCodeIsRequired = "لطفاً کد امنیتی را وارد نمائید.";
            CaptchaCodeIsIncorrect = "کد امنیتی را اشتباه وارد کرده اید.";
            CookieMustEnabled = "ابتدا باید قابلیت کوکی ها را در مرورگر خود فعال نمائید.";
            ExpireTimeCaptchaCodeBySeconds = expireTimeCaptchaCodeBySeconds;
            TimeIsExpired = string.Format("حداکثر مهلت وارد کردن کد امنیتی {0} ثانیه است.",
                ExpireTimeCaptchaCodeBySeconds);
        }

        public ValidateCaptchaAttribute(string errorWasHappened, string captchaCodeIsRequired, string captchaCodeIsIncorrect,
            string timeIsExpired, string cookieMustEnabled, int expireTimeCaptchaCodeBySeconds)
        {
            this.ErrorWasHappened = errorWasHappened;
            this.CaptchaCodeIsRequired = captchaCodeIsRequired;
            this.CaptchaCodeIsIncorrect = captchaCodeIsIncorrect;
            this.TimeIsExpired = timeIsExpired;
            this.CookieMustEnabled = cookieMustEnabled;
            this.ExpireTimeCaptchaCodeBySeconds = expireTimeCaptchaCodeBySeconds;
        }

        #endregion

        #region Methods (1)

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(!Convert.ToBoolean(ConfigurationManager.AppSettings["ShowCaptcha"]))
            {
                return;
            }

            var controllerBase = filterContext.Controller;
            var captchaInputTextProvider = controllerBase.ValueProvider.GetValue("CaptchaInputText");
            if (captchaInputTextProvider == null)
            {
                controllerBase.ViewData.ModelState.AddModelError("CaptchaInputText", ErrorWasHappened);
                base.OnActionExecuting(filterContext);
                return;
            }
            var inputText = captchaInputTextProvider.AttemptedValue;

            //if (inputText.Trim().Length <= 0)
            //{
            //    controllerBase.ViewData.ModelState.AddModelError("CaptchaInputText", CaptchaCodeIsRequired);
            //    return;
            //}

            var httpCookie = HttpContext.Current.Request.Cookies["captchastring"];
            
            if (httpCookie == null)
            {
                controllerBase.ViewData.ModelState.AddModelError("CaptchaInputText", CookieMustEnabled);
                return;
            }

            string decryptedString = "";
            try
            {
                decryptedString = httpCookie.Value.Decrypt(DecryptionKey + DateTime.Now.Date.ToString(CultureInfo.InvariantCulture));
            }
            catch (System.Security.Cryptography.CryptographicException exception)
            {
                controllerBase.ViewData.ModelState.AddModelError("CaptchaInputText", ErrorWasHappened);
                return;
            }
            
            var arr = decryptedString.Split(',');
            var originalCaptchaNumber = arr[0];
            var generatedCaptchaDateTime = arr[1];

            int num;
            var dt = new DateTime();
            if (originalCaptchaNumber == "" || generatedCaptchaDateTime == "" || !int.TryParse(originalCaptchaNumber, out num) ||
                !DateTime.TryParse(generatedCaptchaDateTime, out dt))
            {
                controllerBase.ViewData.ModelState.AddModelError("CaptchaInputText", ErrorWasHappened);
                return;
            }

            var secondsDiff = (DateTime.Now - DateTime.Parse(generatedCaptchaDateTime)).TotalSeconds;
            if (secondsDiff > ExpireTimeCaptchaCodeBySeconds)
            {
                controllerBase.ViewData.ModelState.AddModelError("CaptchaInputText", TimeIsExpired);
                return;
            }

            if (inputText != originalCaptchaNumber)
            {
                controllerBase.ViewData.ModelState.AddModelError("CaptchaInputText", CaptchaCodeIsIncorrect);
                return;
            }

            HttpContext.Current.Response.Cookies.Remove("captchastring");
        }
        
        #endregion
    }
}
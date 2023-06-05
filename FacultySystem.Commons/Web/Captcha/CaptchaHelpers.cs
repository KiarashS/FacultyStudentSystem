using System;

namespace ContentManagementSystem.Commons.Web.Captcha
{
    public static class CaptchaHelpers
    {
        public static int CreateSalt()
        {
            var random = new Random();
            return random.Next(1000, 9999);
        }
    }
}
using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace ContentManagementSystem.Commons.Web
{
    public static class Extensions
    {
        public static string RemovePrefix(this string input, string prefix)
        {
            if (prefix == null) return input;
            return !input.StartsWith(prefix) ? input : input.Remove(0, prefix.Length);
        }

        public static string RemoveSuffix(this string input, string suffix)
        {
            if (suffix == null) return input;
            return !input.EndsWith(suffix) ? input : input.Remove(input.Length - suffix.Length, suffix.Length);
        }

        public static DateTime UtcToLocalDateTime(this DateTime utcDateTime, string zone = "Iran Standard Time")
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(zone);

            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, timeZoneInfo);
        }

        public static DateTime LocalToUtcDateTime(this DateTime localDateTime, string zone = "Iran Standard Time")
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(zone);

            return TimeZoneInfo.ConvertTimeToUtc(localDateTime, timeZoneInfo);
        }

        public static Task<T> WithTimeout<T>(this Task<T> task, int duration)
        {
            return Task.Factory.StartNew(() =>
            {
                try
                {
                    var b = task.Wait(duration);
                    return b ? task.Result : default(T);
                }
                catch (AggregateException e)
                {
                    return default(T);
                }
            });
        }

        public static string GetMd5Hash(this string strToHash)
        {
            var bytToHash = Encoding.ASCII.GetBytes(strToHash);
            var tmpHash = (new MD5CryptoServiceProvider()).ComputeHash(bytToHash);
            var i = 0;
            var sOutput = new StringBuilder(tmpHash.Length);
            for (i = 0; i <= tmpHash.Length - 1; i++)
            {
                sOutput.Append(tmpHash[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        public static string NormalizeDoi(this string doi)
        {
            if (doi.StartsWith("/"))
                doi = doi.Remove(0, 1);

            if (doi.EndsWith("/"))
                doi = doi.Remove(doi.Length-1, 1);

            return doi.StartsWith("10") ? doi : null;
        }

        //public static string Truncate(this string value, int maxLength)
        //{
        //    if (string.IsNullOrEmpty(value)) return value;
        //    return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        //}
    }

    public static class RelativeTimeCalculator
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;

        public static string CalculateRelativeTime(this DateTime dateTime)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - dateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);
            if (delta < 1 * MINUTE)
            {
                return ts.Seconds == 1 ? "لحظه ای قبل" : ts.Seconds + " ثانیه قبل";
            }
            if (delta < 2 * MINUTE)
            {
                return "یک دقیقه قبل";
            }
            if (delta < 45 * MINUTE)
            {
                return ts.Minutes + " دقیقه قبل";
            }
            if (delta < 90 * MINUTE)
            {
                return "یک ساعت قبل";
            }
            if (delta < 24 * HOUR)
            {
                return ts.Hours + " ساعت قبل";
            }
            if (delta < 48 * HOUR)
            {
                return "دیروز";
            }
            if (delta < 30 * DAY)
            {
                return ts.Days + " روز قبل";
            }
            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "یک ماه قبل" : months + " ماه قبل";
            }
            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return years <= 1 ? "یک سال قبل" : years + " سال قبل";
        }

        public static string TruncateAtWord(this string value, int length)
        {
            if (value == null || value.Length < length || value.IndexOf(" ", length) == -1)
                return value;

            return string.Format("{0}...", value.Substring(0, value.IndexOf(" ", length)));
        }
    }
}

namespace System.Web.Mvc
{
    public static class EngineFilter
    {
        public static RazorViewEngine DisableVbhtml(this RazorViewEngine engine)
        {
            engine.AreaViewLocationFormats = FilterOutVbhtml(engine.AreaViewLocationFormats);
            engine.AreaMasterLocationFormats = FilterOutVbhtml(engine.AreaMasterLocationFormats);
            engine.AreaPartialViewLocationFormats = FilterOutVbhtml(engine.AreaPartialViewLocationFormats);
            engine.ViewLocationFormats = FilterOutVbhtml(engine.ViewLocationFormats);
            engine.MasterLocationFormats = FilterOutVbhtml(engine.MasterLocationFormats);
            engine.PartialViewLocationFormats = FilterOutVbhtml(engine.PartialViewLocationFormats);
            engine.FileExtensions = FilterOutVbhtml(engine.FileExtensions);
            return engine;
        }

        private static string[] FilterOutVbhtml(string[] source)
        {
            return source.Where(s => !s.Contains("vbhtml")).ToArray();
        }
    }
}


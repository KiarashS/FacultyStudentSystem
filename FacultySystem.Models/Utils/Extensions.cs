
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ContentManagementSystem.Models.Utils
{
    public static class Extensions
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;
        private const int MaxLenghtSlug = 100;

        public static string CalculateRelativeTime(this DateTime dateTime)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - dateTime.Ticks);
            PersianCalendar pc = new PersianCalendar();
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
                return ts.Days + " روز قبل" + $" ({pc.GetYear(dateTime)}/{pc.GetMonth(dateTime)}/{pc.GetDayOfMonth(dateTime)})";
            }
            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return (months <= 1 ? "یک ماه قبل" : months + " ماه قبل") + $" ({pc.GetYear(dateTime)}/{pc.GetMonth(dateTime)}/{pc.GetDayOfMonth(dateTime)})";
            }
            int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
            return (years <= 1 ? "یک سال قبل" : years + " سال قبل") + $" ({pc.GetYear(dateTime)}/{pc.GetMonth(dateTime)}/{pc.GetDayOfMonth(dateTime)})";
        }

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

        public static DateTimeOffset UtcToLocalDateTimeOffset(this DateTimeOffset utcDateTime, string zone = "Iran Standard Time")
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(zone);

            return TimeZoneInfo.ConvertTime(utcDateTime, timeZoneInfo);
        }

        public static DateTimeOffset LocalToUtcDateTimeOffset(this DateTimeOffset localDateTime)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

            return TimeZoneInfo.ConvertTime(localDateTime, timeZoneInfo);
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
        
        public static string GenerateSlug(string title)
        {
            var slug = RemoveAccent(title).ToLower();
            slug = Regex.Replace(slug, @"[^a-z0-9-\u0600-\u06FF]", "-");
            slug = Regex.Replace(slug, @"\s+", "-").Trim();
            slug = Regex.Replace(slug, @"-+", "-");
            slug = slug.Substring(0, slug.Length <= MaxLenghtSlug ? slug.Length : MaxLenghtSlug).Trim();

            return slug;
        }

        private static string RemoveAccent(string text)
        {
            var bytes = Encoding.GetEncoding("UTF-8").GetBytes(text);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string TruncateAtWord(this string value, int length)
        {
            if (value == null || value.Length < length || value.IndexOf(" ", length, StringComparison.InvariantCultureIgnoreCase) == -1)
                return value;

            return string.Format("{0}...", value.Substring(0, value.IndexOf(" ", length, StringComparison.InvariantCultureIgnoreCase)));
        }
    }
}

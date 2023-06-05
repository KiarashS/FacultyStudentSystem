using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ContentManagementSystem.Commons.Web
{
    public static class Cookies
    {
        private const int MaxInt = 99999999;
        private static readonly string CookieTextPath;
        private static readonly string CookieLogPath;

        private readonly static List<CookieSet> CookieList = new List<CookieSet>();

        static Cookies()
        {
            CookieTextPath = HttpContext.Current.Server.MapPath("~/App_Data/Temp/Cookie/Cookie.txt");
            CookieLogPath = HttpContext.Current.Server.MapPath("~/App_Data/Temp/Cookie/CookieLog.txt");
            AddCookie();
        }
        public static Task<CookieSet> GetCookieAsync()
        {
            if (CookieList.Count == 0)
                AddCookie();
            return SelectCookie();
        }

        private async static Task<bool> IsValid(CookieSet cookie)
        {
            if (!cookie.IsValid)
                return false;
            if (cookie.Cookie.IsNullOrEmpty())
                return false;
            
            try
            {
                var request = (HttpWebRequest)WebRequest.Create("http://www.sciencedirect.com." + cookie.Domain);
                request.Headers.Add(cookie.Cookie);
                request.AllowAutoRedirect = false;
                request.Method = "Head";
                var response = (HttpWebResponse)await request.GetResponseAsync();
                if (response.StatusCode == HttpStatusCode.Found)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message == "The remote server returned an error: (503) Server Unavailable.")
                {
                    return true;
                }
                throw;
            }
        }

        private static void UpdateCookies(CookieSet cookie)
        {
            Cookies.Remove(cookie);
            WriteToFile(cookie, "Removed");
            AddCookie();
        }

        private static void Remove(CookieSet cookie)
        {
            var oldLines = File.ReadAllLines(CookieTextPath);
            File.WriteAllText(CookieTextPath, string.Empty);
            var fileStream = new StreamWriter(CookieTextPath);
            foreach (var line in oldLines)
            {
                if (!cookie.Cookie.Contains(line.Split("@")[0], StringComparison.OrdinalIgnoreCase))
                    fileStream.WriteLine(line);
            }
            fileStream.Close();
            CookieList.Remove(cookie);
        }

        public static bool AddCookie()
        {
            using (var fileStream = new StreamReader(CookieTextPath))
            {
                string line;
                while ((line = fileStream.ReadLine()) != null)
                {
                    if (line.IsNullOrEmpty())
                        continue;
                    var info = line.Split("@");
                    var cookie = new CookieSet();
                    cookie.Cookie = "Cookie:" + info[0];
                    cookie.Domain = info[1];
                    cookie.UniversityName = info[2];
                    cookie.IsValid = true;
                    cookie.InUseCount = 0;
                    if (CookieList.Contains(cookie))
                        continue;
                    CookieList.Add(cookie);
                    WriteToFile(cookie, "Added");
                    CookieList.Sort();
                }
                fileStream.Close();
                return true;
            }
        }

        private async static Task<CookieSet> SelectCookie()
        {
            if (CookieList.Count == 0)
            {
                throw new HttpException(604, "All the cookies are invalid");
            }
            var cookieSet = CookieList[0];

            if (await IsValid(cookieSet))
            {
                cookieSet.InUseCount++;
                CookieList.Sort();
                return cookieSet;
            }

            cookieSet.IsValid = false;
            cookieSet.InUseCount = MaxInt;
            CookieList.Sort();
            UpdateCookies(cookieSet);
            return await SelectCookie();
        }

        public static void DecreaseCookie(CookieSet cookie)
        {
            if (cookie.InUseCount > 0)
                cookie.InUseCount--;
        }

        private static void WriteToFile(CookieSet cookie, string info)
        {
            string textToWrite = info + "      " + DateTime.UtcNow.UtcToLocalDateTime() + "      " + cookie.UniversityName + "      " + cookie.Cookie;
            var file = new StreamWriter(CookieLogPath, true);

            if (file.IsNotNull())
            {
                file.WriteLine(textToWrite);
                file.Close();
            }

        }

    }

    public class CookieSet : IComparable<CookieSet>, IEquatable<CookieSet>
    {
        public string Cookie;
        public string Domain;
        public string UniversityName;
        public int InUseCount;
        public bool IsValid = true;

        public int CompareTo(CookieSet other)
        {
            return InUseCount.CompareTo(other.InUseCount);
        }

        public bool Equals(CookieSet other)
        {
            return Cookie.Contains(other.Cookie);
        }
    }
}
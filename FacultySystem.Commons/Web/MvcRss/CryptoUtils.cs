﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace ContentManagementSystem.Commons.Web.MvcRss
{
    public static class CryptoUtils
    {
        public static string SHA1(this string data)
        {
            using (var sha1 = new SHA1Managed())
            {
                var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));
                return BitConverter.ToString(hash).Replace("-", "").ToUpperInvariant();
            }
        }
    }
}

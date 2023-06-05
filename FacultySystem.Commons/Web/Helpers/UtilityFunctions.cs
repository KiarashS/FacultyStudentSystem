//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Web;
//using ContentManagementSystem.Models.DownloaderModels;

//namespace ContentManagementSystem.Commons.Web.Helpers
//{
//    public static class UtilityFunctions
//    {
//        public static Tuple<string, string> GetRandomSugarDomain()
//        {
//            var randomGenerator = new Random();

//            var randomNo = randomGenerator.Next(2);

//            switch (randomNo)
//            {
//                case 0:
//                    return new Tuple<string, string>("http://www.sciencedirect.com", "sciencedirect");

//                case 1:
//                    return new Tuple<string, string>("http://link.springer.com", "springer");

//                //case 2:
//                //    return new Tuple<string, string>("http://link.springer.com", "springer");
//            }
//            return new Tuple<string, string>("http://www.sciencedirec.com", "sciencedirect");
//        }

//        public static List<UniversitySourceAccessibilities> SortAccessibilitieses(this IList<UniversitySourceAccessibilities> accessibilitieses)
//        {
//            var newAccessebilities = accessibilitieses.Take(1).ToList();
//            newAccessebilities.AddRange(
//                accessibilitieses.Skip(1)
//                    .OrderByDescending(acc => acc.Access)
//                    .ToList());

//            return newAccessebilities;
//        }

//        public static string GetUnblockedIp()
//        {
//            string[] ipAddress = {"149.202.142.189", "149.202.142.181", "151.80.19.36"};

//            var ipNo = new Random();

//            return ipAddress[ipNo.Next(0,ipAddress.Length)];
//        }

//        public static string GetFilePath(string md5, string sourceName)
//        {
//            var path = String.Format("E:/pdf/Articles/{0}/{1}{2}", sourceName, md5, Constants.PdfExtension);

//            if (File.Exists(path))
//                return path.RemoveSuffix(Constants.PdfExtension);

//            path = String.Format("D:/pdf/Articles/{0}/{1}{2}", sourceName, md5, Constants.PdfExtension);

//            if (File.Exists(path))
//                return path.RemoveSuffix(Constants.PdfExtension);

//            return null;
//        }
//        public static string GetThesisFilePath(string fileName, string sourceName)
//        {
//            var path = String.Format("E:/pdf/Thesis/{0}/{1}{2}", sourceName, fileName, Constants.PdfExtension);

//            if (File.Exists(path))
//                return path.RemoveSuffix(Constants.PdfExtension);

//            path = String.Format("D:/pdf/Thesis/{0}/{1}{2}", sourceName, fileName, Constants.PdfExtension);

//            if (File.Exists(path))
//                return path.RemoveSuffix(Constants.PdfExtension);

//            return null;
//        }

//        public static string GetEbookFilePath(string filePath, string publisher)
//        {
//            if (publisher.IsNullOrWhiteSpace())
//                publisher = "other";

//            var path = String.Format("E:/pdf/Ebook/{0}/{1}{2}", publisher, filePath, Constants.PdfExtension);

//            if (File.Exists(path))
//                return path.RemoveSuffix(Constants.PdfExtension);

//            path = String.Format("D:/pdf/Ebook/{0}/{1}{2}", publisher, filePath, Constants.PdfExtension);

//            if (File.Exists(path))
//                return path.RemoveSuffix(Constants.PdfExtension);

//            return null;
//        }

//        public static string MapPathReverse(string fullServerPath)
//        {
//            return @"~\" + fullServerPath.Replace(HttpContext.Current.Request.PhysicalApplicationPath, string.Empty);
//        }

//        internal static string GetStandardFilePath(string filePath, string sourceName)
//        {
//            var path = String.Format("E:/pdf/Standard/{0}/{1}{2}", sourceName, filePath, Constants.PdfExtension);

//            if (File.Exists(path))
//                return path.RemoveSuffix(Constants.PdfExtension);

//            path = String.Format("D:/pdf/Standard/{0}/{1}{2}", sourceName, filePath, Constants.PdfExtension);

//            if (File.Exists(path))
//                return path.RemoveSuffix(Constants.PdfExtension);

//            return null;
//        }
//    }
//}
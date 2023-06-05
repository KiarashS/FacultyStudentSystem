using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ContentManagementSystem.Web.Utils
{
    public static class FilenameToFilePath
    {
        public static string GetGalleryVideoPath(int userId, string filename, long galleryId)
        {
            return HostingEnvironment.MapPath("~") + @"App_Data\UsersFiles\" + userId + @"\GalleryFiles\" + galleryId + @"\" + filename;
        }
    }
}

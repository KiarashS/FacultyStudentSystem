using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using NReco.VideoConverter;

namespace ContentManagementSystem.Commons.Web
{
    public static class VideoThumb
    {
        public static string CreateThumb(string pathToVideoFile, float frameTime = 10)
        {
            var ffMpeg = new FFMpegConverter();
            var imageStream = new MemoryStream();

            ffMpeg.GetVideoThumbnail(pathToVideoFile, imageStream, frameTime);

            return ImageToBase64(imageStream);
        }

        private static string ImageToBase64(MemoryStream stream)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(stream))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    return Convert.ToBase64String(imageBytes);
                }
            }
        }
    }
}

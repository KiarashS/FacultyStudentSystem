    using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace ContentManagementSystem.Commons.Web.Captcha
{
    public class CaptchaImageResult : ActionResult
    {
        #region Fields (6)

        private const int Height = 60;
        private const int Width = 250;
        private readonly Color _backGroundColor = ColorTranslator.FromHtml("#FFF"); //Color.White;//Color.FromArgb(255, 239, 239, 239);
        private const string EncryptionKey = "AdflDpdcmfiZSjnSPEQNjFD5lsdf43DdimnqwejdksfSDaA4HSsasdS12qweW3Fod48a9sdfkwe";
        private const string CaptchaFontFamily = "Tahoma";
        private const int CaptchaFontSize = 10;

        #endregion

        #region Methods (1)

        public override void ExecuteResult(ControllerContext context)
        {
            var bitmap = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            var gfxCaptchaImage = Graphics.FromImage(bitmap);

            gfxCaptchaImage.PageUnit = GraphicsUnit.Pixel;
            gfxCaptchaImage.SmoothingMode = SmoothingMode.HighQuality;
            gfxCaptchaImage.Clear(_backGroundColor);

            var salt = CaptchaHelpers.CreateSalt();
            var encryptionSaltKey = EncryptionKey + DateTime.Now.Date.ToString(CultureInfo.InvariantCulture);
            var plainText = salt.ToString(CultureInfo.InvariantCulture) + "," + DateTime.Now.ToString(CultureInfo.InvariantCulture);
            var encryptedValue = (plainText).Encrypt(encryptionSaltKey);
            var cookie = new HttpCookie("captchastring") {Value = encryptedValue};
            HttpContext.Current.Response.Cookies.Add(cookie);

            var randomString = (salt).NumberToText(Language.Persian);
            var format = new StringFormat();
            var faLcid = new CultureInfo("fa-IR").LCID;
            format.SetDigitSubstitution(faLcid, StringDigitSubstitute.National);
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            format.FormatFlags = StringFormatFlags.DirectionRightToLeft;

            var font = new Font(CaptchaFontFamily, CaptchaFontSize);
            var path = new GraphicsPath();

            path.AddString(randomString,
                font.FontFamily,
                (int)font.Style,
                (gfxCaptchaImage.DpiY * font.SizeInPoints / 72),
                new Rectangle(0, 0, Width, Height),
                format);

            var random = new Random();

            var pen = new Pen(Color.FromArgb(random.Next(0, 100), random.Next(0, 100), random.Next(0, 100)));
            gfxCaptchaImage.DrawPath(pen, path);

            var distortion = random.Next(-10, 10);
            using (var copy = (Bitmap)bitmap.Clone())
            {
                for (var y = 0; y < Height; y++)
                {
                    for (var x = 0; x < Width; x++)
                    {
                        var newX = (int)(x + (distortion * Math.Sin(Math.PI * y / 64.0)));
                        var newY = (int)(y + (distortion * Math.Cos(Math.PI * x / 64.0)));
                        if (newX < 0 || newX >= Width) newX = 0;
                        if (newY < 0 || newY >= Height) newY = 0;
                        bitmap.SetPixel(x, y, copy.GetPixel(newX, newY));
                    }
                }
            }

            int i, r, xx, yy, u, v;
            for (i = 1; i < 10; i++)
            {
                pen.Color = Color.FromArgb((random.Next(0, 255)), (random.Next(0, 255)), (random.Next(0, 255)));
                r = random.Next(0, (Width / 3));
                xx = random.Next(0, Width);
                yy = random.Next(0, Height);
                u = xx - r;
                v = yy - r;
                gfxCaptchaImage.DrawEllipse(pen, u, v, r, r);
            }

            gfxCaptchaImage.DrawImage(bitmap, new Point(0, 0));
            gfxCaptchaImage.Flush();

            var response = context.HttpContext.Response;
            response.ContentType = "image/jpeg";
            context.HttpContext.DisableBrowserCache();
            bitmap.Save(response.OutputStream, ImageFormat.Jpeg);

            font.Dispose();
            gfxCaptchaImage.Dispose();
            bitmap.Dispose();
        }

        #endregion
    }
}

<%@ WebHandler Language="C#" Class="ImageGenerator" %>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Web;
using ContentManagementSystem.Commons.Web;

public class ImageGenerator : IHttpHandler {
    private static string d_fontFamily = "Arial";
    private static int d_fontSize = 10;
    private static bool d_bold = false;
    private static bool d_italic = false;
    private static string d_color = "Black";
    private static string d_bgColor = "Transparent";
    private static int d_rotation = 0;
    private static int d_underlineWidth = 0;
    private static string d_underlineColor = "Black";
    private static bool d_usePng = false;

    public void ProcessRequest (HttpContext context) {
        string fontFamily = context.Request.QueryString["FontFamily"] ?? d_fontFamily;
        int fontSize = Int32.Parse(context.Request.QueryString["FontSize"] ?? d_fontSize.ToString());
        bool bold = Boolean.Parse(context.Request.QueryString["Bold"] ?? d_bold.ToString());
        bool italic = Boolean.Parse(context.Request.QueryString["Italic"] ?? d_italic.ToString());
        string color = context.Request.QueryString["TextColor"] ?? d_color;
        string bgColor = context.Request.QueryString["BackgroundColor"] ?? d_bgColor;
        int rotation = Int32.Parse(context.Request.QueryString["Rotation"] ?? d_rotation.ToString());
        int underlineWidth = Int32.Parse(context.Request.QueryString["UnderlineWidth"] ?? d_underlineWidth.ToString());
        string underlineColor = context.Request.QueryString["UnderlineColor"] ?? d_underlineColor;
        bool usePng = Boolean.Parse(context.Request.QueryString["UsePng"] ?? d_usePng.ToString());

        string code = "";
        if (context.Request.QueryString["Code"] == null)
        {
            //text = "Supported parameters: Text,FontFamily,FontSize,Bold,Italic,TextColor,BackgroundColor,Rotation,UnderlineWidth,UnderlineColor,UsePng\nExample: TextImage.ashx?Text=LINQ%20Rocks!&FontFamily=Arial&FontSize=18&Bold=True&Italic=False&TextColor=Black&BackgroundColor=Transparent&Rotation=90&UnderlineWidth=0&UnderlineColor=%23FF0000&UsePng=False";
            code = "Not valid request!";
            fontFamily = d_fontFamily;
            fontSize = d_fontSize;
            bold = d_bold;
            italic = d_italic;
            color = d_color;
            bgColor = d_bgColor;
            rotation = d_rotation;
            underlineWidth = d_underlineWidth;
            underlineColor = d_underlineColor;
            usePng = d_usePng;
        }
        else
        {
            code = context.Request.QueryString["Code"] ?? "Text not passed.";
        }

        if (code != "Not valid request!" || code != "Text not passed.")
        {
            code = RijndaelManagedEncryption.DecryptRijndael(code);
        }

        Byte[] buffer =
            CreateTextImage(code, fontFamily, fontSize, bold, italic, color, bgColor, rotation, underlineWidth, underlineColor, usePng);

        context.Response.ContentType = usePng ? "image/png" : "image/gif";
        context.Response.BinaryWrite(buffer);
        context.Response.Flush();
    }

    public bool IsReusable {
        get {
            return true;
        }
    }

    private byte[] CreateTextImage(string text, string fontFamily, int fontSize, bool bold, bool italic, string color, string bgColor, int rotation, int underlineWidth, string underlineColor, bool usePng)
    {
        float textWidth = 0;
        float textHeight = 0;

        FontStyle fontStyle = FontStyle.Regular;
        if (bold)
            fontStyle |= FontStyle.Bold;
        if (italic)
            fontStyle |= FontStyle.Italic;

        if (color.StartsWith("Sharp-", StringComparison.InvariantCulture))
        {
                color = color.Replace("Sharp-", "#");
        }

        using (Font font = new Font(fontFamily, fontSize, fontStyle))
        {
            using (Bitmap bitmap = new Bitmap(1, 1))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    textWidth = graphics.MeasureString(text, font).Width;
                    textHeight = graphics.MeasureString(text, font).Height;
                }
            }

            //  Calculate the needed bitmap measurements based on the text measurements.
            int bitmapWidth = GetRotatedRectangleWidth(textWidth, textHeight, rotation);
            int bitmapHeight = GetRotatedRectangleHeight(textWidth, textHeight, rotation);

            //  Now I create the real bitmap of the necessary size to fit the text.
            using (Bitmap bitmap = new Bitmap(bitmapWidth, bitmapHeight))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    if (!usePng)
                    {
                        graphics.Clear(Color.FromArgb(255, 255, 255, 204));//Color.FromArgb(&HFF, &HFF, &HFF, &HCC)
                        //graphics.Clear(Color.Transparent);
                    }

                    //  Since I will be rotating text, I need to move the rotation point using the TranslateTransform.
                    int x, y;
                    //  But first I need to know the location of the rotation point.
                    GetXY(rotation, textWidth, textHeight, out x, out y);
                    graphics.TranslateTransform(x, y);

                    //  Now rotate and draw the text.
                    graphics.RotateTransform(rotation);

                    //  Fill in the background color
                    using (Brush brush = new SolidBrush(ColorTranslator.FromHtml(bgColor)))
                    {
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        graphics.FillRectangle(brush, 0, 0, textWidth, textHeight);
                    }

                    graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
                    using (Brush brush = new SolidBrush(ColorTranslator.FromHtml(color)))
                    {
                        graphics.DrawString(text, font, brush, 0, 0);
                    }

                    if (underlineWidth > 0)
                    {
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        using (Pen pen = new Pen(ColorTranslator.FromHtml(underlineColor), underlineWidth))
                        {
                            graphics.DrawLine(pen, 0, textHeight, textWidth, textHeight);
                        }
                    }

                    graphics.Flush();

                    MemoryStream m = new MemoryStream();
                    bitmap.Save(m, usePng ? ImageFormat.Png : ImageFormat.Gif);

                    if (usePng)
                    {
                        return m.ToArray();
                    }
                    else
                    {
                        // transparency hack.
                        byte[] n = { };
                        n = m.ToArray();
                        n[787] = 254;
                        return n;
                    }
                } // using graphics
            } // using bitmap
        } // using font
    }

    private void GetXY(int rotation, float tw, float th, out int xT, out int yT)
    {
        xT = 0;
        yT = 0;

        double radians = GetRadians(GetReferenceAngleForPositioning(rotation));

        if (rotation >= 0 && rotation <= 90)
        {
            xT = Convert.ToInt32(Math.Sin(radians) * th);
        }
        else if (rotation > 90 && rotation <= 180)
        {
            xT = Convert.ToInt32((Math.Sin(radians) * tw) + (Math.Cos(radians) * th));
            yT = Convert.ToInt32(Math.Sin(radians) * th);
        }
        else if (rotation > 180 && rotation <= 270)
        {
            xT = Convert.ToInt32(Math.Cos(radians) * tw);
            yT = Convert.ToInt32((Math.Cos(radians) * th) + (Math.Sin(radians) * tw));
        }
        else
        {
            yT = Convert.ToInt32(Math.Sin(radians) * tw);
        }
    }

    private int GetRotatedRectangleWidth(float width, float height, int rotation)
    {
        return GetRotatedRectangleWidth(width, height, GetRadians(GetReferenceAngleForSizing(rotation)));
    }

    private int GetRotatedRectangleWidth(float width, float height, double rotationInRadians)
    {
        double w1 = width * Math.Cos(rotationInRadians);
        double w2 = height * Math.Sin(rotationInRadians);

        return Convert.ToInt32(Math.Ceiling(w1 + w2));
    }

    private int GetRotatedRectangleHeight(float width, float height, int rotation)
    {
        return GetRotatedRectangleHeight(width, height, GetRadians(GetReferenceAngleForSizing(rotation)));
    }

    private int GetRotatedRectangleHeight(float width, float height, double rotationInRadians)
    {
        double h1 = width * Math.Sin(rotationInRadians);
        double h2 = height * Math.Cos(rotationInRadians);

        return Convert.ToInt32(Math.Ceiling(h1 + h2));
    }

    private double GetRadians(double referenceAngle)
    {
        return Math.PI * referenceAngle / 180.0;
    }

    //  The only difference between the reference angles for sizing and positioning is in the 3rd quadrant (> 90 <= 180).
    private double GetReferenceAngleForSizing(int rotationInDegrees)
    {
        if (rotationInDegrees >= 0 && rotationInDegrees <= 90)
            return rotationInDegrees;
        else if (rotationInDegrees > 90 && rotationInDegrees <= 180)
            return 180 - rotationInDegrees;
        else if (rotationInDegrees > 180 && rotationInDegrees <= 270)
            return rotationInDegrees - 180;
        else if (rotationInDegrees > 270 && rotationInDegrees <= 360)
            return 360 - rotationInDegrees;
        else
            return rotationInDegrees;
    }

    private double GetReferenceAngleForPositioning(int rotationInDegrees)
    {
        if (rotationInDegrees >= 0 && rotationInDegrees <= 90)
            return rotationInDegrees;
        else if (rotationInDegrees > 90 && rotationInDegrees <= 180)
            return rotationInDegrees - 90;
        else if (rotationInDegrees > 180 && rotationInDegrees <= 270)
            return rotationInDegrees - 180;
        else if (rotationInDegrees > 270 && rotationInDegrees <= 360)
            return 360 - rotationInDegrees;
        else
            return rotationInDegrees;
    }
}
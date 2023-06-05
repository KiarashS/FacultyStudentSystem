//using System.IO;
//using System.Web;
//using iTextSharp.text;
//using iTextSharp.text.pdf;

//namespace ContentManagementSystem.Commons.Web.Helpers
//{
//    public class PdfHelpers
//    {
//        public static void AddWatermark(string pdfFile, string watermarkedFile, string watermarkText)
//        {
//            FontFactory.Register(HttpContext.Current.Server.MapPath("~/Content/fonts/MICROSS.TTF"));
//            var tahoma = FontFactory.GetFont("MICROSS", BaseFont.IDENTITY_H, 40, Font.NORMAL, BaseColor.BLACK);
            
//            var reader = new PdfReader(pdfFile);
//            using (var fileStream = new FileStream(watermarkedFile, FileMode.Create, FileAccess.Write, FileShare.None))
//            {
//                using (var stamper = new PdfStamper(reader, fileStream))
//                {
//                    int pageCount = reader.NumberOfPages;
//                    for (int i = 1; i <= pageCount; i++)
//                    {
//                        var rect = reader.GetPageSize(i);
//                        var cb = stamper.GetUnderContent(i);
//                        var gState = new PdfGState();
//                        gState.FillOpacity = 0.25f;
//                        cb.SetGState(gState);

//                        ColumnText.ShowTextAligned(
//                            canvas: cb,
//                            alignment: Element.ALIGN_CENTER,
//                            phrase: new Phrase(watermarkText, tahoma),
//                            x: rect.Width / 2,
//                            y: rect.Height / 2,
//                            rotation: 45f,
//                            runDirection: PdfWriter.RUN_DIRECTION_LTR,
//                            arabicOptions: 0);
//                    }
//                }
//            }
//        }

//        public static void AddFooter(string pdfFile, string watermarkedFile, string footerText)
//        {
//            PdfReader.unethicalreading = true;
//            var reader = new PdfReader(pdfFile);
//            reader.RemoveUnusedObjects();

//            using (var fileStream = new FileStream(watermarkedFile, FileMode.Create, FileAccess.Write, FileShare.None))
//            {
//                using (var stamper = new PdfStamper(reader, fileStream))
//                {
//                    for (var i = 1; i <= reader.NumberOfPages; i++)
//                    {
//                        GetFooterTable(footerText)
//                            .WriteSelectedRows(0, -1, (reader.GetPageSize(i).Width / 2) - 35, 15,
//                                stamper.GetOverContent(i));
//                    }

//                    stamper.SetFullCompression();
//                    stamper.Close();
//                }
//            }

//            reader.Close();
//        }


//        #region Private Methods (1)

//        private static PdfPTable GetFooterTable(string footerText)
//        {
//            var footer = new Paragraph(footerText,
//                FontFactory.GetFont(FontFactory.TIMES, 8, Font.NORMAL, BaseColor.BLACK));

//            var footerTbl = new PdfPTable(1) { TotalWidth = 300 };

//            var cell = new PdfPCell(footer) { Border = 0 };

//            footerTbl.AddCell(cell);

//            return footerTbl;
//        }

//        #endregion Private Methods
//    }
//}
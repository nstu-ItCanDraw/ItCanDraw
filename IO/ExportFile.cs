using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Serialization;

namespace IO
{
    [Serializable]
    public class BadFileExtensionError : Exception
    {
        public BadFileExtensionError(string message) : base(message)
        { }

        protected BadFileExtensionError(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
        { }
    }

    public static class ExportFile
    {
        public static void ToSVG(string filename, object document)
        {
            var svgDoc = SVG.GetSvgDocumentFromDocument(document);
            string svgString = SVG.GetStringFromSvgDocument(svgDoc);

            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(svgString);
            }
        }

        public static void ToHTML(string filename, object document)
        {
            // TODO: не использовать строки для работы с путями
            // TODO: валидировать путь нормально
            if (!(filename.EndsWith(".html") || filename.EndsWith(".htm")))
            {
                throw new BadFileExtensionError("File extension must be .htm or .html");
            }

            var svgDoc = SVG.GetSvgDocumentFromDocument(document);
            string svgString = SVG.GetStringFromSvgDocument(svgDoc);
            var htmlstring = HTML.GetHTMLStringFromSVGString(svgString, filename);

            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(htmlstring);
            }
        }

        public static void ToPDF(string filename, object document)
        {
            if (!filename.EndsWith(".pdf"))
            {
                throw new BadFileExtensionError("File extension must be .pdf");
            }

            var svgDoc = SVG.GetSvgDocumentFromDocument(document);
            string svgString = SVG.GetStringFromSvgDocument(svgDoc);
            var htmlstring = HTML.GetHTMLStringFromSVGString(svgString, filename);

            var renderer = new IronPdf.ChromePdfRenderer();
            var pdf = renderer.RenderHtmlAsPdf(htmlstring);
            pdf.MetaData.CustomProperties.Add("svgImage", svgString);
            pdf.SaveAs(filename);
        }

        public static void ToPNG(string filename, object document)
        {
            var svgDoc = SVG.GetSvgDocumentFromDocument(document);

            svgDoc.Draw().Save(filename, ImageFormat.Png);
        }

        public static void ToJPEG(string filename, object document)
        {
            var svgDoc = SVG.GetSvgDocumentFromDocument(document);

            svgDoc.Draw().Save(filename, ImageFormat.Jpeg);
        }

        public static void ToBMP(string filename, object document)
        {
            var svgDoc = SVG.GetSvgDocumentFromDocument(document);

            svgDoc.Draw().Save(filename, ImageFormat.Bmp);
        }

        public static void ToGIF(string filename, object document)
        {
            var svgDoc = SVG.GetSvgDocumentFromDocument(document);

            svgDoc.Draw().Save(filename, ImageFormat.Gif);
        }

        public static void ToTIFF(string filename, object document)
        {
            var svgDoc = SVG.GetSvgDocumentFromDocument(document);

            svgDoc.Draw().Save(filename, ImageFormat.Tiff);
        }

        public static void ToPNGFromBitmap(string filename, Bitmap bitmap)
        {
            bitmap.Save(filename, ImageFormat.Png);
        }

        public static void ToJPEGFromBitmap(string filename, Bitmap bitmap)
        {
            bitmap.Save(filename, ImageFormat.Jpeg);
        }

        public static void ToBMPFromBitmap(string filename, Bitmap bitmap)
        {
            bitmap.Save(filename, ImageFormat.Bmp);
        }

        public static void ToGIFFromBitmap(string filename, Bitmap bitmap)
        {
            bitmap.Save(filename, ImageFormat.Gif);
        }

        public static void ToTIFFFromBitmap(string filename, Bitmap bitmap)
        {
            bitmap.Save(filename, ImageFormat.Tiff);
        }
    }
}

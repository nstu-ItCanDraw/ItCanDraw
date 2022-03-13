using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace IO
{
    public static class ExportFile
    {
        public static object FromJSON(string filename)
        {
            string json_string;

            using (StreamReader reader = new StreamReader(filename))
            {
                json_string = reader.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<object>(json_string);
        }

        public static void ToJSON(string filename, object document)
        {
            string json_string = JsonConvert.SerializeObject(document);

            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(json_string);
            }
        }

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
            var svgDoc = SVG.GetSvgDocumentFromDocument(document);
            string svgString = SVG.GetStringFromSvgDocument(svgDoc);

            // Тимур: Обрамить SVG строку HTML, HEAD, TITLE, BODY тэгами и сохранить в filename

            var htmlstring = HTML.GetHTMLStringFromSVGString(svgString);// исправить
                                                                        // исправить
            using (StreamWriter writer = new StreamWriter(filename))    // исправить
            {                                                           // исправить
                writer.Write(htmlstring);                                // исправить
            }                                                           // исправить
        }

        public static void ToPDF(string filename, object document)
        {
            // Тимур: IronPDF
            var htmlstring = HTML.GetHTMLStringFromSVGString("");
            IronPdf.ChromePdfRenderer.StaticRenderHtmlAsPdf(htmlstring).SaveAs(filename);
        }

        public static void ToPNG(string filename, object document)
        {
            var svgDoc = SVG.GetSvgDocumentFromDocument(document);

            svgDoc.Draw().Save(filename, ImageFormat.Png);
            // Никита: сохранить в PNG и прочие растровые форматы
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

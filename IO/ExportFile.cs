using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using Logic;

namespace IO
{
    public static class ExportFile
    {

        public static void ToSVG(string filename, IDocument document)
        {
            FileValidator.CheckParentDirectory(filename);
            FileValidator.CheckExtension(filename, FileValidator.SVG_EXTENSION);

            var svgDoc = SVG.GetSvgDocumentFromDocument(document);
            string svgString = SVG.GetSvgStringFromSvgDocument(svgDoc);

            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(svgString);
            }
        }

        public static void ToHTML(string filename, IDocument document)
        {
            FileValidator.CheckParentDirectory(filename);
            FileValidator.CheckExtension(filename, FileValidator.HTML_EXTENSION);

            var svgDoc = SVG.GetSvgDocumentFromDocument(document);
            string svgString = SVG.GetSvgStringFromSvgDocument(svgDoc);
            var htmlstring = HTML.GetHTMLStringFromSVGString(svgString, filename);

            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(htmlstring);
            }
        }

        public static void ToPDF(string filename, IDocument document)
        {
            FileValidator.CheckParentDirectory(filename);
            FileValidator.CheckExtension(filename, FileValidator.PDF_EXTENSION);

            var svgDoc = SVG.GetSvgDocumentFromDocument(document);
            string svgString = SVG.GetSvgStringFromSvgDocument(svgDoc);
            var htmlstring = HTML.GetHTMLStringFromSVGString(svgString, filename);

            var renderer = new IronPdf.ChromePdfRenderer();
            var pdf = renderer.RenderHtmlAsPdf(htmlstring);
            pdf.MetaData.CustomProperties.Add("svgImage", svgString);
            pdf.SaveAs(filename);
        }

        public static void ToPNG(string filename, IDocument document)
        {
            FileValidator.CheckParentDirectory(filename);
            FileValidator.CheckExtension(filename, FileValidator.PNG_EXTENSION);

            var svgDoc = SVG.GetSvgDocumentFromDocument(document);

            svgDoc.Draw().Save(filename, ImageFormat.Png);
        }

        public static void ToJPEG(string filename, IDocument document)
        {
            FileValidator.CheckParentDirectory(filename);
            FileValidator.CheckExtension(filename, FileValidator.JPEG_EXTENSION);

            var svgDoc = SVG.GetSvgDocumentFromDocument(document);

            svgDoc.Draw().Save(filename, ImageFormat.Jpeg);
        }

        public static void ToBMP(string filename, IDocument document)
        {
            FileValidator.CheckParentDirectory(filename);
            FileValidator.CheckExtension(filename, FileValidator.BMP_EXTENSION);

            var svgDoc = SVG.GetSvgDocumentFromDocument(document);

            svgDoc.Draw().Save(filename, ImageFormat.Bmp);
        }

        public static void ToGIF(string filename, IDocument document)
        {
            FileValidator.CheckParentDirectory(filename);
            FileValidator.CheckExtension(filename, FileValidator.GIF_EXTENSION);

            var svgDoc = SVG.GetSvgDocumentFromDocument(document);

            svgDoc.Draw().Save(filename, ImageFormat.Gif);
        }

        public static void ToTIFF(string filename, IDocument document)
        {
            FileValidator.CheckParentDirectory(filename);
            FileValidator.CheckExtension(filename, FileValidator.TIFF_EXTENSION);

            var svgDoc = SVG.GetSvgDocumentFromDocument(document);

            svgDoc.Draw().Save(filename, ImageFormat.Tiff);
        }

        public static void ToPNGFromBitmap(string filename, Bitmap bitmap)
        {
            FileValidator.CheckParentDirectory(filename);
            FileValidator.CheckExtension(filename, FileValidator.PNG_EXTENSION);

            bitmap.Save(filename, ImageFormat.Png);
        }

        public static void ToJPEGFromBitmap(string filename, Bitmap bitmap)
        {
            FileValidator.CheckParentDirectory(filename);
            FileValidator.CheckExtension(filename, FileValidator.JPEG_EXTENSION);

            bitmap.Save(filename, ImageFormat.Jpeg);
        }

        public static void ToBMPFromBitmap(string filename, Bitmap bitmap)
        {
            FileValidator.CheckParentDirectory(filename);
            FileValidator.CheckExtension(filename, FileValidator.BMP_EXTENSION);

            bitmap.Save(filename, ImageFormat.Bmp);
        }

        public static void ToGIFFromBitmap(string filename, Bitmap bitmap)
        {
            FileValidator.CheckParentDirectory(filename);
            FileValidator.CheckExtension(filename, FileValidator.GIF_EXTENSION);

            bitmap.Save(filename, ImageFormat.Gif);
        }

        public static void ToTIFFFromBitmap(string filename, Bitmap bitmap)
        {
            FileValidator.CheckParentDirectory(filename);
            FileValidator.CheckExtension(filename, FileValidator.TIFF_EXTENSION);

            bitmap.Save(filename, ImageFormat.Tiff);
        }
    }
}

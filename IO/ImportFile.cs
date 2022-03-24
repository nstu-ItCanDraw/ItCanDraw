using System.IO;

using Svg;

using Logic;
using System.Text;
using HtmlAgilityPack;
using System.Linq;

namespace IO
{
    public static class ImportFile
    {
        public static IDocument FromSVG(string filename)
        {
            FileValidator.CheckFileExists(filename);
            FileValidator.CheckExtension(filename, FileValidator.SVG_EXTENSION);

            var svgDoc = SvgDocument.Open(filename);

            return SVG.GetDocumentFromSvgDocument(svgDoc);
        }

        public static IDocument FromHTML(string filename)
        {
            FileValidator.CheckFileExists(filename);
            FileValidator.CheckExtension(filename, FileValidator.HTML_EXTENSION);

            var htmlString = "";

            using (StreamReader streamReader = new StreamReader(filename, Encoding.UTF8))
            {
                htmlString = streamReader.ReadToEnd();
            }

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlString);

            var nodes = htmlDoc.DocumentNode.SelectNodes("/*[name()='svg']").ToArray();

            if (nodes.Length == 0)
            {
                throw new BadFileError("HTML file does not contain \"svg\" tag");
            }
            else if (nodes.Length > 1)
            {
                throw new BadFileError("HTML file contains more than one \"svg\" tag"); 
            }

            var svgString = nodes[0].ToString();
            var doc = SvgDocument.FromSvg<SvgDocument>(svgString);

            return SVG.GetDocumentFromSvgDocument(doc);
        }

        public static IDocument FromPDF(string filename)
        {
            FileValidator.CheckFileExists(filename);
            FileValidator.CheckExtension(filename, FileValidator.PDF_EXTENSION);

            var svgString = PDF.GetSvgFromPDF(filename);
            var doc = SvgDocument.FromSvg<SvgDocument>(svgString);

            return SVG.GetDocumentFromSvgDocument(doc);
        }
    }
}

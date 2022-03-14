using System.IO;

using Svg;

using Logic;

namespace IO
{
    public static class ImportFile
    {
        public static IDocument FromSVG(string filename)
        {
            var svgDoc = SvgDocument.Open(filename);

            return SVG.GetDocumentFromSvgDocument(svgDoc);
        }

        public static IDocument FromHTML(string filename)
        {
            return null;
        }

        public static IDocument FromPDF(string filename)
        {
            return null;
        }
    }
}

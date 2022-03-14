using System.IO;

using Svg;

namespace IO
{
    public static class ImportFile
    {
        public static object FromSVG(string filename)
        {
            var svgDoc = SvgDocument.Open(filename);

            return SVG.GetDocumentFromSvgDocument(svgDoc);
        }

        public static object FromHTML(string filename)
        {
            return null;
        }

        public static object FromPDF(string filename)
        {
            return null;
        }
    }
}

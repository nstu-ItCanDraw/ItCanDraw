using Microsoft.VisualStudio.TestTools.UnitTesting;
using Svg;

namespace IO.Test
{
    [TestClass()]
    public class HelpersTest
    {
        [TestMethod()]
        public void HTMLFromSVG_Test_HelloWorld()
        {
            string svgString = "Hello World";
            string title = "Test";
            string expected = "\r\n                <!DOCTYPE html>\r\n                <html lang=\"ru\">\r\n                    <head>\r\n                        <title>Test</title>\r\n                    </head>\r\n                    <body>\r\n                        Hello World\r\n                    </body>\r\n                </html>";
            //string expected = @"
            //    <!DOCTYPE html>
            //    <html lang=""ru"">
            //        <head>
            //            <title>Test</title>
            //        </head>
            //        <body>
            //            Hello World
            //        </body>
            //    </html>";

            string actual = HTML.GetHTMLStringFromSVGString(svgString, title);
            Assert.AreEqual(expected, actual);
        }

        [ExpectedException(typeof(BadPDFError))]
        [TestMethod()]
        public void SVGFromPDFExeption()
        {
            string filename = @"../../..TestPDF.pdf";

            PDF.GetSvgFromPDF(filename);
        }

        [TestMethod()]
        public void TestSVGFromDocument()
        {
            Logic.IDocument document = Logic.DocumentFactory.CreateDocument("SVG", 100, 100);
            SVG.GetSvgDocumentFromDocument(document);

            Assert.IsInstanceOfType(SVG.GetSvgDocumentFromDocument(document), typeof(SvgDocument));
        }

        [TestMethod()]
        public void TestSvgStringFromSvgDocument()
        {
            SvgDocument svg = new SvgDocument();

            Assert.IsInstanceOfType(SVG.GetSvgStringFromSvgDocument(svg), typeof(string));

        }
    }
}

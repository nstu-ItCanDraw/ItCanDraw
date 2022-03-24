using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IO.Test
{
    [TestClass()]
    public class ImportFileTest
    {
        [ExpectedException(typeof(NotSupportedException))]
        [TestMethod()]
        public void importFromSVGExeption()
        {
            string filename = @"C:..\..\..\molumen_Lenin_silhouette.svg";
            ImportFile.FromSVG(filename);
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod()]
        public void importFromHTMLExeption()
        {
            string filename = @"..\..\..\index.html";
            ImportFile.FromHTML(filename);
        }

        [ExpectedException(typeof(BadFileExtensionError))]
        [TestMethod()]
        public void importFromPDFExeption()
        {
            string filename = @"..\..\..\TestPDF.pdf";
            ImportFile.FromHTML(filename);
            //Assert.IsInstanceOfType(ImportFile.FromHTML(filename), typeof(Logic.IDocument));
        }
    }
}

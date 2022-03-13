using System;
using System.Runtime.Serialization;

namespace IO
{
    [Serializable]
    public class BadPDFError : Exception
    {
        public BadPDFError(string message) : base(message)
        { }

        protected BadPDFError(SerializationInfo info, StreamingContext ctxt) : base(info, ctxt)
        { }
    }

    public static class PDF
    {
        public static string GetSvgFromPDF(string filename)
        {
            // TODO: проверить, что файл вообще существует
            var pdf = IronPdf.PdfDocument.FromFile(filename);

            if (pdf.MetaData.CustomProperties.Contains("svgImage"))
            {
                return pdf.MetaData.CustomProperties["svgImage"];
            }
            else
            {
                throw new BadPDFError("unsupported PDF file");
            }
        }
    }
}

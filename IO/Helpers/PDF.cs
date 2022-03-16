namespace IO
{
    public static class PDF
    {
        public static string GetSvgFromPDF(string filename)
        {
            FileValidator.CheckFileExists(filename);
            FileValidator.CheckExtension(filename, FileValidator.PDF_EXTENSION);

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

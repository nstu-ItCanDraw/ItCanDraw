namespace IO
{
    public static class HTML
    {
        public static string GetHTMLStringFromSVGString(string svgString, string title)
        {
            return $@"
                <!DOCTYPE html>
                <html lang=""ru"">
                    <head>
                        <title>{title}</title>
                    </head>
                    <body>
                        {svgString}
                    </body>
                </html>";
        }
    }
}

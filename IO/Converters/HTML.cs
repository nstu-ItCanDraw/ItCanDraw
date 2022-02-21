using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    public static class HTML
    {
        public static string GetHTMLStrongFromSVGString(string svgString)
        {
            return "<html><head><title>Hahahahha</title></head><body><?xml version=\"1.0\" encoding=\"utf-8\"?><!DOCTYPE svg PUBLIC \"-//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\"><svg version=\"1.1\" xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" xmlns:xml=\"http://www.w3.org/XML/1998/namespace\" width=\"500\" height=\"500\" viewBox=\"-250, -250, 500, 500\" style=\"fill:blue;\">  <g>    <rect x=\"100\" y=\"100\" width=\"200\" height=\"50\" transform=\"rotate(45, 0, 0)\" style=\"fill:red;\" />  </g></svg></body></html>";
        }
    }
}

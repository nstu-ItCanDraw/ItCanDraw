using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class DocumentFactory
    {
        public static IDocument CreateDocument(string name, int width, int height)
        {
            return new Document(name, width, height);
        }
    }
}

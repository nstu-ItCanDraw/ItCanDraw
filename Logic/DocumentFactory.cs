using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    /// <summary>
    /// Creates documents
    /// </summary>
    public static class DocumentFactory
    {
        /// <summary>
        /// Creates new document with specified name and width and height in pixels
        /// </summary>
        /// <param name="name">Name of the document</param>
        /// <param name="width">Width of the document in pixels</param>
        /// <param name="height">Height of the document in pixels</param>
        /// <returns>New document object</returns>
        public static IDocument CreateDocument(string name, int width, int height)
        {
            return new Document(name, width, height);
        }
    }
}

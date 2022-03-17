using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    /// <summary>
    /// Contains information about document
    /// </summary>
    public interface IDocument : INotifyPropertyChanged
    {
        /// <summary>
        /// Name of this document
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// List view collection of all geometries in document
        /// </summary>
        public IReadOnlyList<IVisualGeometry> VisualGeometries { get; }
        /// <summary>
        /// Width of the document in pixels
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Height of the document in pixels
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Color of document background
        /// </summary>
        public Color BackgroundColor { get; set; }
        /// <summary>
        /// Adds given visual geometry to geometries in this document
        /// </summary>
        public void AddVisualGeometry(IVisualGeometry visualGeometry);
        /// <summary>
        /// Removes specified visual geometry from this document
        /// </summary>
        public void RemoveVisualGeometry(IVisualGeometry visualGeometry);
        /// <summary>
        /// Moves given visual geometry to specified position in list of visual geometries, changing order of objects
        /// </summary>
        public void ReorderVisualGeometry(IVisualGeometry visualGeometry, int newPosition);
    }
}

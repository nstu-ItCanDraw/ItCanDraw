using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public interface IDocument : INotifyPropertyChanged
    {
        /// <summary>
        /// Name of this document
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// List view collection of all geometries in document
        /// </summary>
        public IReadOnlyCollection<IVisualGeometry> VisualGeometries { get; }
        /// <summary>
        /// Width of the document in pixels
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Height of the document in pixels
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Adds given visual geometry to geometries in this document
        /// </summary>
        public void AddVisualGeometry(IVisualGeometry visualGeometry);
        /// <summary>
        /// Removes specified visual geometry from this document
        /// </summary>
        public void RemoveVisualGeometry(IVisualGeometry visualGeometry);
        /// <summary>
        /// Adds specified visual geometry to selected geometries in this document
        /// </summary>
    }
}

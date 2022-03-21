using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geometry;

namespace Logic
{
    /// <summary>
    /// Contains information about geometry with visual representation
    /// </summary>
    public interface IVisualGeometry : INotifyPropertyChanged
    {
        /// <summary>
        /// Name for geometry
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Geometry base
        /// </summary>
        public IGeometry Geometry { get; }
        /// <summary>
        /// Background brush for geometry
        /// </summary>
        public IBrush BackgroundBrush { get; set; }
        /// <summary>
        /// Border brush for geometry
        /// </summary>
        public IBrush BorderBrush { get; set; }
        /// <summary>
        /// Thickness of border
        /// </summary>
        public double BorderThickness { get; set; }
    }
}

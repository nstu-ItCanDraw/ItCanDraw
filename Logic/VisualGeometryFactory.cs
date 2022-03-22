using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geometry;

namespace Logic
{
    /// <summary>
    /// Creates visual geometries
    /// </summary>
    public static class VisualGeometryFactory
    {
        /// <summary>
        /// Creates new visual geometry wrapper for given geometry object
        /// </summary>
        public static IVisualGeometry CreateVisualGeometry(string name, IGeometry geometry)
        {
            return new VisualGeometry(name, geometry);
        }
    }
}

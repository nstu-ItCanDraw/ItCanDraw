using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geometry;

namespace Logic
{
    public static class VisualGeometryFactory
    {
        public static IVisualGeometry CreateVisualGeometry(IGeometry geometry)
        {
            return new VisualGeometry(geometry);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    public static class FigureFactory
    {
        public static IFigure CreateRectangle(double width, double height, Vector2 position)
        {
            return new Rectangle(width, height, position);
        }
        public static IFigure CreateEllipse(double radiusX, double radiusY, Vector2 position)
        {
            return new Ellipse(radiusX, radiusY, position);
        }
        public static IFigure CreateTriangle(double width, double height, Vector2 position)
        {
            return new Triangle(width, height, position);
        }
        public static IFigure CreatePolyline(IList<Vector2> points)
        {
            return new Polyline(points);
        }
        public static IFigure CreatePolygon(IList<Vector2> points)
        {
            return new Polygon(points);
        }
    }


}

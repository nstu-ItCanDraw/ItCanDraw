using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    public interface IRectangle : IFigure
    {
        double Width { get; set; }
        double Height { get; set; }
    }
    class Rectangle : NotifyPropertyChanged, IRectangle
    {
        public double Width { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Height { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<List<double[]>> Curves => throw new NotImplementedException();

        public Transform Transform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public (double left, double top, double right, double bottom) AABB => throw new NotImplementedException();

        public (double left, double top, double right, double bottom) OBB => throw new NotImplementedException();

        public bool PointInFigure(Vector2 position, double eps)
        {
            throw new NotImplementedException();
        }

        public Rectangle(double width, double height, Vector2 Position)
        {
            Width = width;
            Height = height;
            Transform = new Transform(Position, new Vector2(1,1), 0);
        }
    }
}

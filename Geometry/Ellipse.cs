using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    public interface IEllipse : IFigure
    {
        double RadiusX { get; set; }
        double RadiusY { get; set; }
    }
    class Ellipse : NotifyPropertyChanged, IEllipse
    {
        public double RadiusX { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double RadiusY { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<List<double[]>> Curves => throw new NotImplementedException();

        public Transform Transform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public (double left, double top, double right, double bottom) AABB => throw new NotImplementedException();

        public (double left, double top, double right, double bottom) OBB => throw new NotImplementedException();

        public bool PointInFigure(Vector2 position, double eps)
        {
            throw new NotImplementedException();
        }
    }
}

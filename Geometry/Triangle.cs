using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    // если сделаем полигон, это не надо будет
    public interface ITriangle : IFigure // треугольник всегда равнобедренный, основание внизу
    {
        double Width { get; set; }
        double Height { get; set; }
        List<Vector2> Points { get; } // наверное пригодится io
    }
    class Triangle : NotifyPropertyChanged, ITriangle
    {
        public double Width { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Height { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<Vector2> Points => throw new NotImplementedException();

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    // если сделаем полигон, это не надо будет
    interface ITriangle : IFigure // треугольник всегда равнобедренный, основание внизу
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

        public (Vector2 left_bottom, Vector2 right_top) AABB => throw new NotImplementedException();

        public (Vector2 left_bottom, Vector2 right_top) OBB => throw new NotImplementedException();

        public IList<Vector2> BasicPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        IList<IList<double[]>> IFigure.Curves => throw new NotImplementedException();

        public Dictionary<string, object> GetParameters()
        {
            throw new NotImplementedException();
        }

        public bool PointInFigure(Vector2 position, double eps)
        {
            throw new NotImplementedException();
        }

        public int SetParameters(Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public bool TrySetParameters(Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public bool TrySetParameters(string paramName, object paramValue)
        {
            throw new NotImplementedException();
        }

        public int SetParameters(string paramName, object paramValue)
        {
            throw new NotImplementedException();
        }

        public Triangle(double _width, double _height, Vector2 Position)
        {
            Width = _width;
            Height = _height;
            Transform = new Transform(Position, new Vector2(1, 1), 0);
        }
    }
}

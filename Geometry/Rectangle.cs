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
        double width;
        double height;

        public double Width { get => width; set { width = value; OnPropertyChanged(); } }
        public double Height { get => height; set { height = value; OnPropertyChanged(); } }

        public List<List<double[]>> Curves {
            get
            {
                // прямоугольник; 
                // две кривые: коэф. при x^2 1/(width/2)^2,  свободный коэф. 1
                //                   при у^2 1/(height/2)^2, свободный коэф. 1
                double[] _first = { 4 / (width * width), 0, 0, 0, 0, 1 };
                double[] _second = { 0, 4 / (height * height), 0, 0, 0, 1 };
                List<double[]> _rect = new List<double[]>();
                _rect.Add(_first);
                _rect.Add(_second);
                List<List<double[]>> _curves = new List<List<double[]>>();
                _curves.Add(_rect);

                return _curves;
            }
        }

        public Transform Transform { get; set; }

        public (double left, double top, double right, double bottom) AABB => throw new NotImplementedException();

        public (double left, double top, double right, double bottom) OBB => throw new NotImplementedException();

        public bool PointInFigure(Vector2 position, double eps)
        {
            throw new NotImplementedException();
        }

        public Rectangle(double _width, double _height, Vector2 Position)
        {
            Width = _width;
            Height = _height;
            Transform = new Transform(Position, new Vector2(1,1), 0);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    interface IRectangle : IFigure
    {
        double Width { get; set; }
        double Height { get; set; }
    }
    class Rectangle : NotifyPropertyChanged, IRectangle
    {
        private static Dictionary<string, PropertyInfo> parameterDictionary;
        Dictionary<string, PropertyInfo> IGeometry.ParameterDictionary => parameterDictionary;

        static string name = "rectangle";
        public string Name => name;

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

        // в глобальных
        public BoundingBox AABB => throw new NotImplementedException();
        // в локальных
        public BoundingBox OBB => throw new NotImplementedException();

        IList<IList<double[]>> IFigure.Curves => throw new NotImplementedException();

        public IList<Vector2> BasicPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        static Rectangle()
        {
            Type rectangleType = typeof(Rectangle);
            parameterDictionary = new Dictionary<string, PropertyInfo>();
            parameterDictionary.Add(nameof(Name).ToLower(), rectangleType.GetProperty(nameof(Name)));
            parameterDictionary.Add(nameof(Width).ToLower(), rectangleType.GetProperty(nameof(Width)));
            parameterDictionary.Add(nameof(Height).ToLower(), rectangleType.GetProperty(nameof(Height)));
        }

        public Rectangle(double _width, double _height, Vector2 Position)
        {
            Width = _width;
            Height = _height;
            Transform = new Transform(Position, new Vector2(1, 1), 0);

            Transform.PropertyChanged += Transform_OnPropertyChanged;
        }

        protected void Transform_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Transform));
        }

        public bool IsPointInFigure(Vector2 position, double eps)
        {
            throw new NotImplementedException();
        }
    }
}

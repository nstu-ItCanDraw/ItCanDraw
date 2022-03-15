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

        public double Width
        {
            get => width;
            set
            {
                if (value < 1E-5)
                    throw new ArgumentException("Rectangle width must be greater or equal 1E-5.");

                if (value != width)
                {
                    width = value;
                    OnPropertyChanged("Width");
                }
            }
        }
        public double Height
        {
            get => height;
            set
            {
                if (value < 1E-5)
                    throw new ArgumentException("Rectangle height must be greater or equal 1E-5.");

                if (value != height)
                {
                    height = value;
                    OnPropertyChanged("Height");
                }
            }
        }

        public Transform Transform { get; }

        // в глобальных
        public BoundingBox AABB => throw new NotImplementedException();
        // в локальных
        public BoundingBox OBB => throw new NotImplementedException();

        public IReadOnlyCollection<IReadOnlyCollection<double[]>> Curves => throw new NotImplementedException();

        public IReadOnlyCollection<Vector2> BasicPoints 
        { 
            get => new List<Vector2>() { new Vector2(-width/2, -height /2), new Vector2(width / 2, -height / 2),
             new Vector2(-width/2, height /2),  new Vector2(width/2, height /2)}; 
            set => throw new NotImplementedException(); 
        }

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

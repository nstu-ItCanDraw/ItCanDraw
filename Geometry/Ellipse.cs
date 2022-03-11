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
    interface IEllipse : IFigure
    {
        double RadiusX { get; set; }
        double RadiusY { get; set; }
    }
    class Ellipse : NotifyPropertyChanged, IEllipse
    {
        private static Dictionary<string, PropertyInfo> parameterDictionary;
        Dictionary<string, PropertyInfo> IGeometry.ParameterDictionary => parameterDictionary;

        static string name = "ellipse";
        public string Name => name;

        public double RadiusX { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double RadiusY { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IReadOnlyCollection<IReadOnlyCollection<double[]>> Curves => throw new NotImplementedException();

        public Transform Transform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public BoundingBox AABB => throw new NotImplementedException();

        public BoundingBox OBB => throw new NotImplementedException();

        public IReadOnlyCollection<Vector2> BasicPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        static Ellipse()
        {
            Type ellipseType = typeof(Ellipse);
            parameterDictionary = new Dictionary<string, PropertyInfo>();
            parameterDictionary.Add(nameof(Name).ToLower(), ellipseType.GetProperty(nameof(Name)));
            parameterDictionary.Add("radiusX", ellipseType.GetProperty(nameof(RadiusX)));
            parameterDictionary.Add("radiusY", ellipseType.GetProperty(nameof(RadiusY)));
        }

        public Ellipse(double _radiusX, double _radiusY, Vector2 Position)
        {
            RadiusX = _radiusX;
            RadiusX = _radiusY;
            Transform = new Transform(Position, new Vector2(1, 1), 0);

            Transform.PropertyChanged += Transform_OnPropertyChanged;
        }

        protected void Transform_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(IGeometry.Transform));
        }

        public bool IsPointInFigure(Vector2 position, double eps)
        {
            throw new NotImplementedException();
        }
    }
}

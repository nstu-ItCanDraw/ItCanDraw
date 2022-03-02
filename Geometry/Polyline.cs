using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    interface IPolyline : IFigure
    {
        List<Vector2> Points { get; set; }
    }
    class Polyline : NotifyPropertyChanged, IPolyline
    {
        private static Dictionary<string, PropertyInfo> parameterDictionary;
        Dictionary<string, PropertyInfo> IGeometry.ParameterDictionary => parameterDictionary;

        static string name = "polyline";
        public string Name => name;

        public List<Vector2> Points { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<List<double[]>> Curves => throw new NotImplementedException();

        public Transform Transform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public (Vector2 left_bottom, Vector2 right_top) AABB => throw new NotImplementedException();

        public (Vector2 left_bottom, Vector2 right_top) OBB => throw new NotImplementedException();

        public IList<Vector2> BasicPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        IList<IList<double[]>> IFigure.Curves => throw new NotImplementedException();

        static Polyline()
        {
            Type polylineType = typeof(Polyline);
            parameterDictionary = new Dictionary<string, PropertyInfo>();
            parameterDictionary.Add(nameof(Name).ToLower(), polylineType.GetProperty(nameof(Name)));
            parameterDictionary.Add(nameof(Points).ToLower(), polylineType.GetProperty(nameof(Points)));
        }

        public bool PointInFigure(Vector2 position, double eps)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    interface IPolygone : IFigure
    {
        List<Vector2> Points { get; set; }
    }
    class Polygon : NotifyPropertyChanged, IPolygone
    {
        private static Dictionary<string, PropertyInfo> parameterDictionary;
        Dictionary<string, PropertyInfo> IGeometry.ParameterDictionary => parameterDictionary;

        static string name = "polygon";
        public string Name => name;

        public List<Vector2> Points { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<List<double[]>> Curves => throw new NotImplementedException();

        public Transform Transform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public BoundingBox AABB => throw new NotImplementedException();

        public BoundingBox OBB => throw new NotImplementedException();

        public IList<Vector2> BasicPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        IList<IList<double[]>> IFigure.Curves => throw new NotImplementedException();

        static Polygon()
        {
            Type polygonType = typeof(Polygon);
            parameterDictionary = new Dictionary<string, PropertyInfo>();
            parameterDictionary.Add(nameof(Name).ToLower(), polygonType.GetProperty(nameof(Name)));
            parameterDictionary.Add(nameof(Points).ToLower(), polygonType.GetProperty(nameof(Points)));
        }

        public bool IsPointInFigure(Vector2 position, double eps)
        {
            throw new NotImplementedException();
        }
    }
}

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

        private List<Vector2> points;
        private List<Vector2> globalpoints;

        public List<Vector2> Points 
        {
            get
            {
                return points;
            }
            set
            {
                if (!globalpoints.SequenceEqual(value)) // ? как Vector2 переварится
                    ChangePoints(value);
            }
        }

        public Transform Transform { get; }

        public BoundingBox AABB
        {
            get
            {
                Vector2 left_bottom = new Vector2(double.MaxValue, double.MaxValue);
                Vector2 right_top = new Vector2(double.MinValue, double.MinValue);

                foreach (var point in globalpoints)
                {
                    if (point.x < left_bottom.x)
                        left_bottom.x = point.x;

                    if (point.x > right_top.x)
                        right_top.x = point.x;

                    if (point.y < left_bottom.y)
                        left_bottom.y = point.y;

                    if (point.y > right_top.y)
                        right_top.y = point.y;
                }

                return new BoundingBox() { left_bottom = left_bottom, right_top = right_top };
            }
        }

        public BoundingBox OBB
        {
            get
            {
                Vector2 left_bottom = new Vector2(double.MaxValue, double.MaxValue);
                Vector2 right_top = new Vector2(double.MinValue, double.MinValue);

                foreach (var point in Points)
                {
                    if (point.x < left_bottom.x)
                        left_bottom.x = point.x;

                    if (point.x > right_top.x)
                        right_top.x = point.x;

                    if (point.y < left_bottom.y)
                        left_bottom.y = point.y;

                    if (point.y > right_top.y)
                        right_top.y = point.y;
                }

                return new BoundingBox() { left_bottom = left_bottom, right_top = right_top };
            }
        }

        public IList<Vector2> BasicPoints { get => Points; set => throw new NotImplementedException(); }

        IList<IList<double[]>> IFigure.Curves
        {
            get
            {
                foreach(var point in Points)
                {

                }

                return new List<IList<double[]>>() { new List<double[]>() { bottomEdge, rightEdge, leftEdge } };
            }
        }

        private void ChangePoints(IList<Vector2> _points)
        {
            Vector2 centre = new Vector2();
            int n = 0;
            foreach (var _point in _points)
            {
                centre += _point;
                n++;
            }
            centre /= n;

            Transform.Position = centre;
            Transform.PropertyChanged += Transform_OnPropertyChanged;

            globalpoints = new List<Vector2>(_points);
            points = new List<Vector2>();
            foreach (var _point in globalpoints)
            {
                points.Add((Transform.View * new Vector3(_point, 1)).xy);
            }
        }

        static Polyline()
        {
            Type polylineType = typeof(Polyline);
            parameterDictionary = new Dictionary<string, PropertyInfo>();
            parameterDictionary.Add(nameof(Name).ToLower(), polylineType.GetProperty(nameof(Name)));
            parameterDictionary.Add(nameof(Points).ToLower(), polylineType.GetProperty(nameof(Points)));
        }

        public Polyline(IList<Vector2> _points)
        {
            Transform = new Transform();
            ChangePoints(_points);
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

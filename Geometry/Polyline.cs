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
                if (value.Count < 2)
                    throw new ArgumentException("Polyline must have 2 or more points");

                if (!globalpoints.SequenceEqual(value)) // ? как Vector2 переварится
                {
                    ChangePoints(value);
                    RecalcCurves();
                    RecalcAABB();
                    RecalcOBB();


                    OnPropertyChanged("Curves");
                    OnPropertyChanged("OBB");
                    OnPropertyChanged("AABB");
                }
            }
        }

        public Transform Transform { get; }

        BoundingBox aabb;
        public BoundingBox AABB => aabb;
        BoundingBox obb;
        public BoundingBox OBB => obb;

        public IReadOnlyCollection<Vector2> BasicPoints { get => Points; set => throw new NotImplementedException(); }

        List<List<double[]>> coeficients;
        public IReadOnlyCollection<IReadOnlyCollection<double[]>> Curves => coeficients;

        private void RecalcAABB()
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

            aabb = new BoundingBox() { left_bottom = left_bottom, right_top = right_top };
        }
        private void RecalcOBB()
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

            obb = new BoundingBox() { left_bottom = left_bottom, right_top = right_top };
        }
        private void RecalcCurves(int pos) // пересчитать для определенной точки
        {
            for (int i = pos; i < pos + 1 && i < Points.Count; i++)
            {
                double vx = Points[i].x - Points[i - 1].x;
                double vy = Points[i].y - Points[i - 1].y;
                double w = -Points[i - 1].x * vy + Points[i - 1].y * vx;
                double a = Math.Min(Points[i].x, Points[i - 1].x) + vx / 2.0;

                double[] bound = { 1.0, 0, 0, -2.0 * a, 0, a * a - (vx * vx) / 4.0 };
                double[] line = { vy * vy, vx * vx, -2.0 * vx * vy, 2.0 * w * vy, -2.0 * w * vx, w * w };

                coeficients[i - 1] = new List<double[]>() { bound, line };
            }
        }
        private void RecalcCurves() // пересчитать все
        {
            coeficients = new List<List<double[]>>();
            for (int i = 1; i < Points.Count; i++)
            {
                double vx = Points[i].x - Points[i - 1].x;
                double vy = Points[i].y - Points[i - 1].y;
                double w = -Points[i - 1].x * vy + Points[i - 1].y * vx;
                double a = Math.Min(Points[i].x, Points[i - 1].x) + vx / 2.0;

                double[] bound = { 1.0, 0, 0, -2.0 * a, 0, a * a - (vx * vx) / 4.0 };
                double[] line = { vy * vy, vx * vx, -2.0 * vx * vy, 2.0 * w * vy, -2.0 * w * vx, w * w };

                coeficients.Add(new List<double[]>() { bound, line });
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
                points.Add((Transform.View * new Vector3(_point, 1.0)).xy);
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
            globalpoints = new List<Vector2>();
            Transform = new Transform();
            Points = new List<Vector2>(_points);
        }

        protected void Transform_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RecalcAABB();
            OnPropertyChanged(nameof(IGeometry.Transform));
        }

        public bool IsPointInFigure(Vector2 position, double eps)
        {
            Vector2 localPosition = (Transform.View * new Vector3(position, 1.0)).xy;

            for (int i = 1; i < Points.Count; i++)
            {
                if (Math.Min(Points[i - 1].x, Points[i].x) - eps < localPosition.x &&
                   Math.Max(Points[i - 1].x, Points[i].x) + eps > localPosition.x &&
                   Math.Min(Points[i - 1].y, Points[i].y) - eps < localPosition.y &&
                   Math.Max(Points[i - 1].y, Points[i].y) + eps > localPosition.y)
                    if (GetValue(localPosition, coeficients[i - 1][1]) < eps)
                        return true;
            }
            return false;
        }
        private double GetValue(Vector2 point, double[] coef)
        {
            return coef[0] * point.x * point.x + coef[1] * point.y * point.y + coef[2] * point.x * point.y +
                coef[3] * point.x + coef[4] * point.y + coef[5];
        }
    }
}

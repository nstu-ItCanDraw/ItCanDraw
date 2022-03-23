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
    interface IPolygone : IFigure
    {
        List<Vector2> Points { get; set; }
    }
    // новые точки полигона ВСЕГДА в конце списка
    class Polygon : NotifyPropertyChanged, IPolygone
    {
        private static Dictionary<string, PropertyInfo> parameterDictionary;
        Dictionary<string, PropertyInfo> IGeometry.ParameterDictionary => parameterDictionary;

        static string name = "polygon";
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
                if (value.Count < 3)
                    throw new ArgumentException("Polygone must have 3 or more points");

                if (!globalpoints.SequenceEqual(value)) // ? как Vector2 переварится
                {
                    
                    if (points != null)
                    {
                        List<Vector2> old_points = new List<Vector2>(points);
                        List<Vector2> old_globalpoints = new List<Vector2>(globalpoints);
                        List<List<double[]>> old_coeficients = new List<List<double[]>>(coeficients);
                        ChangePoints(value);
                        int err = RecalcCurves();

                        if (err != 0)
                        {
                            points = new List<Vector2>(old_points);
                            globalpoints = new List<Vector2>(old_globalpoints);
                            coeficients = new List<List<double[]>>(old_coeficients);
                            if (err == 1)
                                throw new ArgumentException("Two points in polygon are too close");
                            if (err == 2)
                                throw new ArgumentException("Non-convex polygon");
                        }
                    }
                    else
                    {
                        ChangePoints(value);
                        int err = RecalcCurves();
                        if (err != 0)
                        {
                            // если две точки совпали и не было предыдущих точек, загрузить базовый полигон
                            globalpoints = new List<Vector2>(new List<Vector2>() { new Vector2(1, 1), new Vector2(5, 1), new Vector2(1, 7) });
                            ChangePoints(value);
                            RecalcCurves();
                            if(err == 1)
                                throw new ArgumentException("Two points in polygon are too close");
                            if (err == 2)
                                throw new ArgumentException("Non-convex polygon");
                        }
                    }
                    RecalcAABB();
                    RecalcOBB();


                    OnPropertyChanged("Curves");
                    OnPropertyChanged("OBB");
                    OnPropertyChanged("AABB");
                }
            }
        }

        List<List<double[]>> coeficients;
        public IReadOnlyCollection<IReadOnlyCollection<double[]>> Curves => coeficients;
        private int RecalcCurves() // пересчитать все
        {
            coeficients = new List<List<double[]>>() { new List<double[]>() };
            double[] line;
            for (int i = 1; i < Points.Count; i++)
            {
                line = GetEdgeCoeffs(Points[i - 1].x, Points[i - 1].y, Points[i].x, Points[i].y);
                if (line[3] * line[3] == 0 && line[4] * line[4] == 0)
                    return 1;
                coeficients[0].Add(line);
                for (int j = i + 1; i < Points.Count; i++)
                {
                    if (GetValue(Points[j], line) > 0)
                    {
                        return 2;
                    }
                }
            }
            line = GetEdgeCoeffs(Points[Points.Count - 1].x, Points[Points.Count - 1].y, Points[0].x, Points[0].y);
            coeficients[0].Add(line);
            return 0;
        }

        /*private bool RecalcCurves() // пересчитать все
        {
            coeficients = new List<List<double[]>>() { new List<double[]>() };
            double[] line;
            int curve_id = 0;
            bool point_not_in_curve = false;
            for (int i = 1; i < Points.Count; i++)
            {
                line = GetEdgeCoeffs(Points[i - 1].x, Points[i - 1].y, Points[i].x, Points[i].y);
                if (line[3] * line[3] == 0 && line[4] * line[4] == 0)
                    return false;
                coeficients[curve_id].Add(line);

                for (int j = i + 1; i < Points.Count; i++)
                {
                    if (GetValue(Points[j], line) > 0)
                    {
                        point_not_in_curve = true;
                    }
                }
                if(point_not_in_curve)
                {
                    point_not_in_curve = false;
                    curve_id++;
                    coeficients.Add(new List<double[]>());
                    line[3] *= -1;
                    line[4] *= -1;
                    line[5] *= -1;
                    coeficients[curve_id].Add(line);
                }
            }
            line = GetEdgeCoeffs(Points[Points.Count - 1].x, Points[Points.Count - 1].y, Points[0].x, Points[0].y);
            coeficients[curve_id].Add(line);
            return true;
        }*/

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

            // -------------- add new points from end to right position --------------
            // -------- no more than one new point is expected near the side ---------
            var to_from = new SortedDictionary<int, List<int>>();
            if (Points != null)
            {
                for (int j = Points.Count; j < _points.Count; j++)
                {
                    Vector2 point_loc = (Transform.View * new Vector3(_points[j], 1.0)).xy;
                    double min_distance = 1e+30, distance;
                    int id_curve = 0;
                    for (int i = 0; i < Curves.ElementAt(0).Count; i++)
                    {
                        int k1 = i,
                            k2 = (i + 1 == points.Count ? 0 : i + 1);
                        if ((points[k1] - point_loc).dot(points[k2] - points[k1]) >= 0)
                            distance = (points[k1] - point_loc).length();
                        else if ((points[k2] - point_loc).dot(points[k2] - points[k1]) <= 0)
                            distance = (points[k2] - point_loc).length();
                        else
                            distance = GetDistanceToCurve(Curves.ElementAt(0).ElementAt(i), point_loc);
                        if (distance < min_distance)
                        {
                            min_distance = distance;
                            id_curve = i;
                        }
                    }
                    if (!to_from.ContainsKey(id_curve))
                        to_from.Add(id_curve, new List<int>());
                    to_from[id_curve].Add(j);
                }
                int shift = 1;

                for (int j = 0; j < Points.Count; j++)
                {
                    globalpoints[j] = new Vector2(_points[j].x, _points[j].y);
                }
                foreach (var to in to_from)
                {
                    if (to.Value.Count == 1)
                    {
                        Vector2 tmp = new Vector2(_points[to.Value[0]].x, _points[to.Value[0]].y);
                        //_points.RemoveAt(to.Value[0]);
                        //_points.Insert(to.Key + shift, tmp);
                        globalpoints.Insert(to.Key + shift, tmp);
                        shift++;
                    }
                    else
                    {
                        throw new ArgumentException("2 new points near one side not released yet");
                        // тут надо отсортировать точки по расстоянию до Point[to.Key] и в таком порядке вставлять
                        // но мне кажется, за раз не будут более 1 точки добавлять из интерфейса, поэтому не сделано
                    }
                }
            }
            else
            {
                globalpoints = new List<Vector2>(_points);
            }
            //--------------------------------------------------------------------------------
            var tmp_save = new List<Vector2>(globalpoints);
            Transform.Position = centre;
            globalpoints = new List<Vector2>(tmp_save);
            //globalpoints = new List<Vector2>(_points);
            points = new List<Vector2>();
            foreach (var _point in globalpoints)
            {
                points.Add((Transform.View * new Vector3(_point, 1.0)).xy);
            }

            Transform.PropertyChanged += Transform_OnPropertyChanged;
        }
        public Transform Transform { get; }

        BoundingBox aabb;
        public BoundingBox AABB => aabb;
        BoundingBox obb;
        public BoundingBox OBB => obb;
        private void RecalcAABB()
        {
            Vector2 left_bottom = new Vector2(double.MaxValue, double.MaxValue);
            Vector2 right_top = new Vector2(double.MinValue, double.MinValue);

            globalpoints = new List<Vector2>();
            foreach (var point in points)
            {
                globalpoints.Add((Transform.Model * new Vector3(point, 1.0)).xy);
            }

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

        // set and get in local
        public IReadOnlyCollection<Vector2> BasicPoints { get => globalpoints; set { Points = value.ToList(); } }

        static Polygon()
        {
            Type polygonType = typeof(Polygon);
            parameterDictionary = new Dictionary<string, PropertyInfo>();
            parameterDictionary.Add(nameof(Name).ToLower(), polygonType.GetProperty(nameof(Name)));
            parameterDictionary.Add(nameof(Points).ToLower(), polygonType.GetProperty(nameof(Points)));
        }

        protected void Transform_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RecalcAABB();
            OnPropertyChanged(nameof(IGeometry.Transform));
        }

        private double[] GetEdgeCoeffs(double x1, double y1, double x2, double y2)
        {
            double vx = x2 - x1;
            double vy = y2 - y1;

            return new double[6] { 0, 0, 0, vy, -vx, -x1 * vy + y1 * vx };
        }
        private double GetDistanceToCurve(double[] _curve, Vector2 _point) // return Distance*Distance
        {
            return (_curve[3]*_point.x + _curve[4]*_point.y + _curve[5]) *
                   (_curve[3] * _point.x + _curve[4] * _point.y + _curve[5]) / 
                   (_curve[3] * _curve[3] + _curve[4] * _curve[4]);
        }

        public bool IsPointInFigure(Vector2 position, double eps)
        {
            Vector2 localPosition = (Transform.View * new Vector3(position, 1.0)).xy;
            bool InFigure = true;
            foreach (var _group_curve in Curves)
            {
                foreach (var _curve in _group_curve)
                {
                    if (GetValue(localPosition, _curve) > eps)
                        InFigure = false;
                }
                if(InFigure == true)
                    return true;
            }

            return false;
        }
        private double GetValue(Vector2 point, double[] coef)
        {
            return coef[3] * point.x + coef[4] * point.y + coef[5];
        }
        public Polygon(IList<Vector2> _points)
        {
            globalpoints = new List<Vector2>();
            Transform = new Transform();
            Points = new List<Vector2>(_points);
        }
    }
}

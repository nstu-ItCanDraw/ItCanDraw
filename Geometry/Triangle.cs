using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private static Dictionary<string, PropertyInfo> parameterDictionary;
        Dictionary<string, PropertyInfo> IGeometry.ParameterDictionary => parameterDictionary;

        static string name = "triangle";
        public string Name => name;

        private double width;
        private double height;

        public double Width 
        {
            get => width; 
            set
            {
                if(value < 1E-5)
                    throw new ArgumentException("Triangle width must be greater or equal 1E-5.");

                if(value != width)
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
                if(value < 1E-5)
                {
                    throw new ArgumentException("Triangle height must be greater or equal 1E-5.");
                }

                if(value != height)
                {
                    height = value;
                    OnPropertyChanged("Height");
                }
            }
        }

        public List<Vector2> Points
        {
            get
            {
                Matrix3x3 globalMatrix = Transform.Model;

                return new List<Vector2>()
                {
                    (globalMatrix * new Vector3(-width / 2.0, -height / 2.0)).xy,
                    (globalMatrix * new Vector3(width / 2.0, -height / 2.0)).xy,
                    (globalMatrix * new Vector3(0.0, height / 2.0)).xy,
                };
            }
        }

        public BoundingBox AABB
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

        public BoundingBox OBB => new BoundingBox()
        {
            left_bottom = new Vector2(-width / 2.0, -height / 2.0),
            right_top = new Vector2(width / 2.0, height / 2.0)
        };

        public IList<IList<double[]>> Curves
        {
            get
            {
                double halfWidth = width / 2.0;
                double halfHeight = height / 2.0;

                double[] bottomEdge = GetEdgeCoeffs(-halfWidth, -halfHeight, halfWidth, -halfHeight);
                double[] rightEdge = GetEdgeCoeffs(halfWidth, -halfHeight, 0.0, halfHeight);
                double[] leftEdge = GetEdgeCoeffs(0.0, halfHeight, -halfWidth, -halfHeight);

                return new List<IList<double[]>>() { new List<double[]>() { bottomEdge, rightEdge, leftEdge } };
            }
        }
        
        public Transform Transform { get; set; }

        public IList<Vector2> BasicPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private double[] GetEdgeCoeffs(double x1, double y1, double x2, double y2)
        {
            double vx = x2 - x1;
            double vy = y2 - y1;

            return new double[6] { 0, 0, 0, vy, -vx, -x1 * vy + y1 * vx };
        }

        public bool IsPointInFigure(Vector2 position, double eps)
        {
            Vector2 localPosition = (Transform.View * new Vector3(position)).xy;

            double halfWidth = width / 2.0;
            double halfHeight = height / 2.0;

            return GetCurveValue(localPosition, -halfWidth, -halfHeight, halfWidth, -halfHeight, eps) <= 0 && 
                   GetCurveValue(localPosition, halfWidth, -halfHeight, 0.0, halfHeight, eps) <= 0 &&
                   GetCurveValue(localPosition, 0.0, halfHeight, -halfWidth, -halfHeight, eps) <= 0;
        }

        private double GetCurveValue(Vector2 point, double x1, double y1, double x2, double y2, double eps)
        {
            double vx = x2 - x1;
            double vy = y2 - y1;

            double value = vy * point.x - vx * point.y - x1 * vy + y1 * vx;

            return Math.Abs(value) <= eps ? 0 : value;
        }

        static Triangle()
        {
            Type triangleType = typeof(Triangle);
            parameterDictionary = new Dictionary<string, PropertyInfo>();
            parameterDictionary.Add(nameof(Name).ToLower(), triangleType.GetProperty(nameof(Name)));
            parameterDictionary.Add(nameof(Width).ToLower(), triangleType.GetProperty(nameof(Width)));
            parameterDictionary.Add(nameof(Height).ToLower(), triangleType.GetProperty(nameof(Height)));
            parameterDictionary.Add(nameof(Points).ToLower(), triangleType.GetProperty(nameof(Points)));
        }

        public Triangle(double _width, double _height, Vector2 Position)
        {
            Width = _width;
            Height = _height;
            Transform = new Transform(Position, new Vector2(1, 1), 0);
        }
    }
}

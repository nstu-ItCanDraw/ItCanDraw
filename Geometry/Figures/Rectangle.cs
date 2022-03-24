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
                    UpdateCurvers();
                    UpdateOBB();
                    UpdateAABB();
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
                    UpdateCurvers();
                    UpdateOBB();
                    UpdateAABB();
                    OnPropertyChanged("Height");
                }
            }
        }

        public Transform Transform { get; }


        // в глобальных
        private BoundingBox aabb;
        public BoundingBox AABB => aabb;
        void UpdateAABB()
        {
            Vector2 left_bottom = new Vector2(double.MaxValue, double.MaxValue);
            Vector2 right_top = new Vector2(double.MinValue, double.MinValue);
            double w2 = width / 2, h2 = height / 2;
            Matrix3x3 globalMatrix = Transform.Model;
            List<Vector2> globalPoints = new List<Vector2>();
            globalPoints.Add((globalMatrix * new Vector3(-w2, -h2, 1)).xy);
            globalPoints.Add((globalMatrix * new Vector3(w2, -h2, 1)).xy);
            globalPoints.Add((globalMatrix * new Vector3(-w2, h2, 1)).xy);
            globalPoints.Add((globalMatrix * new Vector3(w2, h2, 1)).xy);

            foreach (var point in globalPoints)
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
        // в локальных
        private BoundingBox obb;
        public BoundingBox OBB => obb;
        private void UpdateOBB()
        {
            double w2 = width / 2, h2 = height / 2;
            obb = new BoundingBox()
            {
                left_bottom = new Vector2(-w2, -h2),
                right_top = new Vector2(w2, h2)
            };
        }

        List<List<double[]>> curves;
        public IReadOnlyCollection<IReadOnlyCollection<double[]>> Curves => curves;
        void UpdateCurvers()
        {
            double[] Curve1 = { 2 / Width, 0, 0, 0, 0, -Width / 2 };
            double[] Curve2 = { 0, 2 / Height, 0, 0, 0, -Height / 2 };

            curves = new List<List<double[]>>() { new List<double[]>() { Curve1, Curve2 } };
        }


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
            Transform = new Transform(Position, new Vector2(1, 1), 0);
            Width = _width;
            Height = _height;

            Transform.PropertyChanged += Transform_OnPropertyChanged;
        }

        protected void Transform_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateAABB();
            OnPropertyChanged(nameof(Transform));
        }

        public bool IsPointInFigure(Vector2 position, double eps)
        {
            double w2 = width / 2, h2 = height / 2;
            Vector2 localPosition = (Transform.View * new Vector3(position, 1.0)).xy;
            if (localPosition.x + eps >= -w2 && localPosition.x - eps <= w2 &&
                localPosition.y + eps >= -h2 && localPosition.y - eps <= h2)
                return true;
            return false;
        }
    }
}

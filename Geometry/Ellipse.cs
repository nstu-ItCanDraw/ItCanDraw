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

        private double rx;
        private double ry;

        public double RadiusX 
        { 
            get => rx; 
            set
            {
                if(value < 1E-5)
                    throw new ArgumentException("Ellipse width must be greater or equal 1E-5.");

                if(value != rx)
                {
                    radiusx = value;
                    OnPropertyChanged("RadiusX");
                }
            }
        }

        public double RadiusY 
        { 
            get => ry; 
            set
            {
                if(value < 1E-5)
                    throw new ArgumentException("Ellipse width must be greater or equal 1E-5.");

                if(value != ry)
                {
                    radiusx = value;
                    OnPropertyChanged("RadiusY");
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
                    (globalMatrix * new Vector3(-rx, 0)).xy,
                    (globalMatrix * new Vector3(0, ry).xy,
                    (globalMatrix * new Vector3(rx, 0)).xy,
                    (globalMatrix * new Vector3(0, -ry)).xy,
                };
            }
        }
        //ввести поинт
        //перебор точек
        //тоже самое 



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

        public BoundingBox OBB 
        {
            get
            {
                left_bottom = new Vector2(-rx, -ry),
                right_top = new Vector2(rx, ry)
            }
        }

        //тут должны быть коэф для прямой
        //массив массивов

        public IReadOnlyCollection<IReadOnlyCollection<double[]>> Curves => throw new NotImplementedException();

        public IList<IList<double[]>> IGeometry.Curves 
        {
            get => curves;

        }

        void UpdateCurvers ()
        {
            double [] CurveEllips = {1/rx/rx, 1/ry/ry, 0, 0, 0, -1};
            curves = new List<IList<double[]>>() { new List<double[]>() {CurveEllips} };
        }

        // 
        public Transform Transform { get; set; }       
        public IList<Vector2> BasicPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        // центральная или 4 крайние 
        

        

        public bool IsPointInFigure(Vector2 position, double eps)
        {
            Vector2 localPosition = (Transform.View * new Vector3(position)).xy;

            if ((localPosition.x^2/rx^2 + localPosition.y^2/ry^2) -1 <= eps)
            { 
                return (true);
            }
            else
            { 
                return (false);
            }
        }



        
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
    }
}

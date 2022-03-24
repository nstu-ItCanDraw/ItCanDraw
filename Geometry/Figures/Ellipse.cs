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
        private double ry = 1e-6;

        public double RadiusX
        {
            get => rx;
            set
            {
                if (value < 1E-5)
                    throw new ArgumentException("Ellipse width must be greater or equal 1E-5.");

                if (value != rx)
                {
                    rx = value;
                    UpdateCurvers();
                    UpdateOBB();
                    UpdateAABB();
                    OnPropertyChanged("RadiusX");
                }
            }
        }

        public double RadiusY
        {
            get => ry;
            set
            {
                if (value < 1E-5)
                    throw new ArgumentException("Ellipse width must be greater or equal 1E-5.");

                if (value != ry)
                {
                    ry = value;
                    UpdateCurvers();
                    UpdateOBB();
                    UpdateAABB();
                    OnPropertyChanged("RadiusY");
                }
            }
        }

        private BoundingBox aabb;
        public BoundingBox AABB => aabb;
        void UpdateAABB()
        {
            // идея: переводим эллипс в глобальные координаты (умножаем матрицу перехода на коэффициенты)
            //       находим производную по х, по у
            //       эти прямые в местах пересечения с эллипсом дадут экстремумы по х и у
            //       2а1*x + a3*y + a4 = 0 - дадет экстремум по у
            //       2а2*у + а3*х + а5 = 0 - дает экстремум по х
            //       подставляем выражение для у в кравнение эллипса и решаем 2 квадратных уравнения.

            Matrix3x3 globalMatrix_inverse = Transform.Model.inverse();
            Matrix3x3 coefMatrix = new Matrix3x3();
            coefMatrix.v00 = curves[0][0][0];
            coefMatrix.v11 = curves[0][0][1];
            coefMatrix.v22 = curves[0][0][5];

            coefMatrix = globalMatrix_inverse.transposed() * coefMatrix * globalMatrix_inverse;
            double[] globalCoef = { coefMatrix.v00, coefMatrix.v11, 2*coefMatrix.v01, 2*coefMatrix.v20, 2* coefMatrix.v21, coefMatrix.v22 };
            double a, b, c;
            double xmax, ymax, ymin, xmin, xtmp;


            if (Math.Abs(globalCoef[2]) > 1e-15) // есть поворот
            {
                a = (4 * globalCoef[1] * globalCoef[0] * globalCoef[0]) / (globalCoef[2] * globalCoef[2])
                  - globalCoef[0];
                b = (4 * globalCoef[0] * globalCoef[1] * globalCoef[3]) / (globalCoef[2] * globalCoef[2])
                    - (2 * globalCoef[0] * globalCoef[4]) / globalCoef[2];
                c = (globalCoef[1] * globalCoef[3] * globalCoef[3]) / (globalCoef[2] * globalCoef[2])
                    - (globalCoef[4] * globalCoef[3]) / globalCoef[2]
                    + globalCoef[5];
                xtmp = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                ymin = -(globalCoef[3] + 2 * globalCoef[0] * xtmp) / globalCoef[2];
                xtmp = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                ymax = -(globalCoef[3] + 2 * globalCoef[0] * xtmp) / globalCoef[2];
                if (ymax < ymin)
                {
                    xtmp = ymin;
                    ymin = ymax;
                    ymax = xtmp;
                }

                a = globalCoef[0] - (globalCoef[2] * globalCoef[2]) / (4 * globalCoef[1]);
                b = globalCoef[3] - (globalCoef[2] * globalCoef[4]) / (2 * globalCoef[1]);
                c = globalCoef[5] - (globalCoef[4] * globalCoef[4]) / (4 * globalCoef[1]);
                xmin = (-b - Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                xmax = (-b + Math.Sqrt(b * b - 4 * a * c)) / (2 * a);
                if (xmax < xmin)
                {
                    xtmp = xmin;
                    xmin = xmax;
                    xmax = xtmp;
                }
            }

            else
            {
                Vector2 pos = Transform.Position;
                Vector2 delta;
                delta = Math.Abs(Transform.RotationDegrees - 90) < 1e-10 ||
                        Math.Abs(Transform.RotationDegrees - 270) < 1e-10 ?
                        new Vector2(RadiusY, RadiusX) : new Vector2(RadiusX, RadiusY);
                xmax = pos.x + delta.x;
                ymax = pos.y + delta.y;
                ymin = pos.y - delta.y;
                xmin = pos.x - delta.x;
            }

            aabb = new BoundingBox() { left_bottom = new Vector2(xmin, ymin), right_top = new Vector2(xmax, ymax) };
        }
        

        private BoundingBox obb;
        public BoundingBox OBB => obb;
        private void UpdateOBB()
        {
            obb = new BoundingBox()
            {
                left_bottom = new Vector2(-rx, -ry),
                right_top = new Vector2(rx, ry)
            };
        }

        List<List<double[]>> curves;
        public IReadOnlyCollection<IReadOnlyCollection<double[]>> Curves => curves;
        void UpdateCurvers()
        {
            double[] CurveEllips = { 1 / rx / rx, 1 / ry / ry, 0, 0, 0, -1 };
            curves = new List<List<double[]>>() { new List<double[]>() { CurveEllips } };
        }

        public Transform Transform { get; }
        
        IReadOnlyCollection<Vector2> IFigure.BasicPoints { get => new List<Vector2>() { new Vector2(0,0) }; set => throw new NotImplementedException(); }
        // возвращает центр в локальных

        public bool IsPointInFigure(Vector2 position, double eps)
        {
            Vector2 localPosition = (Transform.View * new Vector3(position, 1)).xy;

            if (localPosition.x * localPosition.x / rx / rx + localPosition.y * localPosition.y / ry /ry - 1 <= eps)
            {
                return true;
            }
            else
            {
                return false;
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
            Transform = new Transform(Position, new Vector2(1, 1), 0);
            RadiusX = _radiusX;
            RadiusY = _radiusY;
            
            Transform.PropertyChanged += Transform_OnPropertyChanged;
        }

        protected void Transform_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateAABB();
            OnPropertyChanged(nameof(IGeometry.Transform));
        }
    }
}

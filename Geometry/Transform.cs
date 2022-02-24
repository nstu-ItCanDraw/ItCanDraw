using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    public class Transform : NotifyPropertyChanged
    {
        private Vector2 localPosition;
        private Vector2 localScale;
        private double angle; // в радианах
        //private Matrix3x3 localView;
        //private Matrix3x3 localModel;

        // Scale, Position, Angle по-идее доступны только внутри геометрии, а Model и View - везде
        // но пока я не знаю, как это настроить
        public Vector2 LocalScale { get { return localScale; } set { localScale = value; RecalcMatrix(); OnPropertyChanged(); } }
        public Matrix3x3 LocalView { get; private set; }
        public Matrix3x3 LocalModel { get; private set; }
        private void RecalcMatrix()
        {
            // Move Rotate Scale
            Matrix3x3 mat = new Matrix3x3(Math.Cos(angle), -Math.Sin(angle), 0.0,
                                       Math.Sin(angle), Math.Cos(angle), 0.0,
                                       0.0, 0.0, 1.0);
            mat.v02 = localPosition.x;
            mat.v12 = localPosition.y;

            mat.v00 *= LocalScale.x;
            mat.v10 *= LocalScale.x;
            mat.v01 *= LocalScale.y;
            mat.v11 *= LocalScale.y;

            LocalModel = mat;

            // Scale^(-1) Rotate^(-1) Move^(-1)
            mat = new Matrix3x3(Math.Cos(angle), Math.Sin(angle), 0.0,
                                      -Math.Sin(angle), Math.Cos(angle), 0.0,
                                      0.0, 0.0, 1.0);

            mat.v00 *= 1.0 / LocalScale.x;
            mat.v01 *= 1.0 / LocalScale.x;
            mat.v10 *= 1.0 / LocalScale.y;
            mat.v11 *= 1.0 / LocalScale.y;

            mat.v02 = -mat.v00 * localPosition.x - mat.v01 * localPosition.y;
            mat.v12 = -mat.v10 * localPosition.x - mat.v11 * localPosition.y;

            LocalView = mat;

            OnPropertyChanged("LocalView");
            OnPropertyChanged("LocalModel");
        }
        public Transform Parent { get; private set; }
        public Vector2 GlobalPosition
        {
            get
            {
                return Parent == null ? localPosition : (Parent.Model * new Vector3(localPosition, 1.0)).xy;
            }
            set
            {
                if (Parent == null)
                    localPosition = value;
                else
                    localPosition = (Parent.View * new Vector3(value, 1.0)).xy;

                RecalcMatrix();
                OnPropertyChanged();
            }
        }
        //public Vector2 Scale // не сделан глобальный scale
        //{
        //    get
        //    {
        //        return Parent == null ? localScale : (Parent.Scale + localScale);
        //    }
        //    set
        //    {
        //        //if (Parent == null)
        //        localPosition = value;
        //        //else
        //        //    localPosition = (Parent.View * new Vector3(value, 1.0)).xy;
        //    }
        //}
        public double AngleDeg
        {
            get
            {
                double t = Parent == null ? angle : Parent.AngleDeg + (angle / Math.PI * 180.0); // учитываем угол поворота и родительского пространства
                while (t > 360)  // угол от 0 до 360 градусов
                    t -= 2 * Math.PI;
                while (t < -360) // угол от -360 до 0 градусов
                    t += 2 * Math.PI;
                return t;
            }
            set
            {
                angle = value * Math.PI / 180.0;
                if (Parent != null)
                {
                    angle -= Parent.AngleDeg; // ??? вроде так угол поворота родителя учитывается
                }

                RecalcMatrix();
                OnPropertyChanged();
            }
        }
        public Matrix3x3 Model
        {
            get
            {
                return Parent == null ? LocalModel : Parent.Model * LocalModel;
            }
        }
        public Matrix3x3 View
        {
            get
            {
                return Parent == null ? LocalView : LocalView * Parent.View;
            }
        }
        public Transform()
        {
            localPosition = Vector2.Zero;
            localScale = new Vector2(1, 1);
            angle = 0;
        }
        public Transform(Vector2 _pos, Vector2 _scale, double _angle)
        {
            localPosition = _pos;
            localScale = _scale;
            angle = _angle;
        }
        public void setParent(Transform transform)
        {
            Parent = transform;
        }
    }
}

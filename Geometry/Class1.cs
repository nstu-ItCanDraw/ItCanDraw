using System;
using System.Collections.Generic;
using System.ComponentModel;
using LinearAlgebra;

namespace Geometry
{

    public class Transform
    {
        public Transform Parent { get; private set; }
        public Vector2 localPosition;
        public Vector2 Position
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
            }
        }
        public Matrix3x3 localRotation;
        public Matrix3x3 Rotation
        {
            get
            {
                return Parent == null ? localRotation : Parent.Rotation * localRotation;
            }
            set
            {
                if (Parent == null)
                    localRotation = value;
                else
                    localRotation = Parent.Rotation.inverse() * value;
            }
        }
        public Matrix3x3 LocalModel
        {
            get
            {
                Matrix3x3 mat = localRotation;
                mat.v02 = localPosition.x;
                mat.v12 = localPosition.y;
                return mat;
            }
        }
        public Matrix3x3 Model
        {
            get
            {
                return Parent == null ? LocalModel : Parent.Model * LocalModel;
            }
        }
        public Matrix3x3 LocalView
        {
            get
            {
                Matrix3x3 view = localRotation.transposed();
                view.v02 = -localPosition.x * view.v00 - localPosition.y * view.v01;
                view.v12 = -localPosition.x * view.v10 - localPosition.y * view.v11;
                return view;
            }
        }
        public Matrix3x3 View
        {
            get
            {
                return Parent == null ? LocalView : LocalView * Parent.View;
            }
        }
        //public Vector2 LocalRight { get { return localRotation * Vector3.UnitX; } }
        //public Vector2 LocalUp { get { return localRotation * Vector3.Up; } }
        //public Vector2 Right { get { return Rotation * Vector3.Right; } }
        //public Vector2 Up { get { return Rotation * Vector3.Up; } }
        public Transform()
        {
            localPosition = Vector2.Zero;
            localRotation = Matrix3x3.Identity;
        }
        public void setParent(Transform transform)
        {
            Parent = transform;
        }
    }

    //public class Transform
    //{
    //    public Vector2 Position { get; set; }
    //    public double Rotation { get; set; }
    //    public Vector2 Scale { get; set; }
    //    // Matrix3 View { get; }
    //    // Matrix3 Model { get; }
    //}

    
    public interface IGeometry : INotifyPropertyChanged
    {
        Transform Transform { get; set; } // преобразование локальной системы координат
        (double left, double top, double right, double bottom) AABB { get; } // контур вокруг фигуры, стороны прямоугольника параллельны осям
        (double left, double top, double right, double bottom) OBB { get; } // контур вокруг фигуры в локальной системе координат
        bool PointInFigure(Vector2 position, double eps); // проверяет, что точка с координатами position внутри фигуры с точностью eps
    }

    public interface IFigure : IGeometry
    {
        List<List<double[]>> Curves { get; } // возвращает набор фигур из кривых 2-ого порядка для описания фигуры (для все точек в фигуре выполнено a1*x^2 + a2*y^2 + a3*xy + a4*x + a5*y + a6 <= 0 для всех кривых)
    }
    public interface IOperator : IGeometry
    {
        List<IGeometry> Operands { get; } // возвращает набор фигур операндов
    }

    public interface IPolyline : IFigure
    {
        List<Vector2> Points { get; set; }
    }
    public interface IRectangle : IFigure
    {
        double Width { get; set; }
        double Height { get; set; }
    }
    public interface IEllipse : IFigure
    {
        double RadiusX { get; set; }
        double RadiusY { get; set; }
    }
    public interface ITriangle : IFigure
    {
        double Width { get; set; }
        double Height { get; set; }
    }
}

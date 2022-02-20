using System;
using System.Collections.Generic;

namespace Geometry
{
    public class Vector2
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
    public class Vector3
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double W { get; set; }
    }

    public class Matrix3
    {
        Vector3 First { get; set; }
        Vector3 Second { get; set; }
        Vector3 Third { get; set; }
    }

    public class Transform
    {
        public Vector2 Position { get; set; }
        public double Rotation { get; set; }
        public Vector2 Scale { get; set; }

        // Matrix3 View { get; }
        // Matrix3 Model { get; }

    }
    public interface IFigure
    {
        Transform Transform { get; set; } // преобразование локальной системы координат
        (double left, double top, double right, double bottom) AABB { get; } // контур вокруг фигуры, стороны прямоугольника параллельны осям
        (double left, double top, double right, double bottom) OBB { get; } // контур вокруг фигуры в локальной системе координат
        // List<string> FigureAsString { get; } // (может и не надо) возвращает набор строк, перая строка имя фигуры, дальше параметры в зависимости от типа фигуры
        bool PointInFigure(Vector2 position, double eps); // проверяет, что точка с координатами position внутри фигуры с точностью eps
        List<List<double>> Curves { get; } // возвращает кривые 2-ого порядка для описания фигуры a1*x^2 + a2*y^2 + a3*xy + a4*x + a5*y + a6 = 0
        bool FigureIsClosed { get; set; } // замкнута фигура (true) или нет (false)
    }
}

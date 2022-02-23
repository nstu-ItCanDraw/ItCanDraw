using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;


namespace Geometry
{
    public interface IGeometry : INotifyPropertyChanged
    {
        Transform Transform { get; set; } // преобразование локальной системы координат
        (double left, double top, double right, double bottom) AABB { get; } // контур вокруг фигуры, стороны прямоугольника параллельны осям
        (double left, double top, double right, double bottom) OBB { get; }  // контур вокруг фигуры в локальной системе координат
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
}

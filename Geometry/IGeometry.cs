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
        Transform Transform { get; } // преобразование локальной системы координат
        (Vector2 left_bottom, Vector2 right_top) AABB { get; } // контур вокруг фигуры, стороны прямоугольника параллельны осям
        (Vector2 left_bottom, Vector2 right_top) OBB { get; }  // контур вокруг фигуры в локальной системе координат
        bool PointInFigure(Vector2 position, double eps); // проверяет, что точка с координатами position внутри фигуры с точностью eps
        bool TrySetParameters(Dictionary<string, object> parameters);
        bool TrySetParameters(string paramName, object paramValue);
        int SetParameters(Dictionary<string, object> parameters);
        int SetParameters(string paramName, object paramValue);
        Dictionary<string, object> GetParameters();
    }

    public interface IFigure : IGeometry
    {
        IList<IList<double[]>> Curves { get; } // возвращает набор фигур из кривых 2-ого порядка для описания фигуры (для все точек в фигуре выполнено a1*x^2 + a2*y^2 + a3*xy + a4*x + a5*y + a6 <= 0 для всех кривых)
        IList<Vector2> BasicPoints { get; set; }
    }
    public interface IOperator : IGeometry
    {
        IList<IGeometry> Operands { get; } // возвращает набор фигур операндов
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    public interface IFigure : IGeometry
    {
        IReadOnlyCollection<IReadOnlyCollection<double[]>> Curves { get; } // возвращает набор фигур из кривых 2-ого порядка для описания фигуры (для все точек в фигуре выполнено a1*x^2 + a2*y^2 + a3*xy + a4*x + a5*y + a6 <= 0 для всех кривых)
        IReadOnlyCollection<Vector2> BasicPoints { get; set; }
    }
}

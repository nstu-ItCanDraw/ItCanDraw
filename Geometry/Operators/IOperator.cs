using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry.Operators
{
    public enum OperatorType
    {
        Union,
        Intersection,
        Exclusion
    }

    public interface IOperator : IGeometry
    {
        OperatorType Type { get; } // тип оператора

        IReadOnlyCollection<IGeometry> Operands { get; } // возвращает набор фигур операндов

        void AddOperand(IGeometry operand); // добавляет операнд в конец
        void InsertOperand(int index, IGeometry operand); // вставляет операнд по индексу

        void RemoveOperand(IGeometry operand); // убирает существующий операнд
        void RemoveOperandAt(int index); // убирает операнд по индексу

        void MakeOperandFirst(IGeometry operand); // делает существующий операнд первым
        void MakeOperandLast(IGeometry operand); // делает существующий операнд последним
        void ReplaceOperandTo(int index, IGeometry operand); // перемещает существующий операнд на место {index}

        IReadOnlyCollection<Vector2> BasicPoints { get; }
    }
}

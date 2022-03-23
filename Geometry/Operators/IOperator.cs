using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    public enum OperatorType
    {
        Union,
        Intersection,
        Exclusion
    }

    public interface IOperator : IGeometry
    {
        public OperatorType Type { get; } // тип оператора

        public IReadOnlyList<IGeometry> Operands { get; } // возвращает набор фигур операндов

        public void AddOperand(IGeometry operand); // добавляет операнд в конец

        public void InsertOperand(int index, IGeometry operand); // вставляет операнд по индексу

        public void RemoveOperand(IGeometry operand); // убирает существующий операнд

        public void RemoveOperandAt(int index); // убирает операнд по индексу

        public void ClearOperands();

        public void MakeOperandFirst(IGeometry operand); // делает существующий операнд первым

        public void MakeOperandLast(IGeometry operand); // делает существующий операнд последним

        public void ReplaceOperandTo(int index, IGeometry operand); // перемещает существующий операнд на место {index}

        public IReadOnlyCollection<Vector2> BasicPoints { get; }
    }
}

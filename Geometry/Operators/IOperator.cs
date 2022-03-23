using System;
using System.Collections.Generic;
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
        protected List<IGeometry> OperandList { get; init; }

        OperatorType Type { get; } // тип оператора

        IReadOnlyCollection<IGeometry> Operands => OperandList.AsReadOnly(); // возвращает набор фигур операндов

        void AddOperand(IGeometry operand) // добавляет операнд в конец
        {
            if (operand == null)
                throw new ArgumentNullException(nameof(operand));

            if (OperandList.Contains(operand))
                throw new ArgumentException("The operand is already exists.");

            OperandList.Add(operand);
        }

        void InsertOperand(int index, IGeometry operand) // вставляет операнд по индексу
        {
            if (index < 0 || index > OperandList.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (OperandList.Contains(operand))
                throw new ArgumentException("The operand is already exists.");

            if (operand == null)
                throw new ArgumentNullException(nameof(operand));

            OperandList.Insert(index, operand);
        }

        void RemoveOperand(IGeometry operand) // убирает существующий операнд
        {
            if (OperandList.Contains(operand))
                OperandList.Remove(operand);
        }

        void RemoveOperandAt(int index) // убирает операнд по индексу
        {
            if (index < 0 || index > OperandList.Count - 1)
                throw new ArgumentOutOfRangeException(nameof(index));

            OperandList.RemoveAt(index);
        }

        void MakeOperandFirst(IGeometry operand) // делает существующий операнд первым
        {
            int oldIndex = OperandList.IndexOf(operand);
            if (oldIndex < 0)
                throw new ArgumentException("Operator doesn't contain this operand.");

            OperandList.RemoveAt(oldIndex);
            OperandList.Prepend(operand);
        }

        void MakeOperandLast(IGeometry operand) // делает существующий операнд последним
        {
            int oldIndex = OperandList.IndexOf(operand);
            if(oldIndex < 0)
                throw new ArgumentException("Operator doesn't contain this operand.");

            OperandList.RemoveAt(oldIndex);
            OperandList.Add(operand);
        }

        void ReplaceOperandTo(int index, IGeometry operand) // перемещает существующий операнд на место {index}
        {
            if(index < 0 || index > OperandList.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            int oldIndex = OperandList.IndexOf(operand);
            if(oldIndex < 0)
                throw new ArgumentException("Operator doesn't contain this operand.");

            OperandList.RemoveAt(oldIndex);
            OperandList.Insert(index, operand);
        }

        IReadOnlyCollection<Vector2> BasicPoints { get; }
    }
}

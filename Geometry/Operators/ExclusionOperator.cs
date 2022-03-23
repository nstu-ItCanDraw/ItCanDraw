using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    internal class ExclusionOperator : IOperator
    {
        public OperatorType Type => OperatorType.Exclusion;
        public IReadOnlyCollection<Vector2> BasicPoints => throw new NotImplementedException();
        public string Name => "ExclusionOperator";
        private Transform transform = new Transform();
        public Transform Transform => transform;
        private BoundingBox aabb;
        public BoundingBox AABB { get => aabb; }
        private BoundingBox obb;
        public BoundingBox OBB { get => obb; }
        private List<IGeometry> operands = new List<IGeometry>();
        public IReadOnlyList<IGeometry> Operands { get => operands; }
        private Dictionary<string, PropertyInfo> parameterDictionary;
        Dictionary<string, PropertyInfo> IGeometry.ParameterDictionary { get => parameterDictionary; }

        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsPointInFigure(Vector2 position, double eps)
        {
            if (!operands[0].IsPointInFigure(position, eps))
                return false;
            for (int i = 1; i < operands.Count; i++)
                if (operands[i].IsPointInFigure(position, eps))
                    return false;
            return true;
        }
        public void AddOperand(IGeometry operand) // добавляет операнд в конец
        {
            if (operand == null)
                throw new ArgumentNullException(nameof(operand));

            if (operands.Contains(operand))
                throw new ArgumentException("The operand already exists in this operator..");
            if (operand.Transform.Parent != null)
                throw new ArgumentException("This operand already exists in another operator.");

            operands.Add(operand);
            operand.Transform.Parent = Transform;
            operand.PropertyChanged += operand_OnPropertyChanged;
            OnPropertyChanged("Operands");
        }
        public void InsertOperand(int index, IGeometry operand) // вставляет операнд по индексу
        {
            if (index < 0 || index > operands.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (operand == null)
                throw new ArgumentNullException(nameof(operand));
            if (operands.Contains(operand))
                throw new ArgumentException("The operand is already exists in this operator..");
            if (operand.Transform.Parent != null)
                throw new ArgumentException("This operand already exists in another operator.");

            operands.Insert(index, operand);
            operand.Transform.Parent = Transform;
            operand.PropertyChanged += operand_OnPropertyChanged;
            OnPropertyChanged("Operands");
        }
        public void RemoveOperand(IGeometry operand) // убирает существующий операнд
        {
            if (operand == null)
                throw new ArgumentNullException(nameof(operand));
            if (!operands.Contains(operand))
                throw new ArgumentException("This operand does not exists in this operator.");

            operand.PropertyChanged -= operand_OnPropertyChanged;
            operand.Transform.Parent = null;
            operands.Remove(operand);
            OnPropertyChanged("Operands");
        }
        public void RemoveOperandAt(int index) // убирает операнд по индексу
        {
            if (index < 0 || index > operands.Count - 1)
                throw new ArgumentOutOfRangeException(nameof(index));

            operands[index].PropertyChanged -= operand_OnPropertyChanged;
            operands[index].Transform.Parent = null;
            operands.RemoveAt(index);
            OnPropertyChanged("Operands");
        }
        public void MakeOperandFirst(IGeometry operand) // делает существующий операнд первым
        {
            int oldIndex = operands.IndexOf(operand);
            if (oldIndex < 0)
                throw new ArgumentException("Operator doesn't contain this operand.");

            operands.RemoveAt(oldIndex);
            operands.Prepend(operand);
            OnPropertyChanged("Operands");
        }
        public void MakeOperandLast(IGeometry operand) // делает существующий операнд последним
        {
            int oldIndex = operands.IndexOf(operand);
            if (oldIndex < 0)
                throw new ArgumentException("Operator doesn't contain this operand.");

            operands.RemoveAt(oldIndex);
            operands.Add(operand);
            OnPropertyChanged("Operands");
        }
        public void ReplaceOperandTo(int index, IGeometry operand) // перемещает существующий операнд на место {index}
        {
            if (index < 0 || index > operands.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            int oldIndex = operands.IndexOf(operand);
            if (oldIndex < 0)
                throw new ArgumentException("Operator doesn't contain this operand.");

            operands.RemoveAt(oldIndex);
            operands.Insert(index, operand);
            OnPropertyChanged("Operands");
        }
        private void recalculateAABB()
        {
            double minX = double.MaxValue, minY = double.MaxValue, maxX = double.MinValue, maxY = double.MinValue;
            foreach (IGeometry operand in operands)
            {
                if (operand.AABB.left_bottom.x < minX)
                    minX = operand.AABB.left_bottom.x;
                if (operand.AABB.left_bottom.y < minY)
                    minY = operand.AABB.left_bottom.y;
                if (operand.AABB.right_top.x > maxX)
                    maxX = operand.AABB.right_top.x;
                if (operand.AABB.right_top.y > maxY)
                    maxY = operand.AABB.right_top.y;
            }
            aabb = new BoundingBox();
            aabb.left_bottom = new Vector2(minX, minY);
            aabb.right_top = new Vector2(maxX, maxY);
            OnPropertyChanged("AABB");
        }
        internal ExclusionOperator()
        {
            Type type = GetType();
            parameterDictionary = new Dictionary<string, PropertyInfo>();
            parameterDictionary.Add(nameof(Name).ToLower(), type.GetProperty(nameof(Name)));
            parameterDictionary.Add(nameof(operands).ToLower(), type.GetProperty(nameof(Operands)));
        }
        private void operand_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Operands");
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

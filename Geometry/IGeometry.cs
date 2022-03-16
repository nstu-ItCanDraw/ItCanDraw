using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;


namespace Geometry
{
    public struct BoundingBox
    {
        public Vector2 left_bottom;
        public Vector2 right_top;
    }


    public interface IGeometry : INotifyPropertyChanged
    {
        protected Dictionary<string, PropertyInfo> ParameterDictionary { get; }
        string Name { get; }

        Transform Transform { get; } // преобразование локальной системы координат
        BoundingBox AABB { get; } // контур вокруг фигуры, стороны прямоугольника параллельны осям
        BoundingBox OBB { get; }  // контур вокруг фигуры в локальной системе координат
        bool IsPointInFigure(Vector2 position, double eps); // проверяет, что точка с координатами position внутри фигуры с точностью eps
        bool TrySetParameters(Dictionary<string, object> parameters)
        {
            PropertyInfo propertyInfo;

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                if (!ParameterDictionary.TryGetValue(parameter.Key, out propertyInfo) || !propertyInfo.CanWrite)
                    return false;

                try
                {
                    object currentValue = propertyInfo.GetValue(this);
                    propertyInfo.SetValue(this, parameter.Value);
                    propertyInfo.SetValue(this, currentValue);
                }
                catch (Exception)
                {
                    return false;
                }
            }

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                ParameterDictionary[parameter.Key].SetValue(this, parameter.Value);
            }

            return true;
        }
        bool TrySetParameter(string paramName, object paramValue)
        {
            PropertyInfo propertyInfo;
            if (!ParameterDictionary.TryGetValue(paramName, out propertyInfo) || !propertyInfo.CanWrite)
                return false;

            object currentValue = propertyInfo.GetValue(this);

            try
            {
                propertyInfo.SetValue(this, paramValue);
                return true;
            }
            catch (Exception)
            {
                propertyInfo.SetValue(this, currentValue);
                return false;
            }
        }
        int SetParameters(Dictionary<string, object> parameters)
        {
            PropertyInfo propInfo;

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                if (!ParameterDictionary.TryGetValue(parameter.Key, out propInfo) || !propInfo.CanWrite)
                    throw new ArgumentException($"{Name} doesn't have '{parameter.Key}' parameter or it's readonly.");

                try
                {
                    object currentValue = propInfo.GetValue(this);
                    propInfo.SetValue(this, parameter.Value);
                    propInfo.SetValue(this, currentValue);
                }
                catch (Exception e)
                {
                    throw new ArgumentException($"Can't assign value to {Name}.{parameter.Key}.", e);
                }
            }

            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                ParameterDictionary[parameter.Key].SetValue(this, parameter.Value);
            }

            return 0;
        }
        int SetParameter(string paramName, object paramValue)
        {
            PropertyInfo propertyInfo;
            if (!ParameterDictionary.TryGetValue(paramName, out propertyInfo) || !propertyInfo.CanWrite)
                throw new ArgumentException($"{Name} doesn't have '{paramName}' parameter or it's readonly.");

            object currentValue = propertyInfo.GetValue(this);

            try
            {
                propertyInfo.SetValue(this, paramValue);
            }
            catch (Exception e)
            {
                propertyInfo.SetValue(this, currentValue);
                throw new ArgumentException($"Can't assign value to {Name}.{paramName}.", e);
            }

            return 0;
        }
        Dictionary<string, object> GetParameters()
        {
            return ParameterDictionary.ToDictionary(parameter => parameter.Key, parameter => parameter.Value.GetValue(this));
        }
    }

    public interface IFigure : IGeometry
    {
        IReadOnlyCollection<IReadOnlyCollection<double[]>> Curves { get; } // возвращает набор фигур из кривых 2-ого порядка для описания фигуры (для все точек в фигуре выполнено a1*x^2 + a2*y^2 + a3*xy + a4*x + a5*y + a6 <= 0 для всех кривых)
        IReadOnlyCollection<Vector2> BasicPoints { get; set; }
    }

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

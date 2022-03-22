using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

using LinearAlgebra;

namespace GUI
{
    internal class Vector2Converter : IValueConverter
    {
        private static Vector2 vec;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            vec = (Vector2)value;

            if (parameter.ToString() == "x")
                return ((Vector2)value).x;
            if (parameter.ToString() == "y")
                return ((Vector2)value).y;

            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter.ToString() == "x")
                vec = new Vector2(System.Convert.ToInt32(value), vec.y);

            if (parameter.ToString() == "y")
                vec = new Vector2(vec.x, System.Convert.ToInt32(value));

            return vec;
        }
    }
}

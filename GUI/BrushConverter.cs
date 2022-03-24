using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

using Logic;

namespace GUI
{
    internal class BrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
            if (!(value is Logic.SolidColorBrush))
                throw new NotImplementedException();

            Logic.SolidColorBrush brush = (Logic.SolidColorBrush)value;

            return new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb((byte)(brush.Opacity * 255), brush.Color.r, brush.Color.g, brush.Color.b));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is System.Windows.Media.SolidColorBrush))
                throw new NotImplementedException();

            System.Windows.Media.SolidColorBrush brush = (System.Windows.Media.SolidColorBrush)value;

            return new Logic.SolidColorBrush(brush.Color.R, brush.Color.G, brush.Color.B, brush.Color.A);
        }
    }
}

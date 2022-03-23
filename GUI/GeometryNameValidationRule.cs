using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GUI
{
    internal class GeometryNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (((string)value).Length == 0)
                return new ValidationResult(false, "Name cannot be empty.");

            return ValidationResult.ValidResult;
        }
    }
}

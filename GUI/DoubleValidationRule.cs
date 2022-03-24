using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GUI
{
    internal class DoubleValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!double.TryParse((string)value, out _))
                return new ValidationResult(false, "Illegal character.");

            return ValidationResult.ValidResult;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class SolidColorBrush : IBrush
    {
        private double opacity = 1.0;
        public double Opacity
        {
            get 
            { 
                return opacity; 
            }
            set 
            { 
                if (value > 1 || value < 0)
                    throw new ArgumentOutOfRangeException("value", "Opacity can't be negative or more than 1.");
                opacity = value;
                OnPropertyChanged("Opacity");
            }
        }
        private Color color;
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                OnPropertyChanged("Color");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public SolidColorBrush(byte r, byte g, byte b)
        {
            color = new Color(r, g, b);
        }
        public SolidColorBrush(Color color)
        {
            this.color = color;
        }
        public SolidColorBrush(byte r, byte g, byte b, byte a)
        {
            color = new Color(r, g, b);
            opacity = a / byte.MaxValue;
        }
        public SolidColorBrush(Color color, byte alpha)
        {
            this.color = color;
            opacity = alpha / byte.MaxValue;
        }
    }
}

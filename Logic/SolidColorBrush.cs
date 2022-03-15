using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    /// <summary>
    /// Contains information about solid color brush
    /// </summary>
    public class SolidColorBrush : IBrush
    {
        private double opacity = 1.0;
        /// <summary>
        /// Opacity of the brush
        /// </summary>
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
        /// <summary>
        /// Color of the brush
        /// </summary>
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
        /// <summary>
        /// Creates new solid color brush with color specified by RGB components
        /// </summary>
        /// <param name="r">Red component of the color</param>
        /// <param name="g">Green component of the color</param>
        /// <param name="b">Blue component of the color</param>
        public SolidColorBrush(byte r, byte g, byte b)
        {
            color = new Color(r, g, b);
        }
        /// <summary>
        /// Creates new solid color brush with specified color
        /// </summary>
        public SolidColorBrush(Color color)
        {
            this.color = color;
        }
        /// <summary>
        /// Creates new solid color brush with color specified by RGBA components
        /// </summary>
        /// <param name="r">Red component of the color</param>
        /// <param name="g">Green component of the color</param>
        /// <param name="b">Blue component of the color</param>
        /// <param name="a">Alpha component of the color</param>
        public SolidColorBrush(byte r, byte g, byte b, byte a)
        {
            color = new Color(r, g, b);
            opacity = a / byte.MaxValue;
        }
        /// <summary>
        /// Creates new solid color brush with specified color and alpha component
        /// </summary>
        /// <param name="a">Alpha component of the color</param>
        public SolidColorBrush(Color color, byte alpha)
        {
            this.color = color;
            opacity = alpha / byte.MaxValue;
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

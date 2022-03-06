using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public struct Color
    {
        public byte r;
        public byte g;
        public byte b;

        public static Color White { get => new Color(255, 255, 255); }
        public static Color Black { get => new Color(0, 0, 0); }
        public static Color Gray { get => new Color(127, 127, 127); }
        public static Color Red { get => new Color(255, 0, 0); }
        public static Color Green { get => new Color(0, 255, 0); }
        public static Color Blue { get => new Color(0, 0, 255); }
        public static Color Yellow { get => new Color(255, 255, 0); }
        public static Color Magenta { get => new Color(255, 0, 255); }
        public static Color Cyan { get => new Color(0, 255, 255); }

        public Color(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
}

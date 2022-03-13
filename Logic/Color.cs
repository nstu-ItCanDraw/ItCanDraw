using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    /// <summary>
    /// Contains information about RGB color in 3 byte components
    /// </summary>
    public struct Color
    {
        /// <summary>
        /// Red component of the color
        /// </summary>
        public byte r;
        /// <summary>
        /// Green component of the color
        /// </summary>
        public byte g;
        /// <summary>
        /// Blue component of the color
        /// </summary>
        public byte b;

        /// <summary>
        /// White color, RGB = (255, 255, 255)
        /// </summary>
        public static Color White { get => new Color(255, 255, 255); }
        /// <summary>
        /// Black color, RGB = (0, 0, 0)
        /// </summary>
        public static Color Black { get => new Color(0, 0, 0); }
        /// <summary>
        /// Gray color, RGB = (127, 127, 127)
        /// </summary>
        public static Color Gray { get => new Color(127, 127, 127); }
        /// <summary>
        /// Red color, RGB = (255, 0, 0)
        /// </summary>
        public static Color Red { get => new Color(255, 0, 0); }
        /// <summary>
        /// Green color, RGB = (0, 255, 0)
        /// </summary>
        public static Color Green { get => new Color(0, 255, 0); }
        /// <summary>
        /// Blue color, RGB = (0, 0, 255)
        /// </summary>
        public static Color Blue { get => new Color(0, 0, 255); }
        /// <summary>
        /// Yellow color, RGB = (255, 255, 0)
        /// </summary>
        public static Color Yellow { get => new Color(255, 255, 0); }
        /// <summary>
        /// Magenta color, RGB = (255, 0, 255)
        /// </summary>
        public static Color Magenta { get => new Color(255, 0, 255); }
        /// <summary>
        /// Cyan color, RGB = (0, 255, 255)
        /// </summary>
        public static Color Cyan { get => new Color(0, 255, 255); }

        /// <summary>
        /// Creates the new color with specified Red, Green and Blue components
        /// </summary>
        /// <param name="r">Red component of the color</param>
        /// <param name="g">Green component of the color</param>
        /// <param name="b">Blue component of the color</param>
        public Color(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
}

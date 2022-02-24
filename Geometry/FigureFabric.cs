using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    public abstract class FigureFabric
    {
        public IGeometry CreateRectangle(double width, double height, Vector2 position)
        {
            return new Rectangle(width, height, position);
        }
    }


}

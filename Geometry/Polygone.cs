using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    public interface IPolygone : IFigure
    {
        List<Vector2> Points { get; set; }
    }
    class Polygone : NotifyPropertyChanged, IPolygone
    {
        public List<Vector2> Points { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<List<double[]>> Curves => throw new NotImplementedException();

        public Transform Transform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public (double left, double top, double right, double bottom) AABB => throw new NotImplementedException();

        public (double left, double top, double right, double bottom) OBB => throw new NotImplementedException();

        public bool PointInFigure(Vector2 position, double eps)
        {
            throw new NotImplementedException();
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinearAlgebra;

namespace Geometry
{
    interface IEllipse : IFigure
    {
        double RadiusX { get; set; }
        double RadiusY { get; set; }
    }
    class Ellipse : NotifyPropertyChanged, IEllipse
    {
        public double RadiusX { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double RadiusY { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<List<double[]>> Curves => throw new NotImplementedException();

        public Transform Transform { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public (Vector2 left_bottom, Vector2 right_top) AABB => throw new NotImplementedException();

        public (Vector2 left_bottom, Vector2 right_top) OBB => throw new NotImplementedException();

        public IList<Vector2> BasicPoints { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        IList<IList<double[]>> IFigure.Curves => throw new NotImplementedException();

        public Dictionary<string, object> GetParameters()
        {
            throw new NotImplementedException();
        }

        public bool PointInFigure(Vector2 position, double eps)
        {
            throw new NotImplementedException();
        }

        public int SetParameters(Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public bool TrySetParameters(Dictionary<string, object> parameters)
        {
            throw new NotImplementedException();
        }

        public bool TrySetParameters(string paramName, object paramValue)
        {
            throw new NotImplementedException();
        }

        public int SetParameters(string paramName, object paramValue)
        {
            throw new NotImplementedException();
        }

        public Ellipse(double _radiusX, double _radiusY, Vector2 Position)
        {
            RadiusX = _radiusX;
            RadiusX = _radiusY;
            Transform = new Transform(Position, new Vector2(1, 1), 0);
        }
    }
}

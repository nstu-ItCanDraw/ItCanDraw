using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geometry;

namespace Logic
{
    public interface IVisualGeometry : INotifyPropertyChanged
    {
        public IGeometry Geometry { get; }
        public IBrush BackgroundBrush { get; set; }
        public IBrush BorderBrush { get; set; }
        public double BorderThickness { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public interface IBrush : INotifyPropertyChanged
    {
        public double Opacity { get; set; }
    }
}

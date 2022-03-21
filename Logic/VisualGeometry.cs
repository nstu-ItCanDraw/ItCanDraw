using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Geometry;

namespace Logic
{
    internal class VisualGeometry : IVisualGeometry
    {
        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
        private IGeometry geometry;
        public IGeometry Geometry { get => geometry; }
        private IBrush backgroundBrush = null;
        public IBrush BackgroundBrush
        {
            get
            {
                return backgroundBrush;
            }
            set
            {
                if (backgroundBrush != null)
                    backgroundBrush.PropertyChanged -= backgroundBrush_OnPropertyChanged;
                backgroundBrush = value;
                if (backgroundBrush != null)
                    backgroundBrush.PropertyChanged += backgroundBrush_OnPropertyChanged;
                OnPropertyChanged("BackgroundBrush");
            }
        }
        private IBrush borderBrush = null;
        public IBrush BorderBrush
        {
            get
            {
                return borderBrush;
            }
            set
            {
                if (borderBrush != null)
                    borderBrush.PropertyChanged -= borderBrush_OnPropertyChanged;
                borderBrush = value;
                if (borderBrush != null)
                    borderBrush.PropertyChanged += borderBrush_OnPropertyChanged;
                OnPropertyChanged("BorderBrush");
            }
        }
        private double borderThickness = 1.0;
        public double BorderThickness
        {
            get
            {
                return borderThickness;
            }
            set
            {
                if (value < 0.0)
                    throw new ArgumentOutOfRangeException("BorderThickness", "Border thickness can't be negative.");
                borderThickness = value;
                OnPropertyChanged("BorderThickness");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private void backgroundBrush_OnPropertyChanged(object sender, PropertyChangedEventArgs name)
        {
            OnPropertyChanged("BackgroundBrush");
        }
        private void borderBrush_OnPropertyChanged(object sender, PropertyChangedEventArgs name)
        {
            OnPropertyChanged("BorderBrush");
        }
        private void geometry_OnPropertyChanged(object sender, PropertyChangedEventArgs name)
        {
            OnPropertyChanged("Geometry");
        }
        public VisualGeometry(IGeometry geometry)
        {
            this.geometry = geometry;
            BackgroundBrush = new SolidColorBrush(Color.Black);
            BorderBrush = new SolidColorBrush(Color.Black);
            geometry.PropertyChanged += geometry_OnPropertyChanged;
        }
        ~VisualGeometry()
        {
            geometry.PropertyChanged -= geometry_OnPropertyChanged;
            backgroundBrush.PropertyChanged -= backgroundBrush_OnPropertyChanged;
            borderBrush.PropertyChanged -= borderBrush_OnPropertyChanged;
        }
    }
}

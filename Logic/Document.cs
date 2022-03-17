using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    internal class Document : IDocument
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
                if (value == "")
                    throw new ArgumentException("Document name can't be empty.");
                name = value;
                OnPropertyChanged("Name");
            }
        }
        private List<IVisualGeometry> visualGeometries = new List<IVisualGeometry>();
        public IReadOnlyList<IVisualGeometry> VisualGeometries { get => visualGeometries.AsReadOnly(); }
        private int width;
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Width", "Document size must be positive.");
                width = value;
                OnPropertyChanged("Width");
            }
        }
        private int height;
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("Height", "Document size must be positive.");
                height = value;
                OnPropertyChanged("Height");
            }
        }
        private Color backgroundColor = Color.White;
        public Color BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
                OnPropertyChanged("BackgroundColor");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Document(string name, int width, int height)
        {
            Name = name;
            Width = width;
            Height = height;
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private void visualGeometry_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("VisualGeometries");
        }
        public void AddVisualGeometry(IVisualGeometry visualGeometry)
        {
            if (visualGeometries.Contains(visualGeometry))
                throw new ArgumentException("This object already exists in this document.");
            visualGeometries.Add(visualGeometry);
            visualGeometry.PropertyChanged += visualGeometry_OnPropertyChanged;
            OnPropertyChanged("VisualGeometries");
        }
        public void RemoveVisualGeometry(IVisualGeometry visualGeometry)
        {
            if (!visualGeometries.Contains(visualGeometry))
                throw new ArgumentException("This object does not present in this document.");
            visualGeometries.Remove(visualGeometry);
            visualGeometry.PropertyChanged -= visualGeometry_OnPropertyChanged;
            OnPropertyChanged("VisualGeometries");
        }
        public void ReorderVisualGeometry(IVisualGeometry visualGeometry, int newPosition)
        {
            if (newPosition < 0 || newPosition >= visualGeometries.Count)
                throw new ArgumentOutOfRangeException("newPosition");
            if (!visualGeometries.Contains(visualGeometry))
                throw new ArgumentException("This object does not present in this document.");

            visualGeometries.Remove(visualGeometry);
            visualGeometries.Insert(newPosition, visualGeometry);

            OnPropertyChanged("VisualGeometries");
        }
    }
}

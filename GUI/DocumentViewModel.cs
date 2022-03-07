using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Logic;
using LinearAlgebra;

namespace GUI
{
    internal class DocumentViewModel : INotifyPropertyChanged
    {
        private IDocument currentDocument = null;
        public IDocument CurrentDocument
        {
            get 
            { 
                return currentDocument; 
            }
            set
            {
                if (currentDocument != null)
                {
                    ClearSelectedVisualGeometries();
                    currentDocument.PropertyChanged -= currentDocument_OnPropertyChanged;
                }
                currentDocument = value;
                if (currentDocument != null)
                    currentDocument.PropertyChanged += currentDocument_OnPropertyChanged;
                OnPropertyChanged("CurrentDocument");
            }
        }
        private List<IVisualGeometry> selectedVisualGeometries = new List<IVisualGeometry>();
        public IReadOnlyCollection<IVisualGeometry> SelectedVisualGeometries { get => selectedVisualGeometries.AsReadOnly(); }
        public event PropertyChangedEventHandler PropertyChanged;
        public DocumentViewModel()
        {

        }
        private void checkDocumentNotNull()
        {
            if (currentDocument == null)
                throw new NullReferenceException("There is no current document.");
        }
        public void SelectVisualGeometry(IVisualGeometry visualGeometry)
        {
            checkDocumentNotNull();
            if (!currentDocument.VisualGeometries.Contains(visualGeometry))
                throw new ArgumentException("Given visual geometry does not present in current document.");
            if (!selectedVisualGeometries.Contains(visualGeometry))
                selectedVisualGeometries.Add(visualGeometry);
        }
        public void DeselectVisualGeometry(IVisualGeometry visualGeometry)
        {
            checkDocumentNotNull();
            if (!currentDocument.VisualGeometries.Contains(visualGeometry))
                throw new ArgumentException("Given visual geometry does not present in current document.");
            if (selectedVisualGeometries.Contains(visualGeometry))
                selectedVisualGeometries.Remove(visualGeometry);
        }
        public void ClearSelectedVisualGeometries()
        {
            checkDocumentNotNull();
            selectedVisualGeometries.Clear();
        }
        public void DeleteSelectedVisualGeometries()
        {
            checkDocumentNotNull();
            foreach (IVisualGeometry visualGeometry in selectedVisualGeometries)
                currentDocument.RemoveVisualGeometry(visualGeometry);
            selectedVisualGeometries.Clear();
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private void currentDocument_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("CurrentDocument");
        }
    }
}

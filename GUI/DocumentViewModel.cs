using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

using Logic;
using IO;
using LinearAlgebra;
using System.Windows;

namespace GUI
{
    internal class DocumentViewModel : INotifyPropertyChanged
    {
        private OpenFileDialog openDocumentFileDialog = new OpenFileDialog() { Filter = "Json File|*.json", Multiselect = false };
        private SaveFileDialog saveDocumentFileDialog = new SaveFileDialog() { Filter = "Json File|*.json" };
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
        private VisualGeometryTree currentDocumentVisualTree = new VisualGeometryTree();
        public VisualGeometryTree CurrentDocumentVisualTree => currentDocumentVisualTree;
        private List<IVisualGeometry> selectedVisualGeometries = new List<IVisualGeometry>();
        public IReadOnlyList<IVisualGeometry> SelectedVisualGeometries { get => selectedVisualGeometries.AsReadOnly(); }
        private List<IDocument> openedDocuments = new List<IDocument>();
        public IReadOnlyList<IDocument> OpenedDocuments { get => openedDocuments.AsReadOnly(); }
        public event PropertyChangedEventHandler PropertyChanged;
        private RelayCommand setCurrentDocumentCommand;
        public RelayCommand SetCurrentDocumentCommand
        {
            get => setCurrentDocumentCommand ?? (setCurrentDocumentCommand = new RelayCommand(obj => CurrentDocument = obj as IDocument, obj => obj is IDocument));
        }
        private RelayCommand selectVisualGeometryCommand;
        public RelayCommand SelectVisualGeometryCommand
        {
            get => selectVisualGeometryCommand ?? (selectVisualGeometryCommand = new RelayCommand(obj => SelectVisualGeometry(obj as IVisualGeometry), obj => obj is IVisualGeometry));
        }
        private RelayCommand selectAllVisualGeometriesCommand;
        public RelayCommand SelectAllVisualGeometriesCommand
        {
            get => selectAllVisualGeometriesCommand ?? (selectAllVisualGeometriesCommand = new RelayCommand(obj => SelectAllVisualGeometries()));
        }
        private RelayCommand deselectVisualGeometryCommand;
        public RelayCommand DeselectVisualGeometryCommand
        {
            get => deselectVisualGeometryCommand ?? (deselectVisualGeometryCommand = new RelayCommand(obj => DeselectVisualGeometry(obj as IVisualGeometry), obj => obj is IVisualGeometry));
        }
        private RelayCommand clearSelectedVisualGeometriesCommand;
        public RelayCommand ClearSelectedVisualGeometryCommand
        {
            get => clearSelectedVisualGeometriesCommand ?? (clearSelectedVisualGeometriesCommand = new RelayCommand(obj => ClearSelectedVisualGeometries()));
        }
        private RelayCommand inverseSelectionCommand;
        public RelayCommand InverseSelectionCommand
        {
            get => inverseSelectionCommand ?? (inverseSelectionCommand = new RelayCommand(obj => InverseSelection()));
        }
        private RelayCommand openDocumentCommand;
        public RelayCommand OpenDocumentCommand
        {
            get => openDocumentCommand ?? (openDocumentCommand = new RelayCommand(obj => OpenDocument()));
        }
        private RelayCommand closeCurrentDocumentCommand;
        public RelayCommand CloseCurrentDocumentCommand
        {
            get => closeCurrentDocumentCommand ?? (closeCurrentDocumentCommand = new RelayCommand(obj => CloseCurrentDocument()));
        }
        private RelayCommand saveAsCurrentDocumentCommand;
        public RelayCommand SaveAsCurrentDocumentCommand
        {
            get => saveAsCurrentDocumentCommand ?? (saveAsCurrentDocumentCommand = new RelayCommand(obj => SaveAsCurrentDocument()));
        }
        private RelayCommand saveCurrentDocumentCommand;
        public RelayCommand SaveCurrentDocumentCommand
        {
            get => saveCurrentDocumentCommand ?? (saveCurrentDocumentCommand = new RelayCommand(obj => SaveCurrentDocument()));
        }
        private RelayCommand createDocumentCommand;
        public RelayCommand CreateDocumentCommand
        {
            get => createDocumentCommand ?? (createDocumentCommand = new RelayCommand(obj => CreateDocument()));
        }
        public DocumentViewModel()
        {

        }
        private void checkDocumentNotNull()
        {
            if (currentDocument == null)
                throw new NullReferenceException("There is no current document.");
        }
        public bool IsVisualGeometrySelected(IVisualGeometry visualGeometry)
        {
            checkDocumentNotNull();
            if (!currentDocument.VisualGeometries.Contains(visualGeometry))
                throw new ArgumentException("Given visual geometry does not present in current document.");
            return selectedVisualGeometries.Contains(visualGeometry);
        }
        public void SelectVisualGeometry(IVisualGeometry visualGeometry)
        {
            checkDocumentNotNull();
            if (!currentDocument.VisualGeometries.Contains(visualGeometry))
                throw new ArgumentException("Given visual geometry does not present in current document.");
            if (!selectedVisualGeometries.Contains(visualGeometry))
                selectedVisualGeometries.Add(visualGeometry);
        }
        public void SelectAllVisualGeometries()
        {
            checkDocumentNotNull();
            selectedVisualGeometries.AddRange(currentDocument.VisualGeometries.Except(selectedVisualGeometries));
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
        public void InverseSelection()
        {
            checkDocumentNotNull();

            int selectedCount = selectedVisualGeometries.Count;
            selectedVisualGeometries.AddRange(currentDocument.VisualGeometries.Except(selectedVisualGeometries));
            selectedVisualGeometries.RemoveRange(0, selectedCount);
        }
        public void OpenDocument()
        {
            if (!openDocumentFileDialog.ShowDialog().Value)
                return;

            try
            {
                string filename = openDocumentFileDialog.FileName;
                IDocument document = OpenFile.FromJSON(filename);
                document.FullName = filename;

                openedDocuments.Add(document);
                CurrentDocument = document;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void CloseCurrentDocument()
        {
            if(CurrentDocument.IsModified)
            {
                if (MessageBox.Show("Сохранить текущий документ?", "Save", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    SaveAsCurrentDocument();
                }
            }

            openedDocuments.Remove(CurrentDocument);
            CurrentDocument = openedDocuments.FirstOrDefault();
        }
        public void SaveAsCurrentDocument()
        {
            saveDocumentFileDialog.FileName = CurrentDocument.Name.Trim('*');
            if(!saveDocumentFileDialog.ShowDialog().Value)
            {
                return;
            }

            try
            {
                SaveFile.ToJSON(saveDocumentFileDialog.FileName, CurrentDocument);
                CurrentDocument.FullName = saveDocumentFileDialog.FileName;
                CurrentDocument.IsModified = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void SaveCurrentDocument()
        {
            if(CurrentDocument.FullName == null)
            {
                SaveAsCurrentDocument();
                return;
            }

            try
            {
                SaveFile.ToJSON(CurrentDocument.FullName, CurrentDocument);
                CurrentDocument.IsModified = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void CreateDocument()
        {
            try
            {
                IDocument document = DocumentFactory.CreateDocument("Untitled", 500, 500);
                openedDocuments.Add(document);
                CurrentDocument = document;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        private void currentDocument_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IDocument.VisualGeometries))
                currentDocumentVisualTree.RebuildFromDocument(CurrentDocument);

            OnPropertyChanged("CurrentDocument");
        }
    }
}

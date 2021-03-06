using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Windows;

using Logic;
using Geometry;
using IO;
using LinearAlgebra;
using System.IO;

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
                    currentDocumentVisualTree.Clear();
                    currentDocument.PropertyChanged -= currentDocument_OnPropertyChanged;
                }
                currentDocument = value;
                if (currentDocument != null)
                {
                    currentDocumentVisualTree.RebuildFromDocument(currentDocument);
                    currentDocument.PropertyChanged += currentDocument_OnPropertyChanged;
                }
                OnPropertyChanged("CurrentDocument");
            }
        }
        private VisualGeometryTree currentDocumentVisualTree = new VisualGeometryTree();
        public VisualGeometryTree CurrentDocumentVisualTree => currentDocumentVisualTree;
        private List<IVisualGeometry> selectedVisualGeometries = new List<IVisualGeometry>();
        public IReadOnlyList<IVisualGeometry> SelectedVisualGeometries { get => selectedVisualGeometries.AsReadOnly(); }
        public IVisualGeometry SelectedVisualGeometry => selectedVisualGeometries.Count < 2 ? selectedVisualGeometries.FirstOrDefault() : null;
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
            get => selectVisualGeometryCommand ?? (selectVisualGeometryCommand = new RelayCommand(obj => SelectVisualGeometry(obj as IVisualGeometry), obj => obj is IVisualGeometry && currentDocument != null));
        }
        private RelayCommand deselectVisualGeometryCommand;
        public RelayCommand DeselectVisualGeometryCommand
        {
            get => deselectVisualGeometryCommand ?? (deselectVisualGeometryCommand = new RelayCommand(obj => DeselectVisualGeometry(obj as IVisualGeometry), obj => obj is IVisualGeometry));
        }
        private RelayCommand clearSelectedVisualGeometriesCommand;
        public RelayCommand ClearSelectedVisualGeometriesCommand
        {
            get => clearSelectedVisualGeometriesCommand ?? (clearSelectedVisualGeometriesCommand = new RelayCommand(obj => ClearSelectedVisualGeometries(), obj => currentDocument != null));
        }
        private RelayCommand deleteSelectedVisualGeometriesCommand;
        public RelayCommand DeleteSelectedVisualGeometriesCommand
        {
            get => deleteSelectedVisualGeometriesCommand ?? (deleteSelectedVisualGeometriesCommand = new RelayCommand(obj => DeleteSelectedVisualGeometries(), obj => currentDocument != null));
        }
        private RelayCommand openDocumentCommand;
        public RelayCommand OpenDocumentCommand
        {
            get => openDocumentCommand ?? (openDocumentCommand = new RelayCommand(obj => OpenDocument()));
        }
        private RelayCommand closeCurrentDocumentCommand;
        public RelayCommand CloseCurrentDocumentCommand
        {
            get => closeCurrentDocumentCommand ?? (closeCurrentDocumentCommand = new RelayCommand(obj => CloseCurrentDocument(), obj => currentDocument != null));
        }
        private RelayCommand saveAsCurrentDocumentCommand;
        public RelayCommand SaveAsCurrentDocumentCommand
        {
            get => saveAsCurrentDocumentCommand ?? (saveAsCurrentDocumentCommand = new RelayCommand(obj => SaveAsCurrentDocument(), obj => currentDocument != null));
        }
        private RelayCommand saveCurrentDocumentCommand;
        public RelayCommand SaveCurrentDocumentCommand
        {
            get => saveCurrentDocumentCommand ?? (saveCurrentDocumentCommand = new RelayCommand(obj => SaveCurrentDocument(), obj => currentDocument != null));
        }
        private RelayCommand createDocumentCommand;
        public RelayCommand CreateDocumentCommand
        {
            get => createDocumentCommand ?? (createDocumentCommand = new RelayCommand(obj => CreateDocument()));
        }
        private RelayCommand addRectangleCommand;
        public RelayCommand AddRectangleCommand
        {
            get => addRectangleCommand ?? (addRectangleCommand = new RelayCommand(obj =>
            currentDocument.AddVisualGeometry(VisualGeometryFactory.CreateVisualGeometry(FigureFactory.CreateRectangle(100, 100, (Vector2)obj))), obj => obj is Vector2 && currentDocument != null));
        }
        private RelayCommand addTriangleCommand;
        public RelayCommand AddTriangleCommand
        {
            get => addTriangleCommand ?? (addTriangleCommand = new RelayCommand(obj =>
            currentDocument.AddVisualGeometry(VisualGeometryFactory.CreateVisualGeometry(FigureFactory.CreateTriangle(100, 100, (Vector2)obj))), obj => obj is Vector2 && currentDocument != null));
        }
        private RelayCommand addEllipseCommand;
        public RelayCommand AddEllipseCommand
        {
            get => addEllipseCommand ?? (addEllipseCommand = new RelayCommand(obj =>
            currentDocument.AddVisualGeometry(VisualGeometryFactory.CreateVisualGeometry(FigureFactory.CreateEllipse(50, 50, (Vector2)obj))), obj => obj is Vector2 && currentDocument != null));
        }
        private RelayCommand addUnionOperatorCommand;
        public RelayCommand AddUnionOperatorCommand
        {
            get => addUnionOperatorCommand ?? (addUnionOperatorCommand = new RelayCommand(obj =>
            currentDocument.AddVisualGeometry(VisualGeometryFactory.CreateVisualGeometry(OperatorFactory.CreateUnionOperator())), obj => currentDocument != null));
        }
        private RelayCommand addIntersectionOperatorCommand;
        public RelayCommand AddIntersectionOperatorCommand
        {
            get => addIntersectionOperatorCommand ?? (addIntersectionOperatorCommand = new RelayCommand(obj =>
            currentDocument.AddVisualGeometry(VisualGeometryFactory.CreateVisualGeometry(OperatorFactory.CreateIntersectionOperator())), obj => currentDocument != null));
        }
        private RelayCommand addExclusionOperatorCommand;
        public RelayCommand AddExclusionOperatorCommand
        {
            get => addExclusionOperatorCommand ?? (addExclusionOperatorCommand = new RelayCommand(obj =>
            currentDocument.AddVisualGeometry(VisualGeometryFactory.CreateVisualGeometry(OperatorFactory.CreateExclusionOperator())), obj => currentDocument != null));
        }
        public DocumentViewModel()
        {
            currentDocumentVisualTree.PropertyChanged += currentDocumentVisualTree_OnPropertyChanged;
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
            OnPropertyChanged("SelectedVisualGeometries");
            OnPropertyChanged("SelectedVisualGeometry");
        }
        public void DeselectVisualGeometry(IVisualGeometry visualGeometry)
        {
            checkDocumentNotNull();
            if (!currentDocument.VisualGeometries.Contains(visualGeometry))
                throw new ArgumentException("Given visual geometry does not present in current document.");
            if (selectedVisualGeometries.Contains(visualGeometry))
                selectedVisualGeometries.Remove(visualGeometry);
            OnPropertyChanged("SelectedVisualGeometries");
            OnPropertyChanged("SelectedVisualGeometry");
        }
        public void ClearSelectedVisualGeometries()
        {
            checkDocumentNotNull();
            selectedVisualGeometries.Clear();
            OnPropertyChanged("SelectedVisualGeometries");
            OnPropertyChanged("SelectedVisualGeometry");
        }
        public void DeleteSelectedVisualGeometries()
        {
            checkDocumentNotNull();
            for(int i = 0; i < selectedVisualGeometries.Count; i++)
                currentDocument.RemoveVisualGeometry(selectedVisualGeometries[i]);
            selectedVisualGeometries.Clear();
        }
        public void OpenDocument()
        {
            if (!openDocumentFileDialog.ShowDialog().Value)
                return;

            try
            {
                string filepath = openDocumentFileDialog.FileName;
                IDocument document = OpenFile.FromJSON(filepath);

                foreach (IDocument doc in openedDocuments)
                    if (doc.Name == document.Name && doc.Path == document.Path)
                    {
                        CurrentDocument = doc;
                        return;
                    }

                openedDocuments.Add(document);
                OnPropertyChanged("OpenedDocuments");
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
                if (MessageBox.Show("Save current document?", "Save", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    SaveCurrentDocument();

            openedDocuments.Remove(CurrentDocument);
            OnPropertyChanged("OpenedDocuments");
            CurrentDocument = openedDocuments.FirstOrDefault();
        }
        public void SaveAsCurrentDocument()
        {
            if(!saveDocumentFileDialog.ShowDialog().Value)
            {
                return;
            }

            try
            {
                CurrentDocument.Path = Path.GetDirectoryName(saveDocumentFileDialog.FileName);
                currentDocument.Name = Path.GetFileNameWithoutExtension(saveDocumentFileDialog.FileName);
                CurrentDocument.IsModified = false;
                SaveFile.ToJSON(saveDocumentFileDialog.FileName, CurrentDocument);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void SaveCurrentDocument()
        {
            if(CurrentDocument.Path == null)
            {
                SaveAsCurrentDocument();
                return;
            }

            try
            {
                CurrentDocument.IsModified = false;
                SaveFile.ToJSON(CurrentDocument.Path + "\\" + currentDocument.Name + ".json", CurrentDocument);
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
                OnPropertyChanged("OpenedDocuments");
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
        private void currentDocumentVisualTree_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("CurrentDocumentVisualTree");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

using Geometry;
using Logic;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            renderControl.Focusable = true;
        }

        private void renderControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void MenuItem_Click_Autors(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(File.ReadAllText("Texts/autors.txt"));
        }

        bool _isDragging = false;

        void treeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                _isDragging = true;
                DragDrop.DoDragDrop(currentDocumentTreeView, currentDocumentTreeView.SelectedValue, DragDropEffects.Move);
            }
        }

        void treeView_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(Task)))
            {
                e.Effects = DragDropEffects.Move;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        void treeView_Drop(object sender, DragEventArgs e)
        {
            _isDragging = false;

            if (e.Data.GetDataPresent(typeof(VisualGeometryTreeNode)))
            {
                // То, что мы перетаскиваем
                VisualGeometryTreeNode source = (VisualGeometryTreeNode)e.Data.GetData(typeof(VisualGeometryTreeNode));

                // То, куда мы перестакиваем
                VisualGeometryTreeNode target = GetItemAtLocation<VisualGeometryTreeNode>(e.GetPosition(currentDocumentTreeView));

                VisualGeometryTreeNode tmp = target;

                while(tmp != null)
                {
                    if (tmp == source)
                        return;
                    tmp = tmp.Parent;
                }

                IGeometry sourceGeometry = source.VisualGeometry.Geometry;
                if (sourceGeometry.Transform.Parent != null)
                {
                    VisualGeometryTreeNode parent = source.Parent;
                    if (parent.VisualGeometry == null || !(parent.VisualGeometry.Geometry is IOperator))
                        throw new ArgumentException();

                    (parent.VisualGeometry.Geometry as IOperator).RemoveOperand(sourceGeometry);
                }

                if (target == null || !(target.VisualGeometry.Geometry is IOperator))
                {
                    return;
                }

                (target.VisualGeometry.Geometry as IOperator).AddOperand(sourceGeometry);

                // Code to move the item in the model is placed here...
            }
        }

        T GetItemAtLocation<T>(Point location)
        {
            T foundItem = default(T);
            HitTestResult hitTestResults = VisualTreeHelper.HitTest(currentDocumentTreeView, location);

            if (hitTestResults.VisualHit is FrameworkElement)
            {
                object dataObject = (hitTestResults.VisualHit as
                    FrameworkElement).DataContext;

                if (dataObject is T)
                {
                    foundItem = (T)dataObject;
                }
            }

            return foundItem;
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                renderControl.Focus();
        }
    }
}

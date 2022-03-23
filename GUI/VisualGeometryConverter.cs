using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Logic;

namespace GUI
{
    internal class VisualGeometryConverter : IMultiValueConverter
    {
        private VisualGeometryTree tree;
        private TreeView treeView;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            tree = (VisualGeometryTree)values[0];
            if (tree == null)
                return null;

            IVisualGeometry visualGeometry = (IVisualGeometry)values[1];
            if (visualGeometry == null)
                return null;

            treeView = (TreeView)values[2];
            if (treeView == null)
                return null;

            VisualGeometryTreeNode node = tree.FindVisualGeometryNode(visualGeometry);
            List<TreeViewItem> path = new List<TreeViewItem>();
            TreeViewItem viewItem = GetTreeViewItemByVisualGeometry(treeView, visualGeometry, path);
            path.ForEach(item => item.IsExpanded = true);
            return viewItem;
        }

        private TreeViewItem GetTreeViewItemByVisualGeometry(ItemsControl tree, IVisualGeometry visualGeometry, List<TreeViewItem> path)
        {
            foreach(VisualGeometryTreeNode obj in tree.Items)
            {
                if(obj.VisualGeometry == visualGeometry)
                    return (TreeViewItem)tree.ItemContainerGenerator.ContainerFromItem(obj);

                ItemsControl child = tree.ItemContainerGenerator.ContainerFromItem(obj) as ItemsControl;
                if(child != null)
                {
                    TreeViewItem item = GetTreeViewItemByVisualGeometry(child, visualGeometry, path);
                    if (item != null)
                    {
                        path.Add((TreeViewItem)child);
                        return item;
                    }
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            VisualGeometryTreeNode node = (VisualGeometryTreeNode)value;
            return new object[] { tree, node != null ? node.VisualGeometry : null, treeView };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Logic;

namespace GUI
{
    internal class VisualGeometryTreeNode
    {
        public IVisualGeometry VisualGeometry { get; set; }
        public List<VisualGeometryTreeNode> Children { get; } = new List<VisualGeometryTreeNode>();
    }

    internal class VisualGeometryTree : INotifyPropertyChanged
    {
        private List<VisualGeometryTreeNode> roots = new List<VisualGeometryTreeNode>();
        public IReadOnlyList<VisualGeometryTreeNode> Roots => roots.AsReadOnly();

        public void RebuildFromDocument(IDocument document)
        {
            roots.Clear();

            List<VisualGeometryTreeNode> nodes = document.VisualGeometries.Select(visualGeometry => new VisualGeometryTreeNode() { VisualGeometry = visualGeometry }).ToList();

            roots.AddRange(nodes.Where(node => node.VisualGeometry.Geometry.Transform.Parent == null));
            nodes = nodes.Except(Roots).ToList();

            int i;
            Queue<VisualGeometryTreeNode> queue = new Queue<VisualGeometryTreeNode>(Roots);
            while(nodes.Count != 0)
            {
                VisualGeometryTreeNode node = queue.Dequeue();
                for (i = 0; i < nodes.Count; i++)
                {
                    VisualGeometryTreeNode currentNode = nodes[i];
                    if (currentNode.VisualGeometry.Geometry.Transform.Parent == node.VisualGeometry.Geometry.Transform)
                    {
                        node.Children.Add(currentNode);
                        queue.Enqueue(currentNode);

                        nodes.Remove(currentNode);
                        i--;
                    }
                }
            }

            OnPropertyChanged("Roots");
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}

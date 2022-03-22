using System;
using System.Collections.Generic;
using System.Linq;
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

    internal class VisualGeometryTree
    {
        public List<VisualGeometryTreeNode> Roots { get; private set; } = new List<VisualGeometryTreeNode>();

        public void RebuildFromDocument(IDocument document)
        {
            Roots.Clear();

            List<VisualGeometryTreeNode> nodes = document.VisualGeometries.Select(visualGeometry => new VisualGeometryTreeNode() { VisualGeometry = visualGeometry }).ToList();
            Roots.AddRange(nodes.Where(node => node.VisualGeometry.Geometry.Transform.Parent == null));
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
        }
    }
}

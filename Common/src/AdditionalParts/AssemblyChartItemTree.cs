using System.Collections.Generic;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class AssemblyChartItemTree
    {
        [Transient]
        private Dictionary<Product, List<AssemblyChartItemNode>> nodesByProduct = null;

        public Dictionary<Product, List<AssemblyChartItemNode>> NodesByProduct
        {
            get
            {
                if (nodesByProduct == null)
                {
                    FillNodesByProductRecursive(Root);
                }
                return nodesByProduct;
            }
        }

        private void FillNodesByProductRecursive(AssemblyChartItemNode node)
        {
            if (nodesByProduct == null)
            {
                nodesByProduct = new Dictionary<Product, List<AssemblyChartItemNode>>();
            }
            if (node != null && node.NodeProduct != null)
            {
                if (!nodesByProduct.ContainsKey(node.NodeProduct) || nodesByProduct[node.NodeProduct] == null)
                {
                    nodesByProduct[node.NodeProduct] = new List<AssemblyChartItemNode>();
                }
                nodesByProduct[node.NodeProduct].Add(node);
                node.Nodes.ForEach(FillNodesByProductRecursive);
            }
        }

        public List<AssemblyChartItemNode> GetChildNodesTransitive(AssemblyChartItemNode node)
        {
            if (node == null)
                return new List<AssemblyChartItemNode>();
            List<AssemblyChartItemNode> result = new List<AssemblyChartItemNode>();
            result.AddRange(node.Nodes);
            node.Nodes.ForEach(curNode => result.AddRange(GetChildNodesTransitive(curNode)));
            return result;
        }
    }
}
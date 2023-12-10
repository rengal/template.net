using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class AssemblyChartsTree
    {
        [Transient]
        private object mutex = 1;

        //todo: RMS-40218 Скорее всего будет приезжать из серверного класса
        public AssemblyChartsTree(AssemblyChartNode root, [CanBeNull] ProductSize productSize)
            : this(root)
        {
            ProductSize = productSize;
        }

        //todo: RMS-40218 Убрать
        public AssemblyChartsTree CreateCopyWithSize([CanBeNull] ProductSize productSize)
        {
            return new AssemblyChartsTree(root, productSize);
        }

        public AssemblyChartNode FindNodeByProduct(Product product)
        {
            lock (mutex)
            {
                return FindNodeByProductInternal(product, Root);
            }
        }

        private AssemblyChartNode FindNodeByProductInternal(Product product, AssemblyChartNode curNode)
        {
            if (curNode.Chart.Product.Equals(product))
            {
                return curNode;
            }
            foreach (AssemblyChartNode node in curNode.Nodes)
            {
                AssemblyChartNode findedNode = FindNodeByProductInternal(product, node);
                if (findedNode != null)
                    return findedNode;
            }
            return null;
        }

        //todo: RMS-40218 Должно быть на сервере
        [CanBeNull]
        public ProductSize ProductSize { get; set; }
    }
}
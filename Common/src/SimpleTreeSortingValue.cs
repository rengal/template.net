using System;

namespace Resto.Data
{
    /// <summary>
    /// Простая реализация абстрактного класса <see cref="TreeSortingValue"/>.
    /// </summary>
    public class SimpleTreeSortingValue : TreeSortingValue
    {
        private readonly IComparable nodeId;
        private readonly IComparable nodeValue;
        private readonly TreeSortingValue parentNode;

        public SimpleTreeSortingValue(IComparable id, IComparable value, TreeSortingValue parent)
        {
            nodeId = id;
            nodeValue = value;
            parentNode = parent;
        }

        public override IComparable NodeId
        {
            get { return nodeId; }
        }

        public override IComparable NodeValue
        {
            get { return nodeValue; }
        }

        public override TreeSortingValue ParentNode
        {
            get { return parentNode; }
        }
    }

    /// <summary>
    /// Простая реализация абстрактного класса <see cref="TreeSortingValue{TNodeKey, TNodeValue}"/>.
    /// </summary>
    /// <remarks>
    /// Данный класс удобно использовать совместно с интерфесом <see cref="ISupportTreeSorting{K, S}"/>.
    /// Или можно самостоятельно создавать и поддерживать связь между элементами сортировки и
    /// узлами исходного дерева.
    /// </remarks>
    public class SimpleTreeSortingValue<TNodeKey, TNodeValue> : TreeSortingValue<TNodeKey, TNodeValue>
        where TNodeKey : IComparable, IComparable<TNodeKey>
        where TNodeValue : IComparable, IComparable<TNodeValue>
    {
        private readonly TNodeKey nodeId;
        private readonly TNodeValue nodeValue;
        private readonly TreeSortingValue<TNodeKey, TNodeValue> parentNode;

        public SimpleTreeSortingValue(TNodeKey id, TNodeValue value, TreeSortingValue<TNodeKey, TNodeValue> parent)
        {
            nodeId = id;
            nodeValue = value;
            parentNode = parent;
        }

        public override IComparable NodeId
        {
            get { return nodeId; }
        }

        public override IComparable NodeValue
        {
            get { return nodeValue; }
        }

        public override TreeSortingValue ParentNode
        {
            get { return parentNode; }
        }
    }
}

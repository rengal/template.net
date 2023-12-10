using System;

using Resto.Framework.Attributes.JetBrains;

using Resto.Framework.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    /// <summary>
    /// Бэковское дополнение для работы с классом фасовка.
    /// </summary>
    public partial class Container : IComparable<Container>, IComparable, IDeletable
    {
        public Container(string name)
            : this(GuidGenerator.Next(), name, 1m, 0m, 1m)
        { }
        public override string ToString()
        {
            return name;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
                return true;

            var cont = obj as Container;
            if (cont == null)
                return false;
            return cont.containerWeight == containerWeight
                   && cont.fullContainerWeight == fullContainerWeight
                   && cont.name == name && cont.count == count;
        }

        public override int GetHashCode()
        {
            return (count + containerWeight + fullContainerWeight + name).GetHashCode();
        }

        [NotNull]
        public static Container GetEmptyContainer()
        {
            return new Container(Guid.Empty, "", 0, 0, 0);
        }

        [NotNull]
        public static Container GetMeasureUnitContainer(MeasureUnit unit)
        {
            return new Container(Guid.Empty, unit != null ? unit.NameLocal: "", 1, 0, 1);
        }

        /// <summary>
        /// Возвращает название фасовки <paramref name="container"/> или название
        /// основной единицы измерения для продукта <paramref name="product"/>,
        /// если фасовка пустая.
        /// </summary>
        /// <param name="container">Фасовка</param>
        /// <param name="product">Продукт</param>
        /// <remarks>
        /// Просто обёртка над <see cref="Container.Name"/> и <see cref="Product.MainUnit.NameLocal"/>
        /// с необходимыми проверками на null.
        /// </remarks>
        public static string GetName([CanBeNull] Container container, [CanBeNull] Product product)
        {
            return container != null
                ? container.Name
                : (product != null
                    ? product.MainUnit.NameLocal
                    : string.Empty);
        }

        /// <summary>
        /// Возвращает true, если фасовка не выбрана или выбрана пустая фасовка.
        /// </summary>
        public static bool IsNullOrEmpty(Container cont)
        {
            return cont == null || cont.IsEmptyContainer;
        }

        public bool IsEmptyContainer
        {
            get { return Id.Equals(Guid.Empty); }
        }

        public decimal GetContainerCountByWeigth(decimal weigth)
        {
            if (weigth == 0)
                return 0;
            decimal result = 1;
            if (Count != 0)
                result = weigth / count;
            else if (ContainerWeight != 0 && FullContainerWeight != 0)
                result = weigth / (FullContainerWeight - ContainerWeight);
            else
                return 0;
            return result;
        }

        public decimal GetContainerCountByWeigthForIncomingInvoice(decimal weigth, decimal actualUnitWeight)
        {
            if (weigth == 0)
                return 0;
            if (actualUnitWeight != 0)
                return weigth / actualUnitWeight;
            if (ContainerWeight != 0 && FullContainerWeight != 0)
                return weigth / (FullContainerWeight - ContainerWeight);
            return 0;
        }

        public decimal GetWeigthByContainerCount(decimal containerCount)
        {
            if (Count != 0)
                return Count * containerCount;
            else
                return (FullContainerWeight - ContainerWeight) * containerCount;
        }

        public decimal GetValueByWeigth(decimal weigth)
        {
            try
            {
                decimal k = Count / (FullContainerWeight - ContainerWeight);
                decimal b = (-1 * ContainerWeight * Count) / (FullContainerWeight - ContainerWeight);
                return weigth * k + b;
            }
            catch
            {
                return 0;
            }
        }

        #region IComparable<Container> Members

        public int CompareTo(Container other)
        {
            if (other == null)
            {
                return 1;
            }
            else if (Equals(other))
            {
                return 0;
            }
            return string.CompareOrdinal(Name, other.Name);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }
            else if (!(obj is Container))
            {
                throw new ArgumentException("Object is not a Container");
            }
            return CompareTo((Container)obj);
        }

        #endregion
    }
}

using System;
using Resto.Framework.Data;

namespace Resto.Data
{
    /// <summary>
    /// Запись документа "Приказ об изменении прейскуранта".
    /// </summary>
    public partial class TreeMenuChangeDocumentItem : IComparable, IComparable<TreeMenuChangeDocumentItem>
    {
        [Transient]
        private decimal? _nullableOldPrice;
        public decimal? NullableOldPrice
        {
            get { return _nullableOldPrice; }
            set { _nullableOldPrice = value; }
        }

        /// <summary>
        /// Продукт с размером
        /// </summary>
        public ProductSizeKey ProductSizeKey
        {
            get { return new ProductSizeKey(product, productSize); }
        }

        public int CompareTo(object obj)
        {
            return CompareTo((TreeMenuChangeDocumentItem)obj);
        }

        public int CompareTo(TreeMenuChangeDocumentItem other)
        {
            if (other is null)
            {
                return -1;
            }

            return Num.CompareTo(other.Num);
        }
    }
}
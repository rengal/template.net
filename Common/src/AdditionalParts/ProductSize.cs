using System;

namespace Resto.Data
{
    public partial class ProductSize : IProductScaleTreeItem, IComparable, IComparable<ProductSize>
    {
        public override string ToString()
        {
            return NameLocal;
        }

        #region IProductScaleTreeItem implementation

        Guid? IProductScaleTreeItem.ParentId
        {
            get { return productScale.Id; }
        }

        string IProductScaleTreeItem.Name
        {
            get { return NameLocal; }
        }

        string IProductScaleTreeItem.ShortName
        {
            get { return shortName.Local; }
        }

        bool? IProductScaleTreeItem.IsDefault
        {
            get { return Equals(productScale.DefaultProductSize); }
        }

        ProductScale IProductScaleTreeItem.Scale
        {
            get { return productScale; }
        }

        bool IProductScaleTreeItem.IsParentDeleted
        {
            get { return productScale.Deleted; }
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return CompareTo((ProductSize)obj);
        }

        #endregion IComparable Members

        #region IComparable<ProductSize> Members

        public int CompareTo(ProductSize other)
        {
            if (other == null)
            {
                return -1;
            }

            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            var result = priority - other.priority;
            return result == 0 ? Name.CompareTo(other.Name) : result;
        }

        #endregion
    }
}
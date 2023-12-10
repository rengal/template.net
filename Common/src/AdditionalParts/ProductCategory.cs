using System;

namespace Resto.Data
{
    public partial class ProductCategory : IComparable
    {
        public ProductCategory(Guid id, string name, long localId)
            : base(id, new LocalizableValue(name))
        {
            LocalId = localId;
        }

        public override string ToString()
        {
            return NameLocal;
        }

        public override bool Equals(object obj)
        {
            var ct = obj as ProductCategory;
            if (ct == null) return false;
            return ct.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (obj == null)
                return -1;
            else
                return NameLocal.CompareTo((obj as ProductCategory).NameLocal);
        }

        #endregion
    }
}
using System;
using Resto.Common.Properties;

namespace Resto.Data
{
    public partial class AccountingCategory : IComparable
    {
        public override string ToString()
        {
            return NameLocal;
        }

        public override bool Equals(object obj)
        {
            var ct = obj as AccountingCategory;
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
                return NameLocal.CompareTo((obj as AccountingCategory).NameLocal);
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;

namespace Resto.Data
{
    public partial class Region : IComparable<Region>, IComparable
    {
        public override string ToString()
        {
            return NameLocal;
        }

        #region IComparable<Region> Members

        public int CompareTo(Region other)
        {
            if (other == null)
            {
                return 1;
            }
            if (ReferenceEquals(this, other))
            {
                return 0;
            }
            int result = Comparer<string>.Default.Compare(NameLocal, other.NameLocal);
            if (result == 0)
            {
                result = Comparer<Guid>.Default.Compare(Id, other.Id);
            }
            return result;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return CompareTo(obj as Region);
        }

        #endregion
    }
}
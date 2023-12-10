using System;

namespace Resto.Data
{
    public partial class CookingPlaceType : IComparable, IComparable<CookingPlaceType>
    {
        public override string ToString()
        {
            return NameLocal;
        }

        public static CookingPlaceType GetEmptyPlaceType()
        {
            var result = new CookingPlaceType(Guid.Empty, new LocalizableValue(String.Empty), -1);
            return result;
        }

        #region IComparable<PreparedRegisterItem> Members

        public int CompareTo(CookingPlaceType other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }
            return string.Compare(NameLocal, other.NameLocal);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return CompareTo((CookingPlaceType)obj);
        }

        #endregion
    }
}
using System;

namespace Resto.Data
{
    /// <summary>
    /// Маркетинговый источник доставки
    /// </summary>
    public partial class MarketingSource : IComparable<MarketingSource>, IComparable
    {
        public override string ToString()
        {
            return Name;
        }

        #region IComparable<Conception> Members
        
        public int CompareTo(MarketingSource other)
        {
            return other == null ? 1 : string.Compare(Name, other.Name);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return (obj as MarketingSource) == null ? 1 : CompareTo((MarketingSource)obj);
        }

        #endregion
    }
}
using System;

namespace Resto.Data
{
    /// <summary>
    /// Статья ДДС
    /// </summary>
    public partial class CashFlowCategory : IComparable<CashFlowCategory>, IComparable
    {
        public static readonly Guid IdCashFlowCategoryInvoices = new Guid("3B2D6123-9C32-2613-E5AB-995444A37BBB");
        public static readonly Guid IdCashFlowCategoryGain = new Guid("14C0FE4B-76EC-2681-846E-81D1EC32DB08");

        public Guid? ParentId
        {
            get { return parentCategory != null ? (Guid?) parentCategory.Id : null; }
        }

        public override string ToString()
        {
            return NameLocal;
        }

        #region IComparable<CashFlowCategory> Members

        public int CompareTo(CashFlowCategory other)
        {
            return other == null ? 1 : string.Compare(NameLocal, other.NameLocal);
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return (obj as CashFlowCategory) == null ? 1 : CompareTo((CashFlowCategory)obj);
        }

        #endregion
    }
}

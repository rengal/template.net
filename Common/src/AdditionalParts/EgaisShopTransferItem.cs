using System;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class EgaisShopTransferItem
    {
        #region CTOR
        
        public EgaisShopTransferItem(EgaisAbstractInvoiceItem item, bool? packed)
            : this(Guid.NewGuid(), item.Num, item.SupplierProduct, item.AmountUnit, item.ContainerId,
                item.ProductInfo, item.BRegId)
        {
            Product = item.Product;
            Amount = item.ActualAmount ?? item.Amount;
            Packed = packed;
        }

        public EgaisShopTransferItem(EgaisBalanceDocumentItem item)
            : this(Guid.NewGuid(), item.Num, item.SupplierProduct, item.AmountUnit, item.ContainerId,
                item.ProductInfo, item.BRegId)
        {
            Amount = item.Amount;
        }

        #endregion

        #region Overridden

        public override bool IsCorrect
        {
            get { return base.IsCorrect && !BRegId.IsNullOrWhiteSpace(); }
        }

        #endregion

    }
}

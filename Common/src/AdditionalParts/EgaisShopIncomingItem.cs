using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resto.Data
{
    public partial class EgaisShopIncomingItem
    {
        public EgaisShopIncomingItem(EgaisAbstractDocumentItem item)
            : this(Guid.NewGuid(), item.Num, item.SupplierProduct, item.AmountUnit, item.ContainerId, item.ProductInfo)
        {
            Product = item.Product;
            Amount = item.Amount;
        }
    }
}

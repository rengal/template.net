using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Resto.Common.Extensions;
using Resto.Framework.Common;

namespace Resto.Data
{
    /// <remarks>
    /// Бэковское дополнение для работы с классом SupplierPriceList.
    /// </remarks>
    public partial class SupplierPriceList
    {
        /// <summary>
        /// Возвращает список айтемов для <paramref name="supplierProduct"/>
        /// </summary>
        public IEnumerable<SupplierPriceListItem> GetItemsBySupplierProduct(Product supplierProduct)
        {
            return this.AllNotDeletedItems().Where(i => i.SupplierProduct.Equals(supplierProduct));
        }
    }
}

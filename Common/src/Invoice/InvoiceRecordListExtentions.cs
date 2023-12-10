using System;
using System.Collections.Generic;
using System.Linq;
using Resto.Common.Extensions;
using Resto.Framework.Common.XmlSerialization;

namespace Resto.Data
{
    /// <summary>
    /// Класс расширение для работы со списком записей <see cref="InvoiceRecord"/>.
    /// </summary>
    public static class InvoiceRecordListExtentions
    {
        /// <summary>
        /// Возвращает прайс <see cref="SupplierPriceList"/> на основе списка <see cref="InvoiceRecord"/>.
        /// </summary>
        public static SupplierPriceList GetSupplierPriceList(this IEnumerable<InvoiceRecord> records, SupplierPriceList supplierPrice)
        {
            var newPrice = Serializer.DeepClone(supplierPrice);
            var newItems = new List<SupplierPriceListItem>();
            foreach (var record in records.Where(r => r.SupplierProduct != null))
            {
                // У строк прайс-листа поставщика ключ "наш продукт, продукт поставщика, фасовка поставщика".
                var item = newPrice.AllNotDeletedItems().FirstOrDefault(i =>
                    i.NativeProduct.Equals(record.Product) &&
                    i.SupplierProduct.Equals(record.SupplierProduct) &&
                    i.ContainerId.Equals(record.Container.Id));

                if (item != null)
                {
                    item.CostPrice = record.GetPriceCurrentPrice();
                }
                else
                {
                    var newItem = new SupplierPriceListItem(Guid.NewGuid(), record.Product,
                        record.Container.Id, record.SupplierProduct)
                    {
                        CostPrice = record.GetPriceCurrentPrice()
                    };

                    newItems.Add(newItem);
                }
            }

            foreach (var newItem in newItems)
            {
                newPrice.AddPriceItem(newItem);
            }

            return newPrice;
        }

        /// <summary>
        /// Возвращает список <see cref="SupplierInfo"/> на основе списка <see cref="InvoiceRecord"/>.
        /// </summary>
        public static IEnumerable<SupplierInfo> GetSupplierInfos(this IEnumerable<InvoiceRecord> records)
        {
            var result = new List<SupplierInfo>();
            foreach (var record in records.Where(r => !r.IsEmptyRecord && r.SupplierProduct == null))
            {
                if (record.IsAdditionalExpense)
                {
                    continue;
                }

                var supInfo = record.GetSupplierInfo();
                if (result.Any(sInfo => sInfo.Equals(supInfo)))
                {
                    continue;
                }

                if (supInfo == null)
                {
                    supInfo = new SupplierInfo(Guid.NewGuid(), record.Supplier, record.Product, record.Container.Id);
                }

                supInfo.CostPrice = record.GetPriceCurrentPrice();
                result.Add(supInfo);
            }

            return result;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common;
using Resto.Framework.Data;

namespace Resto.Data
{
    public static class ProductSizeExtensions
    {
        /// <summary>
        /// Проверяет, что данный <see cref="ProductSizeKey"/> и
        /// продукт в нём не <c>null</c>
        /// </summary>
        public static bool IsEmpty([CanBeNull] this ProductSizeKey productSizeKey)
        {
            return productSizeKey == null || productSizeKey.Product == null;
        }

        /// <summary>
        /// Для указанного продукта возвращает множество пар "Продукт - Размер"
        /// </summary>
        /// <param name="product">Продукт</param>
        /// <remarks>
        /// Если продукту не назначена шкала размеров, то возвращается единственное
        /// значение "Продукт - null", иначе возвращается по одной паре
        /// "Продукт - Размер" для каждого размера шкалы, исключая удалённые и те,
        /// которые отключены (disabled) для данного продукта.
        /// </remarks>
        public static ICollection<ProductSizeKey> WithSizes([NotNull] this Product product)
        {
            var productScale = product.ScaleOfProductOrModifierSchema;
            if (productScale == null)
            {
                return new List<ProductSizeKey> { new ProductSizeKey(product, null) };
            }
            return EntityManager.INSTANCE
                .GetAllNotDeleted<ProductSize>(size => Equals(size.ProductScale, productScale))
                .Where(size => product.DisabledProductSizes == null || !product.DisabledProductSizes.Contains(size.Id))
                .OrderBy(size => size.Priority)
                .Select(size => new ProductSizeKey(product, size))
                .ToList();
        }

        /// <summary>
        /// Копирует <see cref="ProductSizeKey"/>
        /// </summary>
        public static ProductSizeKey Copy([CanBeNull] this ProductSizeKey productSizeKey)
        {
            return productSizeKey == null ? null : new ProductSizeKey(productSizeKey.Product, productSizeKey.ProductSize);
        }

        /// <summary>
        /// При смене продукта в строке документа устанавливает размер по умолчанию
        /// для этого продукта. Если у нового выбранного продукта та же шкала размеров,
        /// что у предыдущего, то размер в строке документа не меняется.
        /// </summary>
        public static void SetDefaultProductSize(this IDocumentRecordWithProductSize record)
        {
            if (record.Product == null || !record.Product.CanHaveSize)
            {
                record.ProductSize = null;
                return;
            }

            var scale = record.Product.ScaleOfProductOrModifierSchema;
            if (scale == null)
            {
                record.ProductSize = null;
                return;
            }

            if (record.ProductSize == null || !Equals(record.ProductSize.ProductScale, scale))
            {
                record.ProductSize = record.Product.DefaultSize;
            }
        }

        /// <summary>
        /// Устанавливает в строке документа коэффициент списания в зависимости от размера и количества.
        /// </summary>
        public static void UpdateAmountFactor(this IDocumentRecordWithProductSize record)
        {
            record.AmountFactor = record.Product == null
                ? ProductSizeServerConstants.INSTANCE.DefaultAmountFactor
                : record.Product.GetAmountFactor(record.ProductSize, record.Amount);
        }

        /// <summary>
        /// Возвращает название размера или пустую строку, если размер <c>null</c>
        /// </summary>
        public static string SizeName([NotNull] this ProductSizeKey productSizeKey)
        {
            return productSizeKey.ProductSize == null ? string.Empty : productSizeKey.ProductSize.NameLocal;
        }
    }
}

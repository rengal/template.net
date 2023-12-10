using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Data
{
    public partial class MenuTemplateItem
    {
        #region CTOR

        public MenuTemplateItem(MenuTemplateItem item)
        {
            num = item.Num;
            product = item.Product;
            productSize = item.ProductSize;
            priceForBaseCategory = item.PriceForBaseCategory;

            if (item.PricesForCategories == null)
            {
                return;
            }

            pricesForCategories = new Dictionary<ClientPriceCategory, decimal>(item.PricesForCategories);
        }

        #endregion
        
        #region Properties

        /// <summary>
        /// Код внутреннего продукта iiko
        /// </summary>
        public ProductNumCompletionData ProductNum
        {
            get { return Product != null ? new ProductNumCompletionData { Product = Product } : null; }
            set { Product = value != null ? value.Product : null; }
        }

        public ProductGroup Group
        {
            get { return Product != null ? Product.Parent : null; }
        }

        #endregion

        #region Methods

        public bool EqualByProductAndSize(MenuTemplateItem item)
        {
            return Equals(Product, item.Product) && Equals(ProductSize, item.ProductSize);
        }

        public bool EqualByProduct(MenuTemplateItem item)
        {
            return Equals(Product, item.product);
        }

        public void SetPriceForCategory([NotNull] ClientPriceCategory priceCategory, decimal? price)
        {
            if (price == null)
            {
                PricesForCategories.Remove(priceCategory);
            }
            else
            {
                PricesForCategories[priceCategory] = price.Value;
            }
        }

        #endregion
    }
}

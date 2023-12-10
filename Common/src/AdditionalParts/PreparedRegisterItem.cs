using Resto.Common.Properties;

namespace Resto.Data
{
    public sealed partial class PreparedRegisterItem
    {
        private ProductNumCompletionData productNum;

        public string CookingPlaceType
        {
            get
            {
                if (Product != null && Product.PlaceType != null)
                {
                    return Product.PlaceType.NameLocal;
                }
                else
                {
                    return Resources.PreparedRegisterItemCookingPlaceTypeNotSpecified;
                }
            }
        }


        /// <summary>
        /// Артикул товара.
        /// </summary>
        public ProductNumCompletionData ProductNum
        {
            get { return productNum = ProductNumCompletionData.SameOrNew(productNum, product); }
            set { Product = value != null ? value.Product : null; }
        }

        public string SessionObj
        {
            get { return Session; }
            set
            {
                if (value.Trim().Length > 0)
                {
                    Session = value;
                }
                else
                {
                    Session = null;
                }
            }
        }
    }
}
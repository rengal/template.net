using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class AbstractExternalDocumentItem
    {
        private ProductNumCompletionData productNum;

        public ProductNumCompletionData ProductNum
        {
            get { return productNum = ProductNumCompletionData.SameOrNew(productNum, product); }
            set { Product = value != null ? value.Product : null; }
        }

        /// <summary>
        /// Фасовка
        /// </summary>
        [CanBeNull]
        public Container Container
        {
            get
            {
                if (Product == null || containerId == null)
                {
                    return null;
                }
                return Product.GetContainerById(ContainerId.GetValueOrFakeDefault());
            }
            set
            {
                if (value == null)
                {
                    containerId = null;
                    return;
                }

                containerId = value.Id;
            }
        }

        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Sum
        {
            get { return ActualAmount.GetValueOrFakeDefault() * Price.GetValueOrFakeDefault(); }
        }

        /// <summary>
        /// Если есть подтвержденное количество - возвращает его, иначе - просто количество 
        /// </summary>
        public decimal ApropriateAmountByStatus
        {
            get
            {
                return ActualAmount == null
                    ? Amount.GetValueOrDefault()
                    : ActualAmount.Value;
            }
        }
    }
}
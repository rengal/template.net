using System;
using Resto.Framework.Attributes.JetBrains;
using Resto.Common.Properties;
using Resto.Framework.Data;

namespace Resto.Data
{
    public partial class FoodcostMinPriceInfo
    {
        [Transient]
        [CanBeNull]
        private Container container;

        [NotNull]
        public Container Container
        {
            get
            {
                return container ?? (container = product.GetContainerById(containerId ?? Guid.Empty));
            }
        }

        public decimal PricePerUnit
        {
            get
            {
                return Container.IsNullOrEmpty(Container) ? price : price / Container.Count;
            }
        }

        [UsedImplicitly]
        public string Source
        {
            get
            {
                if (isFromInvoice)
                {
                    return Resources.FoodcostMinPriceSourceInvoice;
                }

                return supplier == null ? Resources.FoodcostMinPriceSourcePriceList : Resources.FoodcostMinPriceSourceSupplierPriceList;
            }
        }
    }
}
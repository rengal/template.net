using System;

// ReSharper disable CheckNamespace
namespace Resto.Data
// ReSharper restore CheckNamespace
{
    /// <summary>
    /// Запись бланка заказа в производство.
    /// </summary>
    public partial class ProductionOrderBlankItem
    {
        private Container container;

        public ProductionOrderBlankItem(Guid? id, Product product, int position, string comment, Container container)
            : this(id, product, position, comment)
        {
            Container = container;
        }

        /// <summary>
        /// Фасовка.
        /// </summary>
        public Container Container
        {
            get
            {
                if (containerId != null && containerId != Guid.Empty &&
                    (Container.IsNullOrEmpty(container) || container.Id != containerId))
                {
                    container = Product.GetContainerById(containerId.Value);
                }

                return container;
            }
            set
            {
                container = value;
                containerId = value == null ? (Guid?) null : value.Id;
            }
        }
    }
}

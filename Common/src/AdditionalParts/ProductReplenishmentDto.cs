using System;
using System.Diagnostics.Contracts;

namespace Resto.Data
{
    public partial class ProductReplenishmentDto
    {
        /// <summary>
        /// Фасовка
        /// </summary>
        public Container Container { get; set; }

        /// <summary>
        /// Количество в фасовке
        /// </summary>
        public decimal ContainerAmount
        {
            get
            {
                Contract.Assert(Amount.HasValue);
                return Container.Count == 0m ? Amount.Value : Amount.Value / Container.Count;
            }
        }

        /// <summary>
        /// Цена за одну фасовку
        /// </summary>
        public decimal ContainerCost
        {
            get
            {
                decimal cost;
                if (!ContainerCosts.TryGetValue(Container == null ? Guid.Empty : Container.Id, out cost))
                {
                    cost = 0m;
                }
                return cost;
            }
        }

        /// <summary>
        /// Округление вверх до целого числа фасовок
        /// </summary>
        public void RoundUpToContainer()
        {
            Contract.Assert(Amount.HasValue);
            Amount = Container.IsNullOrEmpty(Container)
                ? Math.Ceiling(Amount.Value)
                : Math.Ceiling(ContainerAmount) * Container.Count;
        }
    }
}

using System;

namespace Resto.Data
{
    public abstract partial class AbstractInvoiceItem : IItemWithCost
    {
        public virtual bool IsEmptyItem => Product == null;

        public decimal SumWithoutDiscount
        {
            get => Sum.GetValueOrDefault() - DiscountSum.GetValueOrDefault();
            set => Sum = value + DiscountSum.GetValueOrDefault();
        }

        public decimal Discount
        {
            get => DiscountSum.GetValueOrDefault();
            set
            {
                Sum = SumWithoutDiscount + value;
                DiscountSum = value;
            }
        }

        public decimal PriceWithoutDiscount
        {
            get => Amount == 0m
                ? Price.GetValueOrDefault()
                : Price.GetValueOrDefault() - ContainerCount * DiscountSum.GetValueOrDefault() / Amount;
            set => Price = Amount == 0m ? value : value + ContainerCount * DiscountSum.GetValueOrDefault() / Amount;
        }

        private decimal ContainerCount => ContainerId.HasValue && Product != null
                ? Product.GetContainerById(ContainerId.Value).Count
                : 1m;
    }
}
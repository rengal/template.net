using Resto.Common.Extensions;

namespace Resto.Data
{
    partial class SalesDocumentListRecord
    {
        public override decimal? DiscountSumValue
        {
            get { return DiscountSum.GetValueOrFakeDefault(); }
            set { DiscountSum = value; }
        }

        public override decimal? SelfPriceSumValue
        {
            get { return SelfPriceSum.GetValueOrFakeDefault(); }
            set { SelfPriceSum = value; }
        }
    }
}
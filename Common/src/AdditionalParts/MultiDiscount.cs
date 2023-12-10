using System.Linq;

namespace Resto.Data
{
    public partial class MultiDiscount
    {
        public override DiscountCard DiscountCard
        {
            get { return Discounts.OfType<DiscountCardItem>().Select(d => d.DiscountCard).FirstOrDefault(); }
        }
    }
}
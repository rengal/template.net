namespace Resto.Data
{
    public partial class DiscountCardItem
    {
        public override DiscountCard DiscountCard
        {
            get { return card; }
        }

        public override string GuestCard
        {
            get { return RolledCard != null ? RolledCard.Slip : ""; }
        }
    }
}
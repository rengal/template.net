namespace Resto.Data
{
    public partial class OrderSumCorrection
    {
        public virtual DiscountCard DiscountCard
        {
            get { return null; }
        }

        /// <summary>
        /// Карта гостя
        /// </summary>
        public virtual string GuestCard
        {
            get
            {
                return (DiscountCard != null) &&
                       (DiscountCard.Card.Slip != null)
                           ? DiscountCard.Card.Slip
                           : string.Empty;
            }
        }
    }
}
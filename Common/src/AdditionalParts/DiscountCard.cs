using System;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class DiscountCard
    {
        public DiscountCard(Guid id, LocalizableValue name, Card card, DiscountCardType type = null, ClientPriceCategory priceCategory = null)
            : base(id, name)
        {
            this.type = type;
            this.priceCategory = priceCategory;
            this.card = card;
        }

        /// <summary>
        /// Процент скидки, если она задана для данной карты, иначе <c>null</c>.
        /// </summary>
        public decimal? Percent
        {
            get { return type != null ? type.Percent.GetValueOrFakeDefault() : (decimal?)null; }
        }

        /// <summary>
        /// Трек.
        /// </summary>
        public string Track
        {
            get { return card.Slip; }
            set { card.Slip = value; }
        }

        /// <summary>
        /// Название типа скидки, если тип скидки задан для данной карты, иначе пустая строка.
        /// </summary>
        public string CardTypeName
        {
            get { return type == null ? "" : type.NameLocal; }
        }
    }
}
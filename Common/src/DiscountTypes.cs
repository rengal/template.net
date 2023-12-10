using System;
using Resto.Common.Extensions;

namespace Resto.Data
{
    public partial class DiscountType
    {
        public abstract string TypeName { get; }
    }

    public sealed partial class LuckyTicketDiscountType
    {
        public override string TypeName
        {
            get { return "LuckyTicket"; }
        }
    }

    public partial class DiscountCardType
    {
        public DiscountCardType(Guid id, string name)
        {
            Id = id;
            Name = new LocalizableValue(name);
        }

        public override string ToString()
        {
            return IsCategorisedDiscount || Mode != DiscountCardMode.PERCENT
                ? NameLocal
                : string.Format("{0} ({1}%)", NameLocal, -Percent.GetValueOrFakeDefault());
        }

        public override string TypeName
        {
            get { return !IsIikoCard51Discount ? "DiscountCard" : "iikoCard51Discount"; }
        }

        /// <summary>
        /// Признак скидки
        /// </summary>
        public bool IsDiscount
        {
            get { return Percent.GetValueOrFakeDefault() >= 0; }
        }

        /// <summary>
        /// Признак надбавки
        /// </summary>
        public bool IsIncrease
        {
            get { return Percent.GetValueOrFakeDefault() < 0; }
        }

        /// <summary>
        /// Признак скидки iikoCard5.
        /// </summary>
        public bool IsDefaultIikoCard51 => Id == PredefinedGuids.DISCOUNT_CARD_TYPE_IIKO_CARD5.Id;
    }

    public sealed partial class FlyerDiscountType
    {
        public FlyerDiscountType(Guid id)
        {
            Id = id;
        }

        public override string TypeName
        {
            get { return "Flyer"; }
        }
    }

    public sealed partial class ServiceFeeDiscountType
    {
        public override string TypeName
        {
            get { return "ServiceFeeDiscount"; }
        }

        public override string ToString()
        {
            return NameLocal;
        }
    }
}

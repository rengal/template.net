using Resto.Common.Extensions;
using Resto.Framework.Common.Currency;

namespace Resto.Data
{
    public partial class Currency : ICurrency
    {
        decimal ICurrency.MinimumDenomination
        {
            get { return MinimumDenomination.GetValueOrDefault(); }
            set { MinimumDenomination = value; }
        }
    }
}
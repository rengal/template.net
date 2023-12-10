using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Framework.Common.Currency;

namespace Resto.Data
{
    partial class CurrencyEntity : ICurrency
    {
        decimal ICurrency.MinimumDenomination
        {
            get { return MinimumDenomination.GetValueOrDefault(); }
            set { MinimumDenomination = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Common;
using Resto.Framework.Common.Currency;

namespace Resto.Data
{
    public partial class SumInCurrency : IComparable<SumInCurrency>, IComparable
    {
        public override string ToString()
        {
            string format = "{0:" + GuiSettings.MoneyFormatString + "}" + 
                (currency == null ? CurrencyHelper.GuiCurrencyName : currency.ShortNameForGui);

            return string.Format(format, sum);
        }

        public override bool Equals(object obj)
        {
            var other = obj as SumInCurrency;
            if (other == null)
            {
                return false;
            }
            return other.Sum == Sum && Equals(Currency, other.Currency);
        }

        public override int GetHashCode()
        {
            return new
            {
                Sum,
                Currency
            }.GetHashCode();
        }

        public int CompareTo(SumInCurrency other)
        {
            return OriginalSum.GetValueOrDefault()
                .CompareTo(other.OriginalSum.GetValueOrDefault());
        }

        public int CompareTo(object obj)
        {
            var other = obj as SumInCurrency;
            if (other == null)
            {
                return -1;
            }

            return CompareTo(other);
        }
    }
}

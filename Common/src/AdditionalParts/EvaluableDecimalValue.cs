using System;

using Resto.Common;
using Resto.Common.Extensions;
using Resto.Framework.Common.Currency;

namespace Resto.Data
{
    public partial class EvaluableDecimalValue : IComparable, IComparable<EvaluableDecimalValue>, IFormattable
    {
        public const char EvaluativeSuffix = '*';
        public static readonly EvaluableDecimalValue ZERO = new EvaluableDecimalValue(decimal.Zero, false);
        public static readonly EvaluableDecimalValue EVAL_ZERO = new EvaluableDecimalValue(decimal.Zero, true);
        public static readonly EvaluableDecimalValue ONE = new EvaluableDecimalValue(decimal.One, false);

        public EvaluableDecimalValue Add(EvaluableDecimalValue evaluableValue)
        {
            return new EvaluableDecimalValue(Value.GetValueOrFakeDefault() + evaluableValue.Value.GetValueOrFakeDefault(),
                                             Evaluative || evaluableValue.Evaluative);
        }

        public EvaluableDecimalValue Multiply(decimal coefficient)
        {
            return new EvaluableDecimalValue(Value.GetValueOrFakeDefault() * coefficient, evaluative);
        }

        public EvaluableDecimalValue Multiply(decimal coefficient, int fractionDigits)
        {
            return new EvaluableDecimalValue(Math.Round(Value.GetValueOrFakeDefault() * coefficient, fractionDigits, MidpointRounding.AwayFromZero), evaluative);
        }

        public EvaluableDecimalValue Divide(decimal coefficient)
        {
            return new EvaluableDecimalValue(Value.GetValueOrFakeDefault() / coefficient, evaluative);
        }

        public EvaluableDecimalValue Divide(decimal coefficient, int fractionDigits)
        {
            return new EvaluableDecimalValue(Math.Round(Value.GetValueOrFakeDefault() / coefficient, fractionDigits, MidpointRounding.AwayFromZero), evaluative);
        }

        public static decimal? GetNullableDecimal(EvaluableDecimalValue value)
        {
            return value == null ? null : value.Value;
        }

        public override string ToString()
        {
            return ToString(GuiSettings.MoneyFormatString, null);
        }

        public EvaluableDecimalValue RoundAsSum()
        {
            return new EvaluableDecimalValue(Value.GetValueOrFakeDefault().MoneyPrecisionMoneyRound(), evaluative);
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            return CompareTo((EvaluableDecimalValue)obj);
        }

        #endregion

        #region IFormattable Members

        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                format = GuiSettings.MoneyFormatString;

            return Value.GetValueOrFakeDefault().ToString(format, formatProvider) + (evaluative ? new string(EvaluativeSuffix, 1) : "");
        }

        #endregion

        #region IComparable<EvaluableDecimalValue> Members

        public int CompareTo(EvaluableDecimalValue other)
        {
            if (other == null)
                return 1;

            int result = Value.GetValueOrFakeDefault().CompareTo(other.Value.GetValueOrFakeDefault());
            if (result == 0)
            {
                result = other.Evaluative.CompareTo(Evaluative);
            }

            return result;
        }

        #endregion
    }
}
using System;
using System.Globalization;
using Resto.Framework.Common.Phone;

namespace Resto.Framework.Common.Search
{
    /// <summary>
    /// Поисковый токен — элемент строки поиска, подготовленный для проверки поддерживаемыми способами.
    /// (Фактически, строка сплитится на токены по пробелам).
    /// </summary>
    internal sealed class SearchToken
    {
        private readonly string s;
        private readonly string stringOnlyDigits;
        private readonly string normalizedPhone;
        private readonly decimal decimalValue;
        private readonly decimal decimalAbsLower;
        private readonly decimal decimalAbsUpper;
        private readonly bool isDecimal;
        private readonly bool isPhone;

        public SearchToken(string searchString, bool needParseDecimal, bool needParseDigits, bool isPhone)
        {
            if (searchString == null)
                searchString = string.Empty;

            s = searchString;

            if (needParseDecimal)
            {
                try
                {
                    if (searchString.Length == 0 || (searchString[0] == '0' && searchString.Length != 1 && (searchString[1] != '.' || searchString[1] != ',')))
                        throw new FormatException("Decimals beginning with '0' are treated as strings (0.123 and 0 are decimal, but 0123 is not).");

                    var sd = searchString.Replace(',', '.');
                    decimalValue = decimal.Parse(sd, NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                    var posDot = sd.IndexOf('.');
                    decimalAbsLower = Math.Abs(Decimal);
                    decimalAbsUpper = posDot != -1
                        ? DecimalAbsLower + 10m.Pow(posDot - sd.Length + 1)
                        : DecimalAbsLower + 1m;
                    isDecimal = true;
                }
                catch
                {
                    isDecimal = false;
                }
            }

            if (needParseDigits)
                stringOnlyDigits = searchString.LeaveOnlyDigits();

            if (isPhone)
            {
                normalizedPhone = PhoneUtils.TryNormalizeUncompletedPhone(searchString, CultureInfo.CurrentUICulture.Name);
                if (!string.IsNullOrEmpty(NormalizedPhone))
                    this.isPhone = true;
            }
        }

        public string String
        {
            get { return s; }
        }

        public string StringOnlyDigits
        {
            get { return stringOnlyDigits; }
        }

        public string NormalizedPhone
        {
            get { return normalizedPhone; }
        }

        public decimal Decimal
        {
            get { return decimalValue; }
        }

        public decimal DecimalAbsLower
        {
            get { return decimalAbsLower; }
        }

        public decimal DecimalAbsUpper
        {
            get { return decimalAbsUpper; }
        }

        public bool IsDecimal
        {
            get { return isDecimal; }
        }

        public bool IsPhone
        {
            get { return isPhone; }
        }
    }
}
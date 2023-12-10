using System;
using System.Globalization;
using Resto.Data;
using Resto.Framework.Common;
using Resto.Framework.Common.Currency;

namespace Resto.Common
{
    #region DecimalExtentions

    /// <summary>
    /// Класс-расширение для работы с типом decimal.
    /// </summary>
    public static class DecimalExtentions
    {
        /// <summary>
        /// Округляет значение кол-ва так же как и на сервере.
        /// До 3-х знаков после запятой.
        /// </summary>
        public static decimal RoundAmountAsServer(this decimal value)
        {
            return Math.Round(value, 3, MidpointRounding.AwayFromZero);
        }

        public static decimal RoundToFourDigits(this decimal value)
        {
            return Math.Round(value, 4, MidpointRounding.AwayFromZero);
        }

        public static decimal RoundToFiveDigits(this decimal value)
        {
            return Math.Round(value, 5, MidpointRounding.AwayFromZero);
        }

        public static decimal Round(this decimal value, RoundMode mode)
        {
            return mode == RoundMode.UP
                       ? Math.Truncate(value) + 1
                       : mode == RoundMode.DOWN
                             ? Math.Truncate(value)
                             : Math.Round(value, 0, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Округляет данное число в указанную сторону с указанной точностью.
        /// </summary>
        /// <param name="value">Число для округления.</param>
        /// <param name="mode">Режим округления.</param>
        /// <param name="precision">Точность округления.</param>
        /// <remarks>Результат округления.</remarks>
        public static decimal Round(this decimal value, RoundMode mode, decimal precision)
        {
            //int Цена_в_копейках;
            int cents = (int)(Math.Round(value,2)*100);

            //int точность_округления_в_копейках;
            int precisionInCents = (int) (precision*100);

            //Если Цена_в_копейках % точность_округления_в_копейках = 0 – округлять не надо
            if (cents % precisionInCents == 0)
            {
                return (decimal)cents / 100;
            }

            //int количество_кратности = Цена_в_копейках/точность_округления_в_копейках;
            int whole = cents/precisionInCents;

            //int Большее_целое_в_копейках = ((количество_кратности + 1)*точность_округления_в_копейках);
            int upInCents = (whole + 1)*precisionInCents;

            //int Меньшее_целое_в_копейках = (количество_кратности*точность_округления_в_копейках);
            int downInCents = whole * precisionInCents;

            if (mode == RoundMode.UP)
            {
                return (decimal)upInCents/100;
            }
            if (mode == RoundMode.DOWN)
            {
                return (decimal)downInCents/100;
            }

            //int Ближайшее_целое_в_копейках = 
            // (Большее_целое_в_копейках – цена_в_копейках) < (Меньшее_целое_в_копейках – цена_в_копейках) : Меньшее_целое_в_копейках
            if (mode == RoundMode.MATH)
            {
                bool upIsNear = Math.Abs(upInCents - cents) < Math.Abs(downInCents - cents);
                return upIsNear ? (decimal)upInCents / 100 : (decimal)downInCents / 100;
            }

            throw new RestoException(string.Format("Unexpected value of mode:{0}.", mode));
        }

        /// <summary>
        /// Конвертирует Nullable decimal в Nullable double.
        /// </summary>
        public static double? ToNullableDouble(this decimal? value)
        {
            return value.HasValue ? Convert.ToDouble(value.Value) : (double?)null;
        }

        /// <summary>
        /// Возвращает строковое представление <paramref name="value"/>.
        /// </summary>
        public static string GetStringValue(this decimal? value)
        {
            return value.HasValue ? value.Value.ToString(CultureInfo.CurrentCulture) : "null";
        }
    }
    #endregion DecimalExtentions
}
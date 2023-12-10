using System;

namespace Resto.Common
{
    /// <summary>
    /// Класс-расширение для работы с типом double.
    /// </summary>
    public static class DoubleExtentions
    {
        /// <summary>
        /// Округляет значение суммы так же как и на сервере.
        /// До 2-х знаков после запятой.
        /// </summary>
        public static double RoundSumAsServer(this double value)
        {
            return Math.Round(value, 2, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Округляет значение кол-ва так же как и на сервере.
        /// До 3-х знаков после запятой.
        /// </summary>
        public static double RoundAmountAsServer(this double value)
        {
            return Math.Round(value, 3, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// Возвращает true, если оба значения равны.
        /// Поскольку сравниваются double значения, то приблизительно равны.
        /// </summary>
        public static bool EqualsEx(this double value, double other)
        {
            return Math.Abs(value - other) < Double.Epsilon;
        }

        /// <summary>
        /// Возвращает true, если значение равно 0.
        /// Поскольку сравниваются double значения, то приблизительно равно 0.
        /// </summary>
        public static bool IsZero(this double value)
        {
            return value.EqualsEx(0);
        }

        /// <summary>
        /// Конвертирует Nullable double в Nullable decimal.
        /// </summary>
        public static decimal? ToNullableDecimal(this double? value)
        {
            return value.HasValue ? Convert.ToDecimal(value.Value) : (decimal?)null;
        }
    }

}

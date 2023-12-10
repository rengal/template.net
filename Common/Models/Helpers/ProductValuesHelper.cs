using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Common.Models.Helpers
{
    public static class ProductValuesHelper
    {
        private const decimal KJoulesPerKCal = 4.184m;

        /// <summary>
        /// Рассчитывает энергетическую ценность в кДж по значению ккал.
        /// </summary>
        [Pure]
        public static decimal CalculateKJouleFromKCal(decimal kcals)
        {
            return KJoulesPerKCal * kcals;
        }

        /// <summary>
        /// Округляет и форматирует значение жиров, белков или углеводов в соответствии с ГОСТом.
        /// </summary>
        [NotNull]
        [Pure]
        public static string FormatMacrosByGOST(decimal value)
        {
            // "Количество белков, жиров углеводов, г..."
            if (value < 0.5m)
            {
                // менее 0.5 - Указывается значение до первого десятичного знака после запятой".
                return value.ToString("0.#");
            }
            if (value <= 10m)
            {
                // "от 0.5 до 10 включительно - до ближайшего значения, кратного 0.5г".
                return (Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2).ToString("0.#");
            }
            // "Свыше 10 - до ближайшего целого числа, кратного 1г".
            return value.ToString("0");
        }

        /// <summary>
        /// Округляет и форматирует значение энергетической ценности в соответствии с ГОСТом.
        /// </summary>
        [NotNull]
        [Pure]
        public static string FormatEnergyByGOST(decimal value)
        {
            // "Энергетическая ценность (калорийность), кДж/ккал..."
            if (value < 1m)
            {
                // "Менее 1 - указывается "1"".
                return decimal.One.ToString("0");
            }
            if (value <= 5m)
            {
                // "От 1 до 5 включительно - До ближайшего целого числа".
                return value.ToString("0");
            }
            if (value <= 100m)
            {
                // "От 5 до 100 включительно - До ближайшего целого числа, кратного 5".
                return (Math.Round(value / 5m) * 5m).ToString("0");
            }
            // "Свыше 100 - До ближайшего целого числа, кратного 10".
            return (Math.Round(value / 10m) * 10m).ToString("0");
        }
    }
}

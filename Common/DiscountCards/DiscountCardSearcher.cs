using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Resto.Data;
using Resto.Framework.Data;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Common;

namespace Resto.Common.DiscountCards
{
    /// <summary>
    /// Класс для поиска скидочных карт.
    /// </summary>
    public static class DiscountCardSearcher
    {
        /// <summary>
        /// Находит максимульную по скидке карту, слип которой соответствует номеру <paramref name="cardNumber"/>.
        /// </summary>
        public static DiscountCard SearchMaxDiscountCard([CanBeNull] string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
                return null;

            IEnumerable<DiscountCard> discountCards = EntityManager.INSTANCE.GetAllNotDeleted<DiscountCard>(
                d => d.Card.Slip.Equals(cardNumber)
                    || IsMatch(d.Card.Slip, cardNumber));
            if (!discountCards.Any())
                return null;

            decimal? maxPercent = discountCards.Max(d => d.Percent);
            return discountCards.First(d => d.Percent == maxPercent);
        }

        /// <summary>
        /// Находит максимульный тип по скидке карте, слип которой соответствует номеру <paramref name="cardNumber"/>.
        /// </summary>
        [CanBeNull]
        public static DiscountCardType SearchMaxDiscountType([NotNull]string cardNumber)
        {
            DiscountCard discountCard = SearchMaxDiscountCard(cardNumber);
            return discountCard != null ? discountCard.Type : null;
        }

        public static bool IsMatch([CanBeNull] string cardTemplate, [NotNull] string cardNumber)
        {
            if (cardNumber == null)
                throw new ArgumentNullException(nameof(cardNumber));

            if (string.IsNullOrEmpty(cardTemplate))
                return false;

            return cardTemplate == cardNumber || IsCardGroup(cardTemplate) && CardMatchingType(cardTemplate).IsMatch(cardNumber);
        }

        public static bool IsCardGroup([NotNull] string cardTemplate)
        {
            if (cardTemplate == null)
                throw new ArgumentNullException(nameof(cardTemplate));

            return cardTemplate.Contains("*") || cardTemplate.Contains("?");
        }

        [NotNull]
        private static Regex CardMatchingType([NotNull] string cardTemplate)
        {
            if (cardTemplate == null)
                throw new ArgumentNullException(nameof(cardTemplate));

            return new Regex("^" +
                             Regex.Escape(cardTemplate)             // обезопасим всю строку (а вдруг она невалидна?)
                                  .Replace(Regex.Escape("*"), ".*") // восстановим символы '*'
                                  .Replace(Regex.Escape("?"), ".")  // и '?'
                             + "$", RegexOptions.Compiled);
        }

        public static bool CheckCardTrack(string input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.Length == 0)
                return true;

            return CardUtils.AnySymbolsRegex.IsMatch(input);
        }
    }
}
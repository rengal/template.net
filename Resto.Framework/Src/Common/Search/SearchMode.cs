using System;

namespace Resto.Framework.Common.Search
{
    [Flags]
    public enum SearchMode
    {
        /// <summary>Не искать вхождение чисел в строковые свойства (например, числа в списке блюд закрытого заказа).</summary>
        IgnoreStringPropertiesOnDecimalSearch = 1 << 0
    }

    internal static class SearcherModeExtensions
    {
        public static bool Contains(this SearchMode a, SearchMode b)
        {
            return (a & b) > 0;
        }
    }
}
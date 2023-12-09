using System;

namespace Resto.Framework.Common.Search
{
    [Flags]
    public enum PropertySearchMode
    {
        /// <summary>Вхождение подстроки в строку(например, фамилия, имя, адрес).</summary>
        StringInclude = 1 << 0,
        /// <summary>Вхождение подстроки в строку, от которой оставлены только цифры (например, телефон).</summary>
        StringDigitsInclude = 1 << 1,
        /// <summary>Точное равенство чисел (например, количество).</summary>
        DecimalEquals = 1 << 2,
        /// <summary>Точное равенство целой части чисел, дробная проверяется со введённой точностью (например, сумма).</summary>
        DecimalStartsWith = 1 << 3,
        /// <summary>Списковое поле (дополнительный флаг к поиску по строке или числу).</summary>
        List = 1 << 4,
        /// <summary>Вхождение подстроки в строку вначале строки - префиксный поиск</summary>
        StringStartsWith = 1 << 5,
        /// <summary>
        /// Вхождение подстроки в строку с учетом нормализации телефонов
        /// </summary>
        Phones = 1 << 6,

        StringModes = StringInclude | StringDigitsInclude | Phones,
        DecimalModes = DecimalEquals | DecimalStartsWith,
        AllModes = StringModes | DecimalModes | StringStartsWith
    }

    internal static class SearcherModePropertyExtensions
    {
        public static bool Contains(this PropertySearchMode a, PropertySearchMode b)
        {
            return (a & b) > 0;
        }
    }
}
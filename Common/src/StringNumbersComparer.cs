using System;
using System.Collections.Generic;

namespace Resto.Data
{
    /// <summary>
    /// Компаратор строковых представлений чисел.
    /// Пытается сортировать строки, как числа (1, 2, 10, ...),
    /// если это не удаётся (какие-то из строк не являются числовыми),
    /// то сортирует как строки (1, 10a, 2, ...).
    /// </summary>
    public sealed class StringNumbersComparer : IComparer<string>
    {
        public int Compare(string value1, string value2)
        {
            int intValue1;
            int intValue2;
            return int.TryParse(value1, out intValue1) && int.TryParse(value2, out intValue2)
                ? intValue1 - intValue2
                : string.CompareOrdinal(value1, value2);
        }
    }
}

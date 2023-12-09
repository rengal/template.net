using System;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class LanguageUtils
    {
        /// <summary>
        /// Выбирает форму множественного числа слова в соответствии с количеством. 
        /// </summary>
        /// <param name="count">Количество (от 0 до <see cref="int.MaxValue"><c>Int32.MaxValue</c></see>).</param>
        /// <param name="wordPluralForms">Формы слова, перечисленные через точку с запятой</param>
        /// <returns>
        /// Слово в нужной форме множественного числа
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/><c> &lt; 0</c>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="wordPluralForms"/><c> == null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="wordPluralForms"/> не содержит непустых форм слова (состоит из пробелов и точек с запятой).
        /// </exception>
        [NotNull]
        public static string SelectWordFormAccordingToCount(int count, [NotNull] string wordPluralForms)
        {
            if (wordPluralForms == null)
                throw new ArgumentNullException(nameof(wordPluralForms));

            var pluralForms = wordPluralForms
                .Split(';')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            if (pluralForms.Count == 0)
                throw new ArgumentException("Parameter contains no word forms", nameof(wordPluralForms));

            if (pluralForms.Count < 3)
            {
                var defaultWordForm = pluralForms.Last();
                while (pluralForms.Count < 3)
                {
                    pluralForms.Add(defaultWordForm);
                }
            }
            return SelectWordFormAccordingToCount(count, pluralForms[0], pluralForms[1], pluralForms[2]);
        }

        /// <summary>
        /// Выбирает форму множественного числа слова в соответствии с количеством. 
        /// </summary>
        /// <param name="count">Количество (от 0 до <see cref="int.MaxValue"><c>Int32.MaxValue</c></see>).</param>
        /// <param name="wordWithEndFor1">
        /// Форма слова, используемая при <paramref name="count"/>,
        /// оканчивающимся на 1 (кроме чисел, оканчивающихся на 11).
        /// </param>
        /// <param name="wordWithEndFrom2To4">
        /// Форма слова, используемая при <paramref name="count"/>,
        /// оканчивающимся на 2, 3 или 4 (кроме чисел, оканчивающихся на 12, 13 или 14).
        /// </param>
        /// <param name="wordWithEndFrom5To0">
        /// Форма слова, используемая при <paramref name="count"/>,
        /// оканчивающимся на 0, 5, 6, 7, 8, 9 и оканчивающимся на 11, 12, 13 или 14.
        /// </param>
        /// <returns>
        /// Слово в нужной форме множественного числа
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/><c> &lt; 0</c>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="wordWithEndFor1"/><c> == null</c> или
        /// <paramref name="wordWithEndFrom2To4"/><c> == null</c> или
        /// <paramref name="wordWithEndFrom5To0"/><c> == null</c>.
        /// </exception>
        [NotNull]
        public static string SelectWordFormAccordingToCount(int count, [NotNull] string wordWithEndFor1, [NotNull] string wordWithEndFrom2To4, [NotNull] string wordWithEndFrom5To0)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be greater than zero.");
            if (wordWithEndFor1 == null)
                throw new ArgumentNullException(nameof(wordWithEndFor1));
            if (wordWithEndFrom2To4 == null)
                throw new ArgumentNullException(nameof(wordWithEndFrom2To4));
            if (wordWithEndFrom5To0 == null)
                throw new ArgumentNullException(nameof(wordWithEndFrom5To0));

            var remain = count % 100;

            if (remain > 10 && remain < 15)
                return wordWithEndFrom5To0;

            remain %= 10;

            switch (remain)
            {
                case 1:
                    return wordWithEndFor1;
                case 2:
                case 3:
                case 4:
                    return wordWithEndFrom2To4;
                default:
                    return wordWithEndFrom5To0;
            }
        }
    }
}

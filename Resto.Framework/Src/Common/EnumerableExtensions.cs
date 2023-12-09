using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;

using Resto.Framework.Src.Common;

using Wintellect.PowerCollections;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Методы для работы с <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Индекс ненайденного значения.
        /// </summary>
        public const int NotFound = -1;

        /// <summary>
        /// Возвращает последовательность, образованную добавлением <paramref name="item"/> в
        /// конец последовательности <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Последовательность, в конец которой добавляется элемент <paramref name="item"/>.</param>
        /// <param name="item">Добавляемый элемент.</param>
        /// <returns>
        /// Последовательность, образованная добавлением <paramref name="item"/> в
        /// конец последовательности <paramref name="sequence"/>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        [NotNull, Pure]
        public static IEnumerable<TSource> ContinueWith<TSource>([NotNull] this IEnumerable<TSource> sequence, TSource item)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));

            return sequence.Concat(item.AsSequence());
        }

        /// <summary>
        /// Возвращает последовательность, образованную добавлением <paramref name="items"/> в
        /// конец последовательности <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Последовательность, в конец которой добавляется элементы <paramref name="items"/>.</param>
        /// <param name="items">Добавляемые элементы.</param>
        /// <returns>
        /// Последовательность, образованная добавлением <paramref name="items"/> в
        /// конец последовательности <paramref name="sequence"/>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c> или <paramref name="items"/><c> == null</c>.
        /// </exception>
        [NotNull, Pure]
        public static IEnumerable<TSource> ContinueWith<TSource>([NotNull] this IEnumerable<TSource> sequence, [NotNull] params TSource[] items)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            return sequence.Concat(items);
        }

        /// <summary>
        /// Возвращает последовательность, образованную вставкой <paramref name="item"/> в
        /// последовательность <paramref name="sequence"/> в позицию с индексом <paramref name="index"/>.
        /// Если количество элементов в последовательности <paramref name="sequence"/> больше или равно
        /// значению <paramref name="index"/>, <paramref name="item"/> добавляется в конец последовательности.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Последовательность, в которую вставляется элемент <paramref name="item"/>.</param>
        /// <param name="index">
        /// Отсчитываемый от нуля индекс позиции в последовательности <paramref name="sequence"/>
        /// в которую будет вставлен элемент <paramref name="item"/>.
        /// </param>
        /// <param name="item">Вставляемый элемент.</param>
        /// <returns>
        /// Последовательность, образованная вставкой <paramref name="item"/> в
        /// последовательность <paramref name="sequence"/> в позицию с индексом <paramref name="index"/>.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/><c> &lt; 0</c>.
        /// </exception>
        [NotNull, Pure]
        public static IEnumerable<TSource> SequenceInsert<TSource>([NotNull] this IEnumerable<TSource> sequence, int index, TSource item)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, "Index cannot be less than zero.");

            return sequence.SequenceInsertCore(index, item);
        }

        [NotNull, Pure]
        private static IEnumerable<TSource> SequenceInsertCore<TSource>([NotNull] this IEnumerable<TSource> sequence, int index, TSource item)
        {
            var counter = 0;
            var inserted = false;
            foreach (var t in sequence)
            {
                if (!inserted)
                {
                    if (counter == index)
                    {
                        inserted = true;
                        yield return item;
                    }
                    else
                    {
                        counter++;
                    }
                }
                yield return t;
            }
            if (!inserted)
                yield return item;
        }

        /// <summary>
        /// Преобразует элемент <paramref name="item"/> в последовательность, состоящую из одного элемента.
        /// </summary>
        /// <typeparam name="TSource">Тип элемента <paramref name="item"/> и тип элементов возвращаемой последовательности.</typeparam>
        /// <param name="item">Элемент, преобразуемый в последовательность.</param>
        /// <returns>Последовательность, состоящая из одного элемента, <paramref name="item"/>.</returns>
        [NotNull, Pure]
        public static IEnumerable<TSource> AsSequence<TSource>(this TSource item)
        {
            return EnumerableEx.Return(item);
        }

        /// <summary>
        /// Возвращает текстовое представление последовательности элементов через запятую.
        /// </summary>
        /// <param name="sequence">Последовательность элементов любого типа, может содержать null'ы.</param>
        public static string AsText(this IEnumerable sequence)
        {
            return string.Format("[{0}]",
                sequence.Cast<object>()
                    .Select(elem => elem == null ? StringExtensions.NullRepresentation : elem.ToString())
                    .JoinWithComma());
        }

        /// <summary>
        /// Проверяет, что последовательность <paramref name="sequence"/> содержит
        /// не менее чем <paramref name="count"/> элементов.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Проверяемая последовательность</param>
        /// <param name="count">Минимально допустимое количество элементов.</param>
        /// <returns>
        /// <c>true</c>, если количество элементов в последовательности <paramref name="sequence"/> больше или равно <paramref name="count"/>,
        /// <c>false</c> в противном случае.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/><c> &lt; 0.</c>
        /// </exception>
        [Pure]
        public static bool AtLeast<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence, int count)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count cannot be less than zero.");

            var counter = 0;
            using (var enumerable = sequence.GetEnumerator())
            {
                while (enumerable.MoveNext())
                {
                    if (++counter >= count)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Проверяет, что последовательность <paramref name="sequence"/> содержит
        /// не менее чем <paramref name="count"/> элементов, удовлетворяющих предикату <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Проверяемая последовательность</param>
        /// <param name="count">Минимально допустимое количество элементов.</param>
        /// <param name="predicate">Предикат, определяющий учитываемые элементы.</param>
        /// <returns>
        /// <c>true</c>, если количество элементов в последовательности <paramref name="sequence"/>, удовлетворяющих предикату,
        /// больше или равно <paramref name="count"/>,
        /// <c>false</c> в противном случае.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/><c> &lt; 0.</c>
        /// </exception>
        [Pure]
        public static bool AtLeast<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence, int count,
                                            [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count cannot be less than zero.");

            return sequence.Where(predicate).AtLeast(count);
        }

        /// <summary>
        /// Проверяет, что последовательность <paramref name="sequence"/> содержит
        /// не более чем <paramref name="count"/> элементов.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Проверяемая последовательность</param>
        /// <param name="count">Максимально допустимое количество элементов.</param>
        /// <returns>
        /// <c>true</c>, если количество элементов в последовательности <paramref name="sequence"/> меньше или равно <paramref name="count"/>,
        /// <c>false</c> в противном случае.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/><c> &lt; 0.</c>
        /// </exception>
        [Pure]
        public static bool AtMost<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence, int count)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count cannot be less than zero.");

            var counter = 0;
            using (var enumerable = sequence.GetEnumerator())
            {
                while (enumerable.MoveNext())
                {
                    if (++counter > count)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Проверяет, что последовательность <paramref name="sequence"/> содержит
        /// не более чем <paramref name="count"/> элементов, удовлетворяющих предикату <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Проверяемая последовательность</param>
        /// <param name="count">Максимально допустимое количество элементов.</param>
        /// <param name="predicate">Предикат, определяющий учитываемые элементы.</param>
        /// <returns>
        /// <c>true</c>, если количество элементов в последовательности <paramref name="sequence"/>, удовлетворяющих предикату,
        /// меньше или равно <paramref name="count"/>,
        /// <c>false</c> в противном случае.
        /// </returns>
        /// 
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="count"/><c> &lt; 0.</c>
        /// </exception>
        [Pure]
        public static bool AtMost<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence, int count,
                                           [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count cannot be less than zero.");

            return sequence.Where(predicate).AtMost(count);
        }

        /// <summary>
        /// Ищет первый дубликат в последовательности <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Последовательность, в которой ищется дубликат.</param>
        /// <returns>Первый найденный дубликат, или default(<typeparamref name="TSource"/>), если дубликатов нет.</returns>
        [Pure]
        public static TSource FirstDuplicateOrDefault<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));

            var hashSet = new HashSet<TSource>();
            return sequence.FirstOrDefault(e => !hashSet.Add(e));
        }

        /// <summary>
        /// Ищет дубликаты в последовательности <paramref name="sequence"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Последовательность, в которой ищутся дубликаты.</param>
        /// <returns>true, если дубликаты есть, иначе false.</returns>
        [Pure]
        public static bool HasDuplicates<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));

            var hashSet = new HashSet<TSource>();
            return sequence.Any(e => !hashSet.Add(e));
        }

        /// <summary>
        /// Получает положение элемента <paramref name="element"/> в последовательности <paramref name="sequence"/>.
        /// Равенство проверяется методом TSource.Equals. Если элемент встречается несколько раз,
        /// возвращается положение первого из них.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Последовательность, по которой производится поиск.</param>
        /// <param name="element">Искомый элемент.</param>
        /// <returns>Положение элемента в последовательности, если он найден; <see cref="NotFound"/> (-1) в противном случае.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        [Pure]
        public static int IndexOf<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence, TSource element)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));

            var comparer = EqualityComparer<TSource>.Default;

            var index = 0;
            foreach (var item in sequence)
            {
                if (comparer.Equals(element, item))
                    return index;
                index++;
            }
            return NotFound;
        }

        /// <summary>
        /// Получает положение элемента в последовательности <paramref name="sequence"/>, удовлетворяющего предикату
        /// <paramref name="predicate"/>. Если предикату удовлетворяет несколько элементов, возвращается положение первого из них.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Последовательность, по которой производится поиск.</param>
        /// <param name="predicate">Предикат, определяющий искомый элемент.</param>
        /// <returns>Положение элемента в последовательности, если он найден; <see cref="NotFound"/> (-1) в противном случае.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="predicate"/><c> == null</c>.
        /// </exception>
        [Pure]
        public static int IndexOf<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> sequence, [NotNull, InstantHandle] Func<TSource, bool> predicate)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            var index = 0;
            foreach (var item in sequence)
            {
                if (predicate(item))
                    return index;
                index++;
            }
            return NotFound;
        }

        /// <summary>
        /// Группирует исходную последовательность <paramref name="sequenceSelector"/> по ключу, возвращаемому <paramref name="keySelector"/>,
        /// и возвращает группы элементов, преобразованные <paramref name="sequenceSelector"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TKey">Тип ключа, по которому производится группировка.</typeparam>
        /// <typeparam name="TResult">Тип элементов в группах результирующей последовательности.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="keySelector">Ключ, по которому производится группировка.</param>
        /// <param name="sequenceSelector">Преобразование последовательности.</param>
        /// <returns>Последовательность групп.</returns>
        [NotNull, Pure]
        public static IEnumerable<IGrouping<TKey, TResult>> GroupBySelect<TSource, TKey, TResult>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, TKey> keySelector,
            [NotNull] Func<IEnumerable<TSource>, IEnumerable<TResult>> sequenceSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (sequenceSelector == null)
                throw new ArgumentNullException(nameof(sequenceSelector));

            return source
                .GroupBy(keySelector)
                .Select(group => new Grouping<TKey, TResult>(group.Key, sequenceSelector(group)).AsGrouping());
        }

        /// <summary>
        /// Вернуть значение из первого элемента последовательности <paramref name="source"/>, удовлетворяющего
        /// предикату <paramref name="predicate"/>, или <paramref name="defaultValue"/>, если ни один элемент предикату
        /// не удовлетворяет.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов, по которым производится поиск.</typeparam>
        /// <typeparam name="TResult">Тип результата.</typeparam>
        /// <param name="source">Входная последовательность.</param>
        /// <param name="predicate">Предикат, проверяющий входную последовательность.</param>
        /// <param name="resultSelector">Функция для извлечения результата из найденного элемента.</param>
        /// <param name="defaultValue">Значение, возвращаемое, если элемент не найден.</param>
        /// <returns>
        /// Результат, возвращённый <paramref name="resultSelector"/>, если элемент найден.
        /// В противном случае <paramref name="defaultValue"/>.
        /// </returns>
        [Pure]
        public static TResult FirstOrDefault<TSource, TResult>(
            [NotNull, InstantHandle] this IEnumerable<TSource> source,
            [NotNull, InstantHandle] Func<TSource, bool> predicate,
            [NotNull, InstantHandle] Func<TSource, TResult> resultSelector,
            TResult defaultValue)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            foreach (var item in source)
                if (predicate(item))
                    return resultSelector(item);
            return defaultValue;
        }

        /// <summary>
        /// Вернуть значение из первого элемента последовательности <paramref name="source"/>, удовлетворяющего
        /// предикату <paramref name="predicate"/>, или значение по умолчанию для типа <typeparamref name="TResult"/>, если
        /// ни один элемент предикату не удовлетворяет.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов, по которым производится поиск.</typeparam>
        /// <typeparam name="TResult">Тип результата.</typeparam>
        /// <param name="source">Входная последовательность.</param>
        /// <param name="predicate">Предикат, проверяющий входную последовательность.</param>
        /// <param name="resultSelector">Функция для извлечения результата из найденного элемента.</param>
        /// <returns>
        /// Результат, возвращённый <paramref name="resultSelector"/>, если элемент найден.
        /// В противном случае значение по умолчанию для типа <typeparamref name="TResult"/>.
        /// </returns>
        [Pure]
        public static TResult FirstOrDefault<TSource, TResult>(
            [NotNull, InstantHandle] this IEnumerable<TSource> source,
            [NotNull, InstantHandle] Func<TSource, bool> predicate,
            [NotNull, InstantHandle] Func<TSource, TResult> resultSelector)
        {
            return FirstOrDefault(source, predicate, resultSelector, default(TResult));
        }

        /// <summary>
        /// Преобразовать последовательность <paramref name="source"/> с помощью <paramref name="selector"/> в набор последовательностей
        /// и объединить последовательности в одну, разделяя их <paramref name="separator"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TResult">Тип элементов результирующей последовательности.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="selector">Функция для преобразования исходных элементов в последовательности.</param>
        /// <param name="separator">Разделитель между объединёнными последовательностями.</param>
        /// <returns>Объединённая последовательность.</returns>
        [NotNull, Pure]
        public static IEnumerable<TResult> SelectManyWithSeparator<TSource, TResult>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, IEnumerable<TResult>> selector,
            [NotNull] IEnumerable<TResult> separator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (separator == null)
                throw new ArgumentNullException(nameof(separator));

            return source.SelectManyWithSeparatorCore(selector, separator);
        }

        [NotNull, Pure]
        private static IEnumerable<TResult> SelectManyWithSeparatorCore<TSource, TResult>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, IEnumerable<TResult>> selector,
            [NotNull] IEnumerable<TResult> separator)
        {
            var i = 0;
            foreach (var sub in source)
            {
                if (i++ > 0)
                    foreach (var item in separator)
                        yield return item;
                foreach (var item in selector(sub))
                    yield return item;
            }
        }

        /// <summary>
        /// Преобразовать последовательность <paramref name="source"/> с помощью <paramref name="selector"/> в набор последовательностей
        /// и объединить последовательности в одну, разделяя их <paramref name="separator"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TResult">Тип элементов результирующей последовательности.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="selector">Функция для преобразования исходных элементов в последовательности.</param>
        /// <param name="separator">Разделитель между объединёнными последовательностями.</param>
        /// <returns>Объединённая последовательность.</returns>
        [NotNull, Pure]
        public static IEnumerable<TResult> SelectManyWithSeparator<TSource, TResult>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, IEnumerable<TResult>> selector,
            [NotNull] TResult separator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));

            return source.SelectManyWithSeparatorCore(selector, separator);
        }

        [NotNull, Pure]
        private static IEnumerable<TResult> SelectManyWithSeparatorCore<TSource, TResult>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, IEnumerable<TResult>> selector,
            [NotNull] TResult separator)
        {
            var i = 0;
            foreach (var sub in source)
            {
                if (i++ > 0)
                    yield return separator;
                foreach (var item in selector(sub))
                    yield return item;
            }
        }

        /// <summary>
        /// Преобразовать последовательность <paramref name="source"/> с помощью <paramref name="selector"/>
        /// и разделить элементы <paramref name="separator"/>.
        /// </summary>
        /// <typeparam name="TSource">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TResult">Тип элементов результирующей последовательности.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="selector">Функция для преобразования исходных элементов в последовательности.</param>
        /// <param name="separator">Функция, возвращающая разделитель между элементами. Параметры: элемент, предшествующий разделителю, и элемент, следующий за разделителем.</param>
        /// <returns>Объединённая последовательность.</returns>
        [NotNull, Pure]
        public static IEnumerable<TResult> SelectWithSeparator<TSource, TResult>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, TResult> selector,
            [NotNull] Func<TSource, TSource, TResult> separator)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            if (separator == null)
                throw new ArgumentNullException(nameof(separator));

            return source.SelectWithSeparatorCore(selector, separator);
        }

        [NotNull, Pure]
        private static IEnumerable<TResult> SelectWithSeparatorCore<TSource, TResult>(
            [NotNull] this IEnumerable<TSource> source,
            [NotNull] Func<TSource, TResult> selector,
            [NotNull] Func<TSource, TSource, TResult> separator)
        {
            var i = 0;
            var prevSub = default(TSource);
            foreach (var sub in source)
            {
                if (i++ > 0)
                    yield return separator(prevSub, sub);
                yield return selector(sub);
                prevSub = sub;
            }
        }

        [Pure]
        public static double AverageOrDefault([NotNull, InstantHandle] this IEnumerable<double> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var sum = .0;
            var count = 0L;
            foreach (var item in source)
            {
                sum += item;
                count += 1L;
            }
            if (count <= 0L)
            {
                return .0;
            }
            return (sum / count);
        }

        [Pure]
        public static decimal AverageOrDefault([NotNull, InstantHandle] this IEnumerable<decimal> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var sum = 0m;
            var count = 0L;
            foreach (var item in source)
            {
                sum += item;
                count += 1L;
            }
            if (count <= 0L)
            {
                return 0m;
            }
            return (sum / count);
        }

        [Pure]
        public static decimal AverageOrDefault<TSource>([NotNull, InstantHandle] this IEnumerable<TSource> source, [NotNull, InstantHandle] Func<TSource, decimal> selector)
        {
            return source.Select(selector).AverageOrDefault();
        }

        /// <summary>
        /// Сортирует коллекцию объектов по значению некоторого булевого свойства (или выражения, полученного на основе объекта) 
        /// по возрастанию - сначала <c>false</c>, затем <c>true</c>.
        /// </summary>
        /// <typeparam name="T">Тип сравниваемых объектов.</typeparam>
        /// <param name="source">Список сортируемых объектов.</param>
        /// <param name="boolPropertySelector">Делегат для получения значения свойства (или выражения) объекта, по которому производится сравнение.</param>
        /// <remarks>
        /// Работа метода напоминает <see cref="Enumerable.OrderBy{TSource,TKey}(System.Collections.Generic.IEnumerable{TSource},System.Func{TSource,TKey})"/> за исключением того, что сортируется только по булевому выражению, за время O(n).
        /// Наиболее оптимально передавать такой <paramref name="boolPropertySelector"/>, который чаще будет равен <c>false</c> на входном списке объектов.
        /// </remarks>
        [NotNull, Pure]
        public static IEnumerable<T> BinaryOrderBy<T>([NotNull] this IEnumerable<T> source, [NotNull] Func<T, bool> boolPropertySelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (boolPropertySelector == null)
                throw new ArgumentNullException(nameof(boolPropertySelector));

            return source.BinaryOrderByCore(boolPropertySelector);
        }

        [NotNull, Pure]
        private static IEnumerable<T> BinaryOrderByCore<T>([NotNull] this IEnumerable<T> source, [NotNull] Func<T, bool> boolPropertySelector)
        {
            var itemsWithTrue = new List<T>();
            foreach (var item in source)
            {
                if (!boolPropertySelector(item))
                    yield return item;
                else
                    itemsWithTrue.Add(item);
            }

            foreach (var item in itemsWithTrue)
                yield return item;
        }

        [Pure]
        public static TimeSpan Sum([NotNull, InstantHandle] this IEnumerable<TimeSpan> seq)
        {
            if (seq == null)
                throw new ArgumentNullException(nameof(seq));

            return seq.Aggregate(TimeSpan.Zero, (subTotal, item) => subTotal + item);
        }

        public static (List<T> matchingItems, List<T> nonMatchingItems) Split<T>([NotNull, InstantHandle] this IEnumerable<T> source, [NotNull, InstantHandle] Func<T, bool> filter)
        {
            source.Split(filter, out var matchingItems, out var nonMatchingItems);
            return (matchingItems, nonMatchingItems);
        }

        /// <summary>
        /// Делит элементы исходного списка по двум отдельным спискам по предикату.
        /// </summary>        
        /// <param name="source">Исходный список.</param>
        /// <param name="filter">Предикат, определяющий, в какой список попадёт элемент.</param>
        /// <param name="matchingItems">Элементы, удовлетворяющие предикату.</param>
        /// <param name="nonMatchingItems">Элементы, не удовлетворяющие предикату.</param>
        public static void Split<T>(
            [NotNull, InstantHandle] this IEnumerable<T> source, [NotNull, InstantHandle] Func<T, bool> filter,
            out List<T> matchingItems, out List<T> nonMatchingItems)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            matchingItems = new List<T>();
            nonMatchingItems = new List<T>();
            foreach (var item in source)
                if (filter(item))
                    matchingItems.Add(item);
                else
                    nonMatchingItems.Add(item);
        }

        /// <summary>
        /// Делит элементы исходного списка по двум отдельным спискам по типу.
        /// </summary>        
        /// <typeparam name="T">Тип элементов исходного списка.</typeparam>
        /// <typeparam name="U">Тип элементов, которые нужно выделить их исходного списка.</typeparam>
        /// <param name="source">Исходный список.</param>        
        /// <param name="matchingItems">Элементы, являющиеся заданным типом <typeparamref name="U"/>.</param>
        /// <param name="nonMatchingItems">Элементы, не являющиеся заданным типом <typeparamref name="U"/>.</param>
        public static void Split<T, U>([NotNull, InstantHandle] this IEnumerable<T> source, out List<U> matchingItems, out List<T> nonMatchingItems)
            where U : T
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            matchingItems = new List<U>();
            nonMatchingItems = new List<T>();
            foreach (var item in source)
                if (item is U)
                    matchingItems.Add((U)item);
                else
                    nonMatchingItems.Add(item);
        }

        /// <summary>
        /// Получает все положения элемента <paramref name="element"/> в последовательности <paramref name="sequence"/>.
        /// Равенство проверяется методом TSource.Equals.
        /// </summary>
        /// <typeparam name="T">Тип элементов в последовательности <paramref name="sequence"/>.</typeparam>
        /// <param name="sequence">Последовательность, по которой производится поиск.</param>
        /// <param name="element">Искомый элемент.</param>
        /// <returns>Все положения элемента в последовательности.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sequence"/><c> == null</c>.
        /// </exception>
        [NotNull, Pure]
        public static IEnumerable<int> IndexesOf<T>([NotNull] this IEnumerable<T> sequence, T element)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));

            return sequence.IndexesOfCore(element);
        }

        [NotNull, Pure]
        private static IEnumerable<int> IndexesOfCore<T>([NotNull] this IEnumerable<T> sequence, T element)
        {
            var comparer = EqualityComparer<T>.Default;

            var i = 0;
            foreach (var item in sequence)
            {
                if (comparer.Equals(element, item))
                    yield return i;

                i++;
            }
        }

        [NotNull]
        public static HashSet<TResult> ToHashSet<TSource, TResult>(
            [NotNull, InstantHandle] this IEnumerable<TSource> source, [NotNull, InstantHandle] Func<TSource, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Select(resultSelector).ToHashSet();
        }

        [NotNull]
        public static HashSet<TResult> ToHashSet<TSource, TResult>(
            [NotNull, InstantHandle] this IEnumerable<TSource> source, [NotNull, InstantHandle] Func<TSource, TResult> resultSelector, [CanBeNull] IEqualityComparer<TResult> equalityComparer)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Select(resultSelector).ToHashSet(equalityComparer);
        }

        [NotNull]
        public static TResult[] ToArray<TSource, TResult>([NotNull, InstantHandle] this IEnumerable<TSource> source, [NotNull, InstantHandle] Func<TSource, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));

            return source.Select(resultSelector).ToArray();
        }

        [Pure]
        public static bool SetEqual<T>([NotNull, InstantHandle] this IEnumerable<T> first, [NotNull, InstantHandle] IEnumerable<T> second)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));

            return new HashSet<T>(first).SetEquals(second);
        }

        [NotNull, Pure]
        public static IEnumerable<T> Union<T, U>([NotNull] this IEnumerable<T> first, [NotNull] IEnumerable<T> second, [NotNull] Func<T, U> getKey)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (getKey == null)
                throw new ArgumentNullException(nameof(getKey));

            return first.Union(second, new EqualityComparerByKey<T, U>(getKey));
        }

        [NotNull, Pure]
        public static IEnumerable<T> Except<T, U>([NotNull] this IEnumerable<T> first, [NotNull] IEnumerable<T> second, [NotNull] Func<T, U> getKey)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (getKey == null)
                throw new ArgumentNullException(nameof(getKey));

            return first.Except(second, new EqualityComparerByKey<T, U>(getKey));
        }

        [NotNull, Pure]
        public static IEnumerable<T> Intersect<T, U>([NotNull] this IEnumerable<T> first, [NotNull] IEnumerable<T> second, [NotNull] Func<T, U> getKey)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (getKey == null)
                throw new ArgumentNullException(nameof(getKey));

            return first.Intersect(second, new EqualityComparerByKey<T, U>(getKey));
        }

        public static void DisposeItems<T>([NotNull, InstantHandle] this IEnumerable<T> items)
            where T : IDisposable
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            items.ForEach(item => item.Dispose());
        }

        /// <summary>
        /// <para>Делает поверхностную копию заданного фрагмента списка.</para>
        /// <para>Обертка вокруг одноименного метода класса <see cref="System.Collections.Generic.List{T}" />.</para>
        /// </summary>
        /// <param name="items">Список</param>
        /// <param name="index">Индекс начала диапазона</param>
        /// <param name="count">Количество элементов диапазона</param>
        [NotNull]
        public static List<T> GetRange<T>([NotNull, InstantHandle] this IEnumerable<T> items, int index, int count)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            return items.ToList().GetRange(index, count);
        }

        /// <summary>
        /// Частично сортирует по возрастанию <paramref name="source"/> по ключу <paramref name="keySelector"/> и
        /// возвращает первые <paramref name="count"/> отсортированных элементов.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="keySelector">Функция получения ключа сортировки из элементов последовательности.</param>
        /// <param name="count">Количество отсортированных элементов, которые надо вернуть.</param>
        [NotNull, Pure]
        public static IEnumerable<TItem> PartialOrderBy<TItem, TKey>(
            [NotNull] this IEnumerable<TItem> source,
            [NotNull] Func<TItem, TKey> keySelector,
            int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return new PartialOrderedEnumerable<TItem, TKey>(source, keySelector, null, false, count);
        }

        /// <summary>
        /// Частично сортирует по возрастанию <paramref name="source"/> по ключу <paramref name="keySelector"/>
        /// с помощью <paramref name="comparer"/> и возвращает первые <paramref name="count"/> отсортированных элементов.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="keySelector">Функция получения ключа сортировки из элементов последовательности.</param>
        /// <param name="comparer">Способ сравнения ключей, по которым выполняется сортировка.</param>
        /// <param name="count">Количество отсортированных элементов, которые надо вернуть.</param>
        [NotNull, Pure]
        public static IEnumerable<TItem> PartialOrderBy<TItem, TKey>(
            [NotNull] this IEnumerable<TItem> source,
            [NotNull] Func<TItem, TKey> keySelector,
            [NotNull] IComparer<TKey> comparer,
            int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new PartialOrderedEnumerable<TItem, TKey>(source, keySelector, comparer, false, count);
        }

        /// <summary>
        /// Частично сортирует по убыванию <paramref name="source"/> по ключу <paramref name="keySelector"/> и
        /// возвращает первые <paramref name="count"/> отсортированных элементов.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="keySelector">Функция получения ключа сортировки из элементов последовательности.</param>
        /// <param name="count">Количество отсортированных элементов, которые надо вернуть.</param>
        [NotNull, Pure]
        public static IEnumerable<TItem> PartialOrderByDescending<TItem, TKey>(
            [NotNull] this IEnumerable<TItem> source,
            [NotNull] Func<TItem, TKey> keySelector,
            int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            return new PartialOrderedEnumerable<TItem, TKey>(source, keySelector, null, true, count);
        }

        /// <summary>
        /// Частично сортирует по убыванию <paramref name="source"/> по ключу <paramref name="keySelector"/>
        /// с помощью <paramref name="comparer"/> и возвращает первые <paramref name="count"/> отсортированных элементов.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="keySelector">Функция получения ключа сортировки из элементов последовательности.</param>
        /// <param name="comparer">Способ сравнения ключей, по которым выполняется сортировка.</param>
        /// <param name="count">Количество отсортированных элементов, которые надо вернуть.</param>
        [NotNull, Pure]
        public static IEnumerable<TItem> PartialOrderByDescending<TItem, TKey>(
            [NotNull] this IEnumerable<TItem> source,
            [NotNull] Func<TItem, TKey> keySelector,
            [NotNull] IComparer<TKey> comparer,
            int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return new PartialOrderedEnumerable<TItem, TKey>(source, keySelector, comparer, true, count);
        }

        private sealed class PartialOrderedEnumerable<TItem, TKey> : IEnumerable<TItem>
        {
            #region Fields
            [NotNull]
            private readonly IEnumerable<TItem> source;
            [NotNull]
            private readonly Func<TItem, TKey> keySelector;
            [NotNull]
            private readonly IComparer<TKey> comparer;
            private readonly bool descending;
            private readonly int count;
            #endregion

            #region Ctor
            internal PartialOrderedEnumerable(
                [NotNull] IEnumerable<TItem> source,
                [NotNull] Func<TItem, TKey> keySelector,
                [CanBeNull] IComparer<TKey> comparer,
                bool descending, int count)
            {
                Debug.Assert(source != null);
                Debug.Assert(keySelector != null);

                this.source = source;
                this.keySelector = keySelector;
                this.comparer = comparer ?? Comparer<TKey>.Default;
                this.descending = descending;
                this.count = count;
            }
            #endregion

            #region Methods
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            [NotNull]
            public IEnumerator<TItem> GetEnumerator()
            {
                if (count <= 0)
                    yield break;

                Pair<TKey, TItem>[] keysWithItems;

                var collection = source as ICollection<TItem>;
                if (collection != null)
                {
                    keysWithItems = new Pair<TKey, TItem>[collection.Count];

                    var index = 0;
                    foreach (var item in collection)
                    {
                        keysWithItems[index] = Tuples.Pair(keySelector(item), item);
                        index++;
                    }
                }
                else
                {
                    keysWithItems = source.Select(item => Tuples.Pair(keySelector(item), item)).ToArray();
                }

                if (keysWithItems.Length == 0)
                    yield break;

                PartialSort(keysWithItems, 0, keysWithItems.Length - 1, count);

                var upper = Math.Min(count, keysWithItems.Length);
                for (var i = 0; i < upper; i++)
                    yield return keysWithItems[i].Second;
            }

            private int CompareKeys(TKey left, TKey right)
            {
                return descending
                    ? comparer.Compare(right, left)
                    : comparer.Compare(left, right);
            }

            private void PartialSort(Pair<TKey, TItem>[] keysWithItems, int left, int right, int itemsToSort)
            {
                if (itemsToSort <= 0)
                    return;

                var i = left;
                var j = right;
                var pivot = keysWithItems[(left + right) / 2].First;

                while (i <= j)
                {
                    while (CompareKeys(keysWithItems[i].First, pivot) < 0)
                        i++;

                    while (CompareKeys(keysWithItems[j].First, pivot) > 0)
                        j--;

                    if (i <= j)
                    {
                        if (i < j)
                        {
                            var tmp = keysWithItems[i];
                            keysWithItems[i] = keysWithItems[j];
                            keysWithItems[j] = tmp;
                        }

                        i++;
                        j--;
                    }
                }

                if (left < j)
                    PartialSort(keysWithItems, left, j, itemsToSort);

                if (i < right)
                    PartialSort(keysWithItems, i, right, left + itemsToSort - i);
            }
            #endregion
        }

        /// <summary>
        /// Сливает две последовательности <paramref name="left"/> и <paramref name="right"/>,
        /// отсортированные по ключу <paramref name="keySelector"/>, в одну отсортированную последовательность.
        /// Входные последовательности должны быть отсортированы.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
        /// <param name="left">Первая исходная последовательность.</param>
        /// <param name="right">Вторая исходная последовательность.</param>
        /// <param name="keySelector">Функция получения ключа сортировки из элементов последовательности.</param>
        [NotNull, Pure]
        public static IEnumerable<TItem> MergeSortedWith<TItem, TKey>(
            [NotNull] this IEnumerable<TItem> left,
            [NotNull] IEnumerable<TItem> right,
            [NotNull] Func<TItem, TKey> keySelector)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return Algorithms.MergeSorted(new ByKeyComparer<TItem, TKey>(keySelector), left, right);
        }

        /// <summary>
        /// Сливает две последовательности <paramref name="left"/> и <paramref name="right"/>,
        /// отсортированные по ключу <paramref name="keySelector"/> с помощью <paramref name="comparer"/>,
        /// в одну отсортированную последовательность.
        /// Входные последовательности должны быть отсортированы.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
        /// <param name="left">Первая исходная последовательность.</param>
        /// <param name="right">Вторая исходная последовательность.</param>
        /// <param name="keySelector">Функция получения ключа сортировки из элементов последовательности.</param>
        /// <param name="comparer">Способ сравнения ключей, по которым отсортированы входные последовательности.</param>
        [NotNull, Pure]
        public static IEnumerable<TItem> MergeSortedWith<TItem, TKey>(
            [NotNull] this IEnumerable<TItem> left,
            [NotNull] IEnumerable<TItem> right,
            [NotNull] Func<TItem, TKey> keySelector,
            [NotNull] IComparer<TKey> comparer)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Algorithms.MergeSorted(new ByKeyComparer<TItem, TKey>(keySelector, comparer), left, right);
        }

        /// <summary>
        /// Сливает две последовательности <paramref name="left"/> и <paramref name="right"/>,
        /// отсортированные по ключу <paramref name="keySelector"/> по убыванию, в одну отсортированную по убыванию последовательность.
        /// Входные последовательности должны быть отсортированы по убыванию.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
        /// <param name="left">Первая исходная последовательность.</param>
        /// <param name="right">Вторая исходная последовательность.</param>
        /// <param name="keySelector">Функция получения ключа сортировки из элементов последовательности.</param>
        [NotNull, Pure]
        public static IEnumerable<TItem> MergeSortedDescendingWith<TItem, TKey>(
            [NotNull] this IEnumerable<TItem> left,
            [NotNull] IEnumerable<TItem> right,
            [NotNull] Func<TItem, TKey> keySelector)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            return Algorithms.MergeSorted(new ReverseComparer<TItem>(new ByKeyComparer<TItem, TKey>(keySelector)), left, right);
        }

        /// <summary>
        /// Сливает две последовательности <paramref name="left"/> и <paramref name="right"/>,
        /// отсортированные по ключу <paramref name="keySelector"/> по убыванию с помощью <paramref name="comparer"/>,
        /// в одну отсортированную по убыванию последовательность.
        /// Входные последовательности должны быть отсортированы по убыванию.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов исходной последовательности.</typeparam>
        /// <typeparam name="TKey">Тип ключа сортировки.</typeparam>
        /// <param name="left">Первая исходная последовательность.</param>
        /// <param name="right">Вторая исходная последовательность.</param>
        /// <param name="keySelector">Функция получения ключа сортировки из элементов последовательности.</param>
        /// <param name="comparer"></param>
        [NotNull, Pure]
        public static IEnumerable<TItem> MergeSortedDescendingWith<TItem, TKey>(
            [NotNull] this IEnumerable<TItem> left,
            [NotNull] IEnumerable<TItem> right,
            [NotNull] Func<TItem, TKey> keySelector,
            [NotNull] IComparer<TKey> comparer)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            return Algorithms.MergeSorted(new ReverseComparer<TItem>(new ByKeyComparer<TItem, TKey>(keySelector, comparer)), left, right);
        }

        public static IEnumerable<Pair<Maybe<TOuter>, Maybe<TInner>>> FullOuterJoin<TOuter, TInner, TKey>(
            [NotNull] this IEnumerable<TOuter> outer,
            [NotNull] IEnumerable<TInner> inner,
            [NotNull] Func<TOuter, TKey> outerKeySelector,
            [NotNull] Func<TInner, TKey> innerKeySelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));

            return outer.FullOuterJoinCore(inner, outerKeySelector, innerKeySelector);
        }

        private static IEnumerable<Pair<Maybe<TOuter>, Maybe<TInner>>> FullOuterJoinCore<TOuter, TInner, TKey>(
            [NotNull] this IEnumerable<TOuter> outer,
            [NotNull] IEnumerable<TInner> inner,
            [NotNull] Func<TOuter, TKey> outerKeySelector,
            [NotNull] Func<TInner, TKey> innerKeySelector)
        {
            var groupedInner = inner.ToLookup(innerKeySelector);

            var joinedKeys = new HashSet<TKey>();

            foreach (var outerItem in outer)
            {
                var outerKey = outerKeySelector(outerItem);

                if (groupedInner.Contains(outerKey))
                {
                    joinedKeys.Add(outerKey);

                    foreach (var innerItem in groupedInner[outerKey])
                    {
                        yield return new Pair<Maybe<TOuter>, Maybe<TInner>>(outerItem, innerItem);
                    }
                }
                else
                {
                    yield return new Pair<Maybe<TOuter>, Maybe<TInner>>(outerItem, Maybe<TInner>.Empty);
                }
            }

            foreach (var group in groupedInner)
            {
                if (joinedKeys.Contains(group.Key))
                    continue;

                foreach (var innerItem in group)
                {
                    yield return new Pair<Maybe<TOuter>, Maybe<TInner>>(Maybe<TOuter>.Empty, innerItem);
                }
            }
        }

        public static IEnumerable<Pair<TOuter, Maybe<TInner>>> LeftOuterJoin<TOuter, TInner, TKey>(
            [NotNull] this IEnumerable<TOuter> outer,
            [NotNull] IEnumerable<TInner> inner,
            [NotNull] Func<TOuter, TKey> outerKeySelector,
            [NotNull] Func<TInner, TKey> innerKeySelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));

            return outer.LeftOuterJoinCore(inner, outerKeySelector, innerKeySelector);
        }

        private static IEnumerable<Pair<TOuter, Maybe<TInner>>> LeftOuterJoinCore<TOuter, TInner, TKey>(
            [NotNull] this IEnumerable<TOuter> outer,
            [NotNull] IEnumerable<TInner> inner,
            [NotNull] Func<TOuter, TKey> outerKeySelector,
            [NotNull] Func<TInner, TKey> innerKeySelector)
        {
            var groupedInner = inner.ToLookup(innerKeySelector);

            foreach (var outerItem in outer)
            {
                var outerKey = outerKeySelector(outerItem);

                if (groupedInner.Contains(outerKey))
                {
                    foreach (var innerItem in groupedInner[outerKey])
                    {
                        yield return new Pair<TOuter, Maybe<TInner>>(outerItem, innerItem);
                    }
                }
                else
                {
                    yield return new Pair<TOuter, Maybe<TInner>>(outerItem, Maybe<TInner>.Empty);
                }
            }
        }

        public static IEnumerable<Pair<Maybe<TOuter>, TInner>> RightOuterJoin<TOuter, TInner, TKey>(
            [NotNull] this IEnumerable<TOuter> outer,
            [NotNull] IEnumerable<TInner> inner,
            [NotNull] Func<TOuter, TKey> outerKeySelector,
            [NotNull] Func<TInner, TKey> innerKeySelector)
        {
            if (outer == null)
                throw new ArgumentNullException(nameof(outer));
            if (inner == null)
                throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(innerKeySelector));

            return outer.RightOuterJoinCore(inner, outerKeySelector, innerKeySelector);
        }

        private static IEnumerable<Pair<Maybe<TOuter>, TInner>> RightOuterJoinCore<TOuter, TInner, TKey>(
            [NotNull] this IEnumerable<TOuter> outer,
            [NotNull] IEnumerable<TInner> inner,
            [NotNull] Func<TOuter, TKey> outerKeySelector,
            [NotNull] Func<TInner, TKey> innerKeySelector)
        {
            var groupedInner = inner.ToLookup(innerKeySelector);

            var joinedKeys = new HashSet<TKey>();

            foreach (var outerItem in outer)
            {
                var outerKey = outerKeySelector(outerItem);

                if (groupedInner.Contains(outerKey))
                {
                    joinedKeys.Add(outerKey);

                    foreach (var innerItem in groupedInner[outerKey])
                    {
                        yield return new Pair<Maybe<TOuter>, TInner>(outerItem, innerItem);
                    }
                }
            }

            foreach (var group in groupedInner)
            {
                if (joinedKeys.Contains(group.Key))
                    continue;

                foreach (var innerItem in group)
                {
                    yield return new Pair<Maybe<TOuter>, TInner>(Maybe<TOuter>.Empty, innerItem);
                }
            }
        }

        /// <summary>
        /// Возвращает исходную коллекцию <see cref="source"/> отсортированной в том же порядке, что и эталонная коллекция <see cref="template"/>.
        /// Те элементы <see cref="source"/>, которые есть в обеих коллекциях, на выходе идут в том порядке, в котором они встречаются в <see cref="template"/>.
        /// Те элементы <see cref="source"/>, которых нет в <see cref="template"/>, идут в конце в исходном порядке.
        /// </summary>
        [NotNull, Pure]
        public static IEnumerable<TSource> OrderByTemplate<TSource, TTemplate, TKey>([NotNull] this IEnumerable<TSource> source, [NotNull] IEnumerable<TTemplate> template,
            [NotNull] Func<TSource, TKey> sourceSelector, [NotNull] Func<TTemplate, TKey> templateSelector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (template == null)
                throw new ArgumentNullException(nameof(template));
            if (sourceSelector == null)
                throw new ArgumentNullException(nameof(sourceSelector));
            if (templateSelector == null)
                throw new ArgumentNullException(nameof(templateSelector));

            return template
                .RightOuterJoin(source, templateSelector, sourceSelector)
                .Select(x => x.Second);
        }

        [NotNull]
        public static IEnumerable<IEnumerable<T>> Paginate<T>(this IEnumerable<T> sequence, int itemsOnPage)
        {
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));
            if (itemsOnPage <= 0)
                throw new ArgumentOutOfRangeException(nameof(itemsOnPage));

            return sequence
                .Select((p, i) => new { Item = p, Page = i / itemsOnPage })
                .GroupBy(p => p.Page, p => p.Item);
        }

        /// <summary>
        /// Обрезает последовательность, возвращая не более <paramref name="count"/> элементов исходной последовательности.
        /// Если в исходной последовательности больше элементов, то все оставшиеся элементы заменяются одним <paramref name="trailingElement"/>.
        /// </summary>
        /// <remarks>
        /// Работает почти как <see cref="Enumerable.Take{TSource}"/> + <see cref="ContinueWith{TSource}(IEnumerable{TSource},TSource)"/>,
        /// но дополнительный элемент добавляется не всегда, а лишь при отбрасывании хвоста исходной последовательности.
        /// <br />
        /// Другой аналог — <see cref="StringExtensions.TrimWithEllipsis"/>.
        /// </remarks>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="count">Количество элементов исходной поледовательности, которые необходимо вернуть.</param>
        /// <param name="trailingElement">Дополнительный замыкающий элемент, заменяющий собой отрезанный хвост последовательности. Игноруется, если количество элементов в исходной последовательности не превышает <paramref name="count"/></param>.
        /// <returns></returns>
        [NotNull, Pure]
        public static IEnumerable<T> Trim<T>([NotNull] this IEnumerable<T> source, int count, T trailingElement)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be non-negative.");

            foreach (var item in source)
            {
                if (count == 0)
                {
                    yield return trailingElement;
                    yield break;
                }

                yield return item;
                count--;
            }
        }
    }
}
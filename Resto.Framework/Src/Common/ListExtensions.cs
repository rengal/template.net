using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Resto.Framework.Attributes.JetBrains;
using Resto.Framework.Src.Common;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Класс-расширитель для <see cref="IList{T}"/> и <see cref="List{T}"/>.
    /// </summary>
    public static class ListExtensions
    {
        public static bool CheckIndex<TSource>([NotNull]this IList<TSource> source, int index)
        {
            return index >= 0 && source.Count > index;
        }

        public static IEnumerable<TSource> TakeFrom<TSource>([NotNull]this IList<TSource> source, int index)
        {
            if (index < 0 || index >= source.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            for (var i = index; i < source.Count; i++)
                yield return source[i];
        }

        public static IEnumerable<TSource> TakeFromReverse<TSource>([NotNull]this IList<TSource> source, int index)
        {
            if (index < 0 || index >= source.Count)
                throw new ArgumentOutOfRangeException(nameof(index));
            for (var i = index; i >= 0; i--)
                yield return source[i];
        }

        /// <summary>
        /// Сортирует список объектов по значению некоторого свойства (или выражения, полученного на основе объекта).
        /// </summary>
        /// <typeparam name="T">Тип сравниваемых объектов.</typeparam>
        /// <typeparam name="U">Тип свойства (или выражения), по которому сравниваются объекты списка.</typeparam>
        /// <param name="list">Список сортируемых объектов.</param>
        /// <param name="propertySelector">Делегат для получения значения свойства (или выражения) объекта, по которому производится сравнение.</param>
        /// <remarks>
        /// Работа метода напоминает <see cref="Enumerable.OrderBy{TSource,TKey}(System.Collections.Generic.IEnumerable{TSource},System.Func{TSource,TKey})"/>
        /// за исключением того, что сортируется непосредственно сам список.
        /// Это удобно при использовании <c>readonly</c> списков.
        /// Для сортировки значений используется <see cref="ByKeyComparer{T, U}"/>.
        /// </remarks>
        public static void Sort<T, U>([NotNull]this List<T> list, [NotNull]Func<T, U> propertySelector)
            where T : class
            where U : IComparable<U>
        {
            list.Sort(new ByKeyComparer<T, U>(propertySelector));
        }

        public static void RemoveAll<T>(this ICollection<T> list, IReadOnlyCollection<T> itemsToRemove)
        {
            foreach (var item in itemsToRemove)
                list.Remove(item);
        }

        public static void RemoveAll<T>([NotNull] this IList<T> list, [NotNull, InstantHandle] Func<T, bool> filter)
        {
            var index1 = 0;

            while (index1 < list.Count && !filter(list[index1]))
                ++index1;

            if (index1 >= list.Count)
                return;

            var index2 = index1 + 1;

            while (index2 < list.Count)
            {
                while (index2 < list.Count && filter(list[index2]))
                    ++index2;

                if (index2 < list.Count)
                    list[index1++] = list[index2++];
            }

            //Инвариант: index2 = list.Count

            while (--index2 >= index1)
                list.RemoveAt(index2);
        }

        /// <summary>
        /// Альтернативная реализация специально для BindingList-а, чтобы формировались правильные event-ы модификации
        /// </summary>
        public static void RemoveAll<T>(this BindingList<T> list, Func<T, bool> filter)
        {
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if (filter(list[i]))
                {
                    list.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Делит элементы исходного списка на указанное количество примерно равных по размеру групп (последняя группа может оказаться больше остальных).
        /// </summary>
        /// <param name="items">Исходный список</param>
        /// <param name="chunksCount">Требуемое количество групп.</param>
        [NotNull]
        public static IEnumerable<List<T>> Split<T>([NotNull] this IReadOnlyCollection<T> items, int chunksCount)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));
            if (chunksCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(chunksCount), chunksCount, "Chunks count must be greater than zero.");
            if (chunksCount > items.Count)
                throw new ArgumentOutOfRangeException(nameof(chunksCount), chunksCount, "Chunks count must be less or equal to items count.");

            var chunkSize = items.Count / chunksCount;
            return Enumerable.Range(0, chunksCount)
                .Select(i => items.GetRange(i * chunkSize, i < chunksCount - 1 ? chunkSize : items.Count - i * chunkSize));
        }

        public static IEnumerable<IEnumerable<T>> SplitIntoBatches<T>(this List<T> list, int size)
        {
            for (var i = 0; i < (float)list.Count / size; i++)
            {
                yield return list.Skip(i * size).Take(size);
            }
        }

        /// <summary>
        /// <para>Возвращает последний из элементов списка, удовлетворяющих заданному условию или default(T) если таких элементов в последовательности нет.</para>
        /// <para>Аналог одноименного метода класса <see cref="System.Collections.Generic.List{T}" />
        /// (код сделан на основе полученного с помощью dotPeek и не имеет алгоритмических отклонений от оригинала)</para>
        /// </summary>
        /// <typeparam name="T">Тип элементов списка</typeparam>
        /// <param name="items">Список</param>
        /// <param name="match">Предикат</param>
        public static T FindLast<T>([NotNull] this IList<T> items, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            for (var index = items.Count - 1; index >= 0; --index)
            {
                var item = items[index];
                if (match(item))
                {
                    return item;
                }
            }

            return default(T);
        }

        /// <summary>
        /// <para>Возвращает индекс последнего вхождения в список элемента, удовлетворяющего переданному предикату</para>
        /// <para>Аналог одноименного метода класса <see cref="System.Collections.Generic.List{T}" />
        /// (код сделан на основе полученного с помощью dotPeek и не имеет алгоритмических отклонений от оригинала)</para>
        /// </summary>
        /// <returns>
        /// Если элемент найден - индекс, в противном случае –1.
        /// </returns>
        /// <param name="items">Список, в котором осуществляется поиск</param>
        /// <param name="match">Предикат</param>
        public static int FindLastIndex<T>([NotNull] this IList<T> items, Predicate<T> match)
        {
            return FindLastIndex(items, items.Count - 1, items.Count, match);
        }

        /// <summary>
        /// <para>Возвращает индекс последнего вхождения в список элемента, удовлетворяющего переданному предикату</para>
        /// <para>Аналог одноименного метода класса <see cref="System.Collections.Generic.List{T}" /> 
        /// (код сделан на основе полученного с помощью dotPeek и не имеет алгоритмических отклонений от оригинала)</para>
        /// </summary>
        /// <returns>
        /// Если элемент найден - индекс, в противном случае –1.
        /// </returns>
        /// <param name="items">Список, в котором осуществляется поиск</param>
        /// <param name="startIndex">Индекс, с которого нужно производить поиск (в обратную сторону)</param>
        /// <param name="match">Предикат</param>
        public static int FindLastIndex<T>([NotNull] this IList<T> items, int startIndex, Predicate<T> match)
        {
            return FindLastIndex(items, startIndex, startIndex + 1, match);
        }

        /// <summary>
        /// <para>Возвращает индекс последнего вхождения в список элемента, удовлетворяющего переданному предикату</para>
        /// <para>Аналог одноименного метода класса <see cref="System.Collections.Generic.List{T}" />
        /// (код сделан на основе полученного с помощью dotPeek и не имеет алгоритмических отклонений от оригинала)</para>
        /// </summary>
        /// <returns>
        /// Если элемент найден - индекс, в противном случае –1.
        /// </returns>
        /// <param name="items">Список, в котором осуществляется поиск</param>
        /// <param name="startIndex">Индекс, с которого нужно производить поиск (в обратную сторону)</param>
        /// <param name="count">Количество элементов фрагмента, на котором будет осуществляться поиск</param>
        /// <param name="match">Предикат</param>
        public static int FindLastIndex<T>([NotNull] this IList<T> items, int startIndex, int count, Predicate<T> match)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }
            if (items.Count == 0)
            {
                if (startIndex != -1)
                {
                    throw new ArgumentOutOfRangeException(nameof(startIndex));
                }
            }
            else if ((uint)startIndex >= (uint)items.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
            }
            if (count < 0 || startIndex - count + 1 < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            var num = startIndex - count;
            for (var index = startIndex; index > num; --index)
            {
                if (match(items[index]))
                    return index;
            }
            return -1;
        }

        /// <summary>
        /// <para>Вставляет коллекцию элементов в заданное место списка.</para>
        /// <para>По сути обертка вокруг <see cref="System.Collections.Generic.List{T}.InsertRange" />.</para>
        /// <para>Для больших наборов данных - применять с осторожностью, т.к. метод будет работать медленнее и потребует больше памяти, чем оригинальный.</para>
        /// </summary>
        /// <typeparam name="T">Тип элементов списка</typeparam>
        /// <param name="targetList">Список, в который осуществляется вставка</param>
        /// <param name="index">Индекс, с которого начинается вставка</param>
        /// <param name="range">Вставляемый список</param>
        public static void InsertRange<T>([NotNull] this IList<T> targetList, int index, [NotNull] ICollection<T> range)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var tempList = new List<T>(targetList.Count + range.Count);
            tempList.AddRange(targetList);
            tempList.InsertRange(index, range);

            targetList.Clear();
            targetList.AddRange(tempList);
        }
    }
}
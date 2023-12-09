using System;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public static class CollectionHelper
    {
        /// <summary>
        /// Добавить элементы к коллекции.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов в коллекции.</typeparam>
        /// <typeparam name="TAddedItem">Тип добавляемых элементов - подтип типа элементов коллекции</typeparam>
        /// <param name="list">Изменяемая коллекция.</param>
        /// <param name="items">Добавляемые элементы.</param>
        public static void AddRange<TItem, TAddedItem>([NotNull] this ICollection<TItem> list, [NotNull] IEnumerable<TAddedItem> items)
            where TAddedItem : TItem
        {
            foreach (var item in items)
                list.Add(item);
        }

        /// <summary>
        /// Заменить содержимое коллекции перечислением элементов.
        /// </summary>
        /// <typeparam name="TItem">Тип элементов в коллекции.</typeparam>
        /// <typeparam name="TAddedItem">Тип добавляемых элементов - подтип типа элементов коллекции</typeparam>
        /// <param name="list">Изменяемая коллекция.</param>
        /// <param name="items">Элементы.</param>
        public static void Set<TItem, TAddedItem>([NotNull] this ICollection<TItem> list, [NotNull] IEnumerable<TAddedItem> items)
            where TAddedItem : TItem
        {
            if (ReferenceEquals(list, items))
                return;

            list.Clear();
            list.AddRange(items);
        }

        /// <summary>
        /// Удалить элемент из коллекции и вызвать у него Dispose.
        /// </summary>
        /// <typeparam name="T">Тип элементов в коллекции.</typeparam>
        /// <param name="list">Изменяемая коллекция.</param>
        /// <param name="item">Удаляемый элемент.</param>
        public static void RemoveAndDispose<T>([NotNull] this ICollection<T> list, T item)
            where T : IDisposable
        {
            list.Remove(item);
            item.Dispose();
        }

        /// <summary>
        /// Удалить перечисление элементов из коллекции.
        /// </summary>
        /// <typeparam name="T">Тип элементов в коллекции.</typeparam>
        /// <param name="list">Изменяемая коллекция.</param>
        /// <param name="items">Удаляемые элементы.</param>
        public static void Remove<T>([NotNull] this ICollection<T> list, [NotNull] IEnumerable<T> items)
        {
            foreach (var item in items)
                list.Remove(item);
        }

        /// <summary>
        /// Удалить перечисление элементов из коллекции и вызвать у них Dispose.
        /// </summary>
        /// <typeparam name="T">Тип элементов в коллекции.</typeparam>
        /// <param name="list">Изменяемая коллекция.</param>
        /// <param name="items">Удаляемые элементы.</param>
        public static void RemoveAndDispose<T>([NotNull] this ICollection<T> list, [NotNull] IEnumerable<T> items)
            where T : IDisposable
        {
            foreach (var item in items)
            {
                list.RemoveAndDispose(item);
            }
        }

        /// <summary>
        /// Вызвать у всех эллементов коллекции Dispose и очистить коллекцию.
        /// </summary>
        /// <typeparam name="T">Тип элементов в коллекции.</typeparam>
        /// <param name="list">Изменяемая коллекция.</param>
        public static void DisposeItemsAndClear<T>([NotNull] this ICollection<T> list)
            where T : IDisposable
        {
            list.DisposeItems();
            list.Clear();
        }

        /// <summary>
        /// Аналог имплементации java.utils.AbstractMap#equals
        /// </summary>
        public static bool Equals<TKey, TValue>([CanBeNull] IDictionary<TKey, TValue> first,
            [CanBeNull] IDictionary<TKey, TValue> second)
        {
            if (first is null && second is null)
            {
                return true;
            }

            if (first is null || second is null)
            {
                return false;
            }

            if (ReferenceEquals(first, second))
            {
                return true;
            }

            if (first.Count != second.Count)
            {
                return false;
            }

            foreach (var key in first.Keys)
            {
                if (!second.ContainsKey(key))
                {
                    return false;
                }

                if (!Equals(first[key], second[key]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Аналог имплементации java.utils.AbstractMap#hashCode
        /// </summary>
        public static int GetHashCode<TKey, TValue>([CanBeNull] IDictionary<TKey, TValue> dict)
        {
            if (dict is null)
            {
                return 0;
            }

            var hash = 0;
            foreach (var key in dict.Keys)
            {
                var value = dict[key];
                var hashKey = key.GetHashCode();
                var hashValue = value == null ? 0 : value.GetHashCode();
                hash += hashKey ^ hashValue;
            }

            return hash;
        }
    }
}
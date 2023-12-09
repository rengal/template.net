using System;
using System.Collections;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;
using System.Linq;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Набор элементов с подсчётом количества каждого элемента.
    /// Реализация <see cref="ICollection{T}"/> представляет коллекцию элементов без дубликатов.
    /// </summary>
    /// <typeparam name="T">Тип элементов</typeparam>
    [Serializable]
    public sealed class CountingHashSet<T> : ICollection<T>
    {
        #region Fields
        private readonly Dictionary<T, int> dict;
        #endregion

        #region Ctor
        /// <summary>
        /// Создаёт пустой набор элементов
        /// </summary>
        /// <param name="comparer">Comparer для сравнения значений ключей. Если <c>null</c>, то используется <c>EqualityComparer&lt;T&gt;.Default</c></param>
        public CountingHashSet([CanBeNull] IEqualityComparer<T> comparer = null)
        {
            dict = new Dictionary<T, int>(comparer);
        }

        /// <summary>
        /// Создаёт набор элементов и заполняет его элементами из <paramref name="items"/>
        /// </summary>
        /// <param name="items">Элементы, которыми заполняется создаваемый набор</param>
        /// <param name="comparer">Comparer для сравнения значений ключей. Если <c>null</c>, то используется <c>EqualityComparer&lt;T&gt;.Default</c></param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="items"/><c> == null</c>
        /// </exception>
        public CountingHashSet([NotNull] IEnumerable<T> items, [CanBeNull] IEqualityComparer<T> comparer = null)
        {
            if (items == null)
                throw new ArgumentNullException(nameof(items));

            dict = items.GroupBy(i => i, comparer).ToDictionary(group => group.Key, group => group.Count(), comparer);
        }

        #endregion

        #region Methods
        /// <summary>
        /// Добавляет в набор элемент <paramref name="item"/> в количестве 1
        /// </summary>
        /// <param name="item">Добавляемый элемент</param>
        /// <returns>
        /// Количество элемента <paramref name="item"/> в наборе после добавления
        /// </returns>
        public int AddOne(T item)
        {
            return Add(item, 1);
        }

        /// <summary>
        /// Добавляет в набор элемент <paramref name="item"/> в количестве <paramref name="count"/>
        /// </summary>
        /// <param name="item">Добавляемый элемент</param>
        /// <param name="count">Количество  (больше 0)</param>
        /// <returns>
        /// Количество элемента <paramref name="item"/> в наборе после добавления
        /// </returns>
        public int Add(T item, int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be greater than zero");

            if (dict.TryGetValue(item, out var currentCount))
                dict[item] += count;
            else
                dict.Add(item, count);

            return currentCount + count;
        }

        /// <summary>
        /// Уменьшает количество элемента <paramref name="item"/> в наборе на 1.
        /// Если количество элемента становится равным 0, элемент из набора удаляется.
        /// </summary>
        /// <param name="item">Элемент, количество которого уменьшается</param>
        /// <returns>
        /// Количество элемента <paramref name="item"/> в наборе после уменьшения
        /// </returns>
        public int RemoveOne(T item)
        {
            return Remove(item, 1);
        }

        /// <summary>
        /// Уменьшает количество элемента <paramref name="item"/> в наборе на <paramref name="count"/>.
        /// Если количество элемента становится равным 0, элемент из набора удаляется.
        /// </summary>
        /// <param name="item">Элемент, количество которого уменьшается</param>
        /// <param name="count">Количество (больше 0 и не больше текущего количества элемента <paramref name="item"/></param>
        /// <returns>
        /// Количество элемента <paramref name="item"/> в наборе после уменьшения
        /// </returns>
        public int Remove(T item, int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be greater than zero");

            if (!dict.TryGetValue(item, out var currentCount))
                throw new ArgumentException("Item 'item' doesn't exist in set", nameof(item));

            if (currentCount < count)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be less or equal than actual item count");

            if (currentCount == count)
                dict.Remove(item);
            else
                dict[item] -= count;

            return currentCount - count;
        }

        /// <summary>
        /// Возвращает количество элемента <paramref name="item"/> в наборе.
        /// </summary>
        /// <param name="item">Элемент, для которого возвращается количество</param>
        /// <returns>
        /// Количество элемента в наборе. Если элемент в наборе отсутствует — возвращает 0
        /// </returns>
        public int CountOf(T item)
        {
            return dict.GetOrDefault(item, 0);
        }
        #endregion

        #region ICollection Implementation
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return dict.Keys.GetEnumerator();
        }

        public void Add(T item)
        {
            Add(item, 1);
        }

        public void Clear()
        {
            dict.Clear();
        }

        public bool Contains(T item)
        {
            return item != null && dict.ContainsKey(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            dict.Keys.CopyTo(array, arrayIndex);
        }

        // ReSharper disable once AnnotationConflictInHierarchy
        public bool Remove([NotNull] T item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return dict.Remove(item);
        }

        public int Count => dict.Count;

        public bool IsReadOnly => false;

        #endregion
    }
}
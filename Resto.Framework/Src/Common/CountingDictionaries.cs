using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

using Resto.Framework.Attributes.JetBrains;

using Wintellect.PowerCollections;

namespace Resto.Framework.Common
{
    /// <summary>
    /// Класс для хранения значения и его количества.
    /// Используется в <see cref="CountingDictionary{TKey,TValue}"/> и
    /// в <see cref="CountingMultiDictionary{TKey,TValue}"/>.
    /// </summary>
    /// <typeparam name="TValue">Тип значения</typeparam>
    /// <seealso cref="CountingDictionary{TKey,TValue}"/>
    /// <seealso cref="CountingMultiDictionary{TKey,TValue}"/>
    public sealed class ValueWithCount<TValue> : IEquatable<ValueWithCount<TValue>>, IEquatable<TValue>
    {
        #region Fields
        private readonly TValue value;
        private int count;
        #endregion

        #region Ctor
        /// <summary>
        /// Новый экземпляр со значением <paramref name="value"/> и количеством 1
        /// </summary>
        /// <param name="value">Значение</param>
        public ValueWithCount(TValue value)
        {
            this.value = value;
            count = 1;
        }

        /// <summary>
        /// Новый экземпляр со значением <paramref name="value"/> и количеством <paramref name="count"/>
        /// </summary>
        /// <param name="value">Значение</param>
        /// <param name="count">Количество (больше 0)</param>
        public ValueWithCount(TValue value, int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be greater than zero");

            this.value = value;
            this.count = count;
        }
        #endregion

        #region Props
        /// <summary>
        /// Хранимое значение
        /// </summary>
        public TValue Value
        {
            get { return value; }
        }

        /// <summary>
        /// Количество
        /// </summary>
        public int Count
        {
            get { return count; }
        }
        #endregion

        #region Methods
        internal int Increase(int countToAdd)
        {
            if (countToAdd < 1)
                throw new ArgumentOutOfRangeException(nameof(countToAdd), countToAdd, "Count to add must be greater than zero");

            count += countToAdd;
            return count;
        }

        internal int Decrease(int countToRemove)
        {
            if (countToRemove >= count)
                throw new ArgumentOutOfRangeException(nameof(countToRemove), countToRemove, "Count to remove must be less than count");

            count -= countToRemove;
            return count;
        }
        #endregion

        #region Equality Members
        public bool Equals(ValueWithCount<TValue> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.value, value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (ValueWithCount<TValue>)) return false;
            return Equals((ValueWithCount<TValue>) obj);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public bool Equals(TValue other)
        {
            return value.Equals(other);
        }
        #endregion
    }

    /// <summary>
    /// Словарь с подсчётом количества значений
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип значения</typeparam>
    public sealed class CountingDictionary<TKey, TValue> : IDictionary<TKey, ValueWithCount<TValue>>
    {
        #region Fields
        private readonly Dictionary<TKey, ValueWithCount<TValue>> dict =
            new Dictionary<TKey, ValueWithCount<TValue>>();
        #endregion

        #region Methods
        /// <summary>
        /// Добавляет в словарь ключ <paramref name="key"/> и значение <paramref name="value"/> в количестве 1
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        /// <returns>
        /// Количество значения <paramref name="value"/> для ключа <paramref name="key"/> после добавления
        /// </returns>
        public int AddOne(TKey key, TValue value)
        {
            return Add(key, value, 1);
        }

        /// <summary>
        /// Добавляет в словарь ключ <paramref name="key"/> и значение <paramref name="value"/> в количестве <paramref name="count"/>
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        /// <param name="count">Количество (больше 0)</param>
        /// <returns>
        /// Количество значения <paramref name="value"/> для ключа <paramref name="key"/> после добавления
        /// </returns>
        public int Add(TKey key, TValue value, int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be greater than zero");

            ValueWithCount<TValue> valueWithCount;
            if (dict.TryGetValue(key, out valueWithCount))
            {
                if (!valueWithCount.Value.Equals(value))
                    throw new ArgumentException("Counting dictionary already contains key 'key' with another value", nameof(value));

                return valueWithCount.Increase(count);
            }

            dict.Add(key, new ValueWithCount<TValue>(value, count));
            return count;
        }

        /// <summary>
        /// Возвращает количество значений для ключа <paramref name="key"/>
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>
        /// Количество значений для ключа. Если ключ в словаре отсутствует — возвращает 0.
        /// </returns>
        public int ValuesCount(TKey key)
        {
            ValueWithCount<TValue> valueWithCount;
            return dict.TryGetValue(key, out valueWithCount) ? valueWithCount.Count : 0;
        }

        /// <summary>
        /// Уменьшает количество значения для ключа <paramref name="key"/> на 1.
        /// Если количество значения для ключа становится равным 0, ключ из словаря удаляется.
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>
        /// Количество значения для ключа <paramref name="key"/> после уменьшения
        /// </returns>
        public int RemoveOne(TKey key)
        {
            return Remove(key, 1);
        }

        /// <summary>
        /// Уменьшает количество значения для ключа <paramref name="key"/> на <paramref name="count"/>.
        /// Если количество значения для ключа становится равным 0, ключ из словаря удаляется.
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="count">Количество (больше 0)</param>
        /// <returns>
        /// Количество значения для ключа <paramref name="key"/> после уменьшения
        /// </returns>
        public int Remove(TKey key, int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be greater than zero");

            ValueWithCount<TValue> valueWithCount;
            if (!dict.TryGetValue(key, out valueWithCount))
                throw new ArgumentException("Counting dictionary does not contains key 'key'", nameof(key));

            if (valueWithCount.Count < count)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be less or equal than actual values count");

            if (valueWithCount.Count == count)
            {
                dict.Remove(key);
                return 0;
            }

            return valueWithCount.Decrease(count);
        }

        /// <summary>
        /// Возвращает значение для ключа <paramref name="key"/>
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>
        /// Значение для ключа <paramref name="key"/>
        /// </returns>
        public TValue Get(TKey key)
        {
            ValueWithCount<TValue> valueWithCount;
            if (dict.TryGetValue(key, out valueWithCount))
                return valueWithCount.Value;

            throw new ArgumentException("Counting dictionary does not contains key 'key'", nameof(key));
        }
        #endregion

        #region Interface Implementation
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TKey, ValueWithCount<TValue>>> GetEnumerator()
        {
            return dict.GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, ValueWithCount<TValue>> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            dict.Clear();
        }

        public bool Contains(KeyValuePair<TKey, ValueWithCount<TValue>> item)
        {
            return ((ICollection<KeyValuePair<TKey, ValueWithCount<TValue>>>)dict).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, ValueWithCount<TValue>>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, ValueWithCount<TValue>>>)dict).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, ValueWithCount<TValue>> item)
        {
            return ((ICollection<KeyValuePair<TKey, ValueWithCount<TValue>>>)dict).Remove(item);
        }

        public int Count
        {
            get { return dict.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool ContainsKey(TKey key)
        {
            return dict.ContainsKey(key);
        }

        public void Add(TKey key, ValueWithCount<TValue> value)
        {
            Add(key, value.Value, value.Count);
        }

        public bool Remove(TKey key)
        {
            return dict.Remove(key);
        }

        public bool TryGetValue(TKey key, out ValueWithCount<TValue> value)
        {
            return dict.TryGetValue(key, out value);
        }

        public ValueWithCount<TValue> this[TKey key]
        {
            get { return dict[key]; }
            set { dict[key] = value; }
        }

        public ICollection<TKey> Keys
        {
            get { return dict.Keys; }
        }

        public ICollection<ValueWithCount<TValue>> Values
        {
            get { return dict.Values; }
        }
        #endregion
    }

    /// <summary>
    /// Словарь «ключ — несколько значений» с подсчётом количества значений внутри ключа
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип значения</typeparam>
    public sealed class CountingMultiDictionary<TKey, TValue> : MultiDictionaryBase<TKey, ValueWithCount<TValue>>
    {
        #region Fields
        private readonly ListMultiDictionary<TKey, ValueWithCount<TValue>> dict =
            new ListMultiDictionary<TKey, ValueWithCount<TValue>>();
        #endregion

        #region Methods
        /// <summary>
        /// Добавляет в ключ <paramref name="key"/> значение <paramref name="value"/> в количестве 1.
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        /// <returns>
        /// Количество значения <paramref name="value"/> для ключа <paramref name="key"/> после добавления
        /// </returns>
        public int AddOne(TKey key, TValue value)
        {
            return Add(key, value, 1);
        }

        /// <summary>
        /// Добавляет в ключ <paramref name="key"/> значение <paramref name="value"/> в количестве <paramref name="count"/>.
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        /// <param name="count">Количество (больше 0)</param>
        /// <returns>
        /// Количество значения <paramref name="value"/> для ключа <paramref name="key"/> после добавления
        /// </returns>
        public int Add(TKey key, TValue value, int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be greater than zero");

            var values = dict[key];

            var valueWithCount = TryGetValueWithCountFor(values, value);

            if (valueWithCount == null)
            {
                values.Add(new ValueWithCount<TValue>(value, count));
                return count;
            }
            
            return valueWithCount.Increase(count);
        }

        /// <summary>
        /// Возвращает количество значения <paramref name="value"/> для ключа <paramref name="key"/>
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        /// <returns>
        /// Количество значения <paramref name="value"/> для ключа <paramref name="key"/>.
        /// Если такого ключа нет — возвращает 0.
        /// Если ключ есть, но такого значения в ключе нет — возвращает 0.
        /// </returns>
        public int ValuesCount(TKey key, TValue value)
        {
            if (!dict.ContainsKey(key))
                return 0;

            var values = dict[key];
            var valueWithCount = TryGetValueWithCountFor(values, value);
            return valueWithCount == null ? 0 : valueWithCount.Count;
        }

        /// <summary>
        /// Уменьшает количество значения <paramref name="value"/> для ключа <paramref name="key"/> на 1.
        /// Если количество значения для ключа становится равным 0 и это единственное значение ключа, ключ из словаря удаляется.
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        /// <returns>
        /// Количество значения для ключа <paramref name="key"/> после уменьшения
        /// </returns>
        public int RemoveOne(TKey key, TValue value)
        {
            return Remove(key, value, 1);
        }

        /// <summary>
        /// Уменьшает количество значения <paramref name="value"/> для ключа <paramref name="key"/> на <paramref name="count"/>.
        /// Если количество значения для ключа становится равным 0 и это единственное значение ключа, ключ из словаря удаляется.
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        /// <param name="count">Количество (больше 0)</param>
        /// <returns>
        /// Количество значения для ключа <paramref name="key"/> после уменьшения
        /// </returns>
        public int Remove(TKey key, TValue value, int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be greater than zero");

            var values = dict[key];

            if (values.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(key), key, "Counting multi dictionary does not contains key 'key'");

            var valueWithCount = TryGetValueWithCountFor(values, value);

            if (valueWithCount == null || valueWithCount.Count < count)
                throw new ArgumentOutOfRangeException(nameof(count), count, "Count must be less or equal than actual values count");

            if (valueWithCount.Count == count)
            {
                values.Remove(valueWithCount);
                return 0;
            }
            
            return valueWithCount.Decrease(count);
        }

        /// <summary>
        /// Возвращает список значений для ключа <paramref name="key"/>
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns>Список значений для ключа <paramref name="key"/></returns>
        public IList<TValue> Get(TKey key)
        {
            var values = dict[key];

            var result = new List<TValue>(values.Count);
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var value in values)
                result.Add(value.Value);

            return result;
        }

        [CanBeNull, Pure]
        private static ValueWithCount<TValue> TryGetValueWithCountFor([NotNull] IEnumerable<ValueWithCount<TValue>> values, TValue value)
        {
            Debug.Assert(values != null);

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var val in values)
            {
                if (val.Value.Equals(value))
                    return val;
            }

            return null;
        }
        #endregion

        #region MultiDictionaryBase Implementation
        public override void Clear()
        {
            dict.Clear();
        }

        protected override IEnumerator<TKey> EnumerateKeys()
        {
            return dict.Keys.GetEnumerator();
        }

        protected override bool TryEnumerateValuesForKey(TKey key, out IEnumerator<ValueWithCount<TValue>> values)
        {
            if (dict.ContainsKey(key))
            {
                values = dict[key].GetEnumerator();
                return true;
            }
            
            values = null;
            return false;
        }

        public override void Add(TKey key, ValueWithCount<TValue> value)
        {
            Add(key, value.Value, value.Count);
        }

        public override bool Remove(TKey key)
        {
            return dict.Remove(key);
        }

        public override bool Remove(TKey key, ValueWithCount<TValue> value)
        {
            return dict.Remove(key, value);
        }

        public override bool Contains(TKey key, ValueWithCount<TValue> value)
        {
            return dict.Contains(key, value);
        }

        public override int Count
        {
            get { return dict.Count; }
        }
        #endregion
    }
}
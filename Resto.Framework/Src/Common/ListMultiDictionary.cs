using System.Collections.Generic;
using System.Linq;
using Wintellect.PowerCollections;

namespace Resto.Framework.Common
{
    /// <summary>
    /// The ListMultiDictionary class that associates values with a key. Unlike an Dictionary,
    /// each key can have multiple values associated with it. When indexing an MultiDictionary,
    /// instead of a single value associated with a key, you retrieve an enumeration of values.
    /// Same value can be associated with a key multiple times.
    /// ListMultiDictionary uses <see cref="List{T}"/> to store values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TValue">The of values associated with the keys.</typeparam>
    public sealed class ListMultiDictionary<TKey, TValue> : MultiDictionaryBase<TKey, TValue>, IReadOnlyDictionary<TKey, IReadOnlyList<TValue>>
    {
        #region Fields
        private readonly Dictionary<TKey, List<TValue>> dict = new Dictionary<TKey, List<TValue>>();
        #endregion

        #region Props
        public override int Count => dict.Count;
        #endregion

        #region Methods
        /// <summary>
        /// Removes all keys and values from the dictionary.
        /// </summary>
        public override void Clear()
        {
            dict.Clear();
        }

        /// <summary>
        /// Enumerate all the keys in the dictionary.
        /// </summary>
        /// <returns>An IEnumerator that enumerates all of the keys in the dictionary that have at least one value associated with them.</returns>
        protected override IEnumerator<TKey> EnumerateKeys()
        {
            return dict.Keys.GetEnumerator();
        }

        /// <summary>
        /// Determines if this dictionary contains a key equal to key.
        /// If so, all the values associated with that key are returned through the values parameter.
        /// </summary>
        /// <param name="key">The key to search for.</param>
        /// <param name="values">Returns all values associated with key, if true was returned.</param>
        /// <returns>True if the dictionary contains key. False if the dictionary does not contain key.</returns>
        protected override bool TryEnumerateValuesForKey(TKey key, out IEnumerator<TValue> values)
        {
            if (dict.TryGetValue(key, out var vals))
            {
                values = vals.GetEnumerator();
                return true;
            }

            values = null;
            return false;
        }

        /// <summary>
        /// Adds a new value to be associated with a key.
        /// If key already has a value equal to value associated with it,
        /// then that value is replaced with value, and the number of values associate with key is unchanged.
        /// </summary>
        /// <param name="key">The key to associate with.</param>
        /// <param name="value">The value to associated with key.</param>
        public override void Add(TKey key, TValue value)
        {
            if (dict.TryGetValue(key, out var values))
                values.Add(value);
            else
                dict.Add(key, new List<TValue>(1) { value });
        }

        /// <summary>
        /// Removes a key and all associated values from the dictionary.
        /// If the key is not present in the dictionary, it is unchanged and false is returned.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns>True if the key was present and was removed. Returns false if the key was not present.</returns>
        public override bool Remove(TKey key)
        {
            return dict.Remove(key);
        }

        /// <summary>
        /// Removes a given value from the values associated with a key.
        /// If the last value is removed from a key, the key is removed also.
        /// </summary>
        /// <param name="key">A key to remove a value from.</param>
        /// <param name="value">The value to remove.</param>
        /// <returns>True if value was associated with key (and was therefore removed). False if value was not associated with key.</returns>
        public override bool Remove(TKey key, TValue value)
        {
            if (dict.TryGetValue(key, out var values))
            {
                var result = values.Remove(value);
                if (!result)
                    return false;

                if (values.Count == 0)
                    dict.Remove(key);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks to see if value is associated with key in the dictionary.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <param name="value">The value to check.</param>
        /// <returns>True if value is associated with key.</returns>
        public override bool Contains(TKey key, TValue value)
        {
            return dict.TryGetValue(key, out var values) && values.Contains(value);
        }

        public override bool ContainsKey(TKey key)
        {
            return dict.ContainsKey(key);
        }

        protected override int CountValues(TKey key)
        {
            return dict.TryGetValue(key, out var values) ? values.Count : 0;
        }

        protected override int CountAllValues()
        {
            return dict.Sum(kvp => kvp.Value.Count);
        }
        #endregion

        #region IReadOnlyDictionary Members
        bool IReadOnlyDictionary<TKey, IReadOnlyList<TValue>>.TryGetValue(TKey key, out IReadOnlyList<TValue> value)
        {
            var result = dict.TryGetValue(key, out var localValue);
            value = localValue;
            return result;
        }

        IReadOnlyList<TValue> IReadOnlyDictionary<TKey, IReadOnlyList<TValue>>.this[TKey key] => dict[key];

        IEnumerable<TKey> IReadOnlyDictionary<TKey, IReadOnlyList<TValue>>.Keys => dict.Keys;

        IEnumerable<IReadOnlyList<TValue>> IReadOnlyDictionary<TKey, IReadOnlyList<TValue>>.Values => dict.Values;

        IEnumerator<KeyValuePair<TKey, IReadOnlyList<TValue>>> IEnumerable<KeyValuePair<TKey, IReadOnlyList<TValue>>>.GetEnumerator()
        {
            return dict.Select(kvp => new KeyValuePair<TKey, IReadOnlyList<TValue>>(kvp.Key, kvp.Value)).GetEnumerator();
        }
        #endregion
    }
}

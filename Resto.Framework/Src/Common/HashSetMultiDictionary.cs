using System;
using System.Collections;
using System.Collections.Generic;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    // TODO: возможно, стоит использовать MultiValueDictionary (https://blogs.msdn.microsoft.com/dotnet/2014/08/05/multidictionary-becomes-multivaluedictionary/)
    /// <summary>
    /// The HashSetMultiDictionary class that associates values with a key. Unlike an Dictionary,
    /// each key can have multiple values associated with it. When indexing an MultiDictionary,
    /// instead of a single value associated with a key, you retrieve an enumeration of values.
    /// Same value can be associated with a key only one time. 
    /// HashSetMultiDictionary uses <see cref="HashSet{T}"/> to store values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys.</typeparam>
    /// <typeparam name="TValue">The of values associated with the keys.</typeparam>
    public sealed class HashSetMultiDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, IReadOnlyCollection<TValue>>
    {
        #region Fields
        private readonly Dictionary<TKey, HashSet<TValue>> dict = new Dictionary<TKey, HashSet<TValue>>();
        #endregion

        public HashSetMultiDictionary()
        { }

        public HashSetMultiDictionary([NotNull] HashSetMultiDictionary<TKey, TValue> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            foreach (var pair in source)
            {
                dict.Add(pair.Key, new HashSet<TValue>(pair.Value));
            }
        }

        #region Props
        public int Count
        {
            get { return dict.Count; }
        }

        [NotNull]
        public IReadOnlyCollection<TValue> this[TKey key]
        {
            get
            {
                IReadOnlyCollection<TValue> existingValues = dict.GetOrDefault(key);
                return existingValues ?? Array.Empty<TValue>();
            }
        }

        public IEnumerable<TKey> Keys
        {
            get { return dict.Keys; }
        }

        public IEnumerable<IReadOnlyCollection<TValue>> Values
        {
            get { return dict.Values; }
        }
        #endregion

        #region Methods
        public bool TryGetValue(TKey key, out IReadOnlyCollection<TValue> value)
        {
            HashSet<TValue> tmp;
            var result = dict.TryGetValue(key, out tmp);
            value = tmp;
            return result;
        }

        /// <summary>
        /// Removes all keys and values from the dictionary.
        /// </summary>
        public void Clear()
        {
            dict.Clear();
        }

        /// <summary>
        /// Adds a new value to be associated with a key.
        /// If key already has a value equal to value associated with it,
        /// then that value is replaced with value, and the number of values associate with key is unchanged.
        /// </summary>
        /// <param name="key">The key to associate with.</param>
        /// <param name="value">The value to associated with key.</param>
        public void Add(TKey key, TValue value)
        {
            HashSet<TValue> values;
            if (dict.TryGetValue(key, out values))
                values.Add(value);
            else
                dict.Add(key, new HashSet<TValue> { value });
        }

        /// <summary>
        /// Removes a key and all associated values from the dictionary.
        /// If the key is not present in the dictionary, it is unchanged and false is returned.
        /// </summary>
        /// <param name="key">The key to remove.</param>
        /// <returns>True if the key was present and was removed. Returns false if the key was not present.</returns>
        public bool Remove(TKey key)
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
        public bool Remove(TKey key, TValue value)
        {
            HashSet<TValue> values;
            if (dict.TryGetValue(key, out values))
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

        public bool ContainsKey(TKey key)
        {
            return dict.ContainsKey(key);
        }

        IEnumerator<KeyValuePair<TKey, IReadOnlyCollection<TValue>>> IEnumerable<KeyValuePair<TKey, IReadOnlyCollection<TValue>>>.GetEnumerator()
        {
            foreach (var pair in dict)
            {
                yield return new KeyValuePair<TKey, IReadOnlyCollection<TValue>>(pair.Key, pair.Value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyDictionary<TKey, IReadOnlyCollection<TValue>>)this).GetEnumerator();
        }
        #endregion
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using EnumerableExtensions;
using Resto.Framework.Attributes.JetBrains;
using Wintellect.PowerCollections;

namespace Resto.Framework.Common
{
    public static class DictionaryHelper
    {
        public static Dictionary<TValue, TKey> Invert<TKey, TValue>(this Dictionary<TKey, TValue> dic)
        {
            var res = new Dictionary<TValue, TKey>(dic.Count);
            res.AddRange(dic.Select(pair => new KeyValuePair<TValue, TKey>(pair.Value, pair.Key)));
            return res;
        }

        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dic,
            IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            pairs.ForEach(pair => dic.Add(pair.Key, pair.Value));
        }

        public static void AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dic,
            IEnumerable<TValue> list, Func<TValue, TKey> selectKeyPredicate)
        {
            list.ForEach(listElement => dic.Add(selectKeyPredicate(listElement), listElement));
        }

        /// <summary>
        /// Заменить содержимое словаря.
        /// </summary>
        public static void Set<TKey, TValue>([NotNull] this Dictionary<TKey, TValue> dic, [NotNull] Dictionary<TKey, TValue> pairs)
        {
            dic.Clear();
            dic.AddRange(pairs);
        }

        /// <summary>
        /// Заменить содержимое словаря.
        /// </summary>
        public static void Set<TKey, TValue>([NotNull] this Dictionary<TKey, TValue> dic, [NotNull] IEnumerable<KeyValuePair<TKey, TValue>> pairs)
        {
            dic.Clear();
            dic.AddRange(pairs);
        }

        public static void Set<TKey, TValue>([NotNull] this Dictionary<TKey, TValue> dic, [NotNull] IEnumerable<TValue> list,
            Func<TValue, TKey> selectKeyPredicate)
        {
            dic.Clear();
            dic.AddRange(list, selectKeyPredicate);
        }

        public static void Remove<TKey, TValue>(this Dictionary<TKey, TValue> dic,
            Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            foreach (var key in dic.Where(predicate).Select(pair => pair.Key).ToList())
            {
                dic.Remove(key);
            }
        }

        /// <summary>
        /// Removes the elements range with the specified keys from the IDictionary&lt;TKey,TValue&gt;.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="keys">Keys of the elements to remove.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="keys"/> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        public static void RemoveRange<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, [NotNull] IEnumerable<TKey> keys)
        {
            keys.ForEach(key => dictionary.Remove(key));
        }

        public static TValue GetOrAdd<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dict, TKey key, [InstantHandle] Func<TKey, TValue> valueFactory)
        {
            Contract.Requires(dict != null);
            Contract.Requires(key != null);
            Contract.Requires(valueFactory != null);

            if (!dict.TryGetValue(key, out var value))
            {
                value = valueFactory(key);
                dict.Add(key, value);
            }
            return value;
        }

        #region GetOrDefault for IDictionary
        public static TValue GetOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dic, TKey key, TValue defaultValue)
        {
            return dic.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static TValue? GetOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dic, TKey key, TValue? defaultValue) where TValue : struct
        {
            return dic.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static TValue GetOrDefault<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dic, TKey key)
        {
            dic.TryGetValue(key, out var value);
            return value;
        }
        #endregion

        #region GetOrDefault for IReadOnlyDictionary
        public static TValue GetOrDefault<TKey, TValue>([NotNull] this IReadOnlyDictionary<TKey, TValue> dic, TKey key, TValue defaultValue)
        {
            return dic.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static TValue? GetOrDefault<TKey, TValue>([NotNull] this IReadOnlyDictionary<TKey, TValue> dic, TKey key, TValue? defaultValue) where TValue : struct
        {
            return dic.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static TValue GetOrDefault<TKey, TValue>([NotNull] this IReadOnlyDictionary<TKey, TValue> dic, TKey key)
        {
            dic.TryGetValue(key, out var value);
            return value;
        }
        #endregion

        #region GetOrDefault for Dictionary
        public static TValue GetOrDefault<TKey, TValue>([NotNull] this Dictionary<TKey, TValue> dic, TKey key, TValue defaultValue)
        {
            return dic.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static TValue? GetOrDefault<TKey, TValue>([NotNull] this Dictionary<TKey, TValue> dic, TKey key, TValue? defaultValue) where TValue : struct
        {
            return dic.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static TValue GetOrDefault<TKey, TValue>([NotNull] this Dictionary<TKey, TValue> dic, TKey key)
        {
            dic.TryGetValue(key, out var value);
            return value;
        }
        #endregion

        #region GetOrDefault for ConcurrentDictionary
        public static TValue GetOrDefault<TKey, TValue>([NotNull] this ConcurrentDictionary<TKey, TValue> dic, TKey key, TValue defaultValue)
        {
            return dic.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static TValue? GetOrDefault<TKey, TValue>([NotNull] this ConcurrentDictionary<TKey, TValue> dic, TKey key, TValue? defaultValue) where TValue : struct
        {
            return dic.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public static TValue GetOrDefault<TKey, TValue>([NotNull] this ConcurrentDictionary<TKey, TValue> dic, TKey key)
        {
            dic.TryGetValue(key, out var value);
            return value;
        }
        #endregion

        public static Maybe<TValue> TryGetValue<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, [NotNull] TKey key)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            return dictionary.TryGetValue(key, out var value) ? value : Maybe<TValue>.Empty;
        }

        /// <summary>
        /// Объединить несколько словарей в один.
        /// </summary>
        /// <remarks>При дублировании ключей будет браться одно из значений. Результат помещается в новый словарь.</remarks>
        /// <typeparam name="TKey">Тип ключа.</typeparam>
        /// <typeparam name="TValue">Тип значения.</typeparam>
        /// <param name="original">Начальный словарь.</param>
        /// <param name="dictionaries">Объединяемые словари.</param>
        /// <returns>Объединенный словарь.</returns>
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(
            this IDictionary<TKey, TValue> original, params IDictionary<TKey, TValue>[] dictionaries)
        {
            IDictionary<TKey, TValue> merged = new Dictionary<TKey, TValue>(original);
            foreach (var pair in dictionaries.SelectMany(dictionary => dictionary))
            {
                merged[pair.Key] = pair.Value;
            }
            return merged;
        }

        public static IDictionary<TKey, TValue> AsReadOnly<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            return Algorithms.ReadOnly(dictionary);
        }

        /// <summary>
        /// Добавляет или обновляет в словаре запись, соответствующую ключу <paramref name="key"/>
        /// </summary>
        /// <param name="dictionary">Словарь для добавления/обновления элемента</param>
        /// <param name="key">Ключ элемента</param>
        /// <param name="newValue">Значение, которое будет прописано в словарь в случае, если для <paramref name="key"/> не будет найдена запись</param>
        /// <param name="updateFunc">Функция обновления, которая будет применена в случае, если для <paramref name="key"/> будет найдена запись</param>
        public static void AddOrUpdate<TKey, TValue>([NotNull] this IDictionary<TKey, TValue> dictionary, TKey key, TValue newValue,
            Func<TValue, TValue> updateFunc)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            if (dictionary.TryGetValue(key, out var value))
                dictionary[key] = updateFunc(value);
            else
                dictionary.Add(key, newValue);
        }
    }
}

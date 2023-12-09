namespace System.Collections.Generic
{
    public sealed class CloneSafeSynchronizedDictionary<TKey, TValue> : ICloneableDictionary<TKey, TValue>, IDictionary
    {
        #region Fields / Properties

        private readonly object syncRoot = new object();
        private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        #endregion

        #region Constructors

        public 

        #endregion

        #region IDictionary<TKey,TValue> Members

        void Add(TKey key, TValue value)
        {
            lock (syncRoot)
            {
                dictionary.Add(key, value);
            }
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        object IDictionary.this[object key]
        {
            get
            {
                return ((IDictionary) dictionary)[key];
            }
            set
            {
                lock (syncRoot)
                {
                    ((IDictionary)dictionary)[key] = value;
                }
            }
        }

        ICollection IDictionary.Keys
        {
            get 
            {
                return ((IDictionary) dictionary).Keys;
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                return ((IDictionary)dictionary).Values;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                lock (syncRoot)
                {
                    return dictionary.Keys;
                }
            }
        }

        public bool Remove(TKey key)
        {
            lock (syncRoot)
            {
                return dictionary.Remove(key);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (syncRoot)
            {
                return dictionary.TryGetValue(key, out value);
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                lock (syncRoot)
                {
                    return dictionary.Values;
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return dictionary[key];
            }
            set
            {
                lock (syncRoot)
                {
                    dictionary[key] = value;
                }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>Members

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            lock (syncRoot)
            {
                ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Add(item);
            }
        }

        public bool Contains(object key)
        {
            return ((IDictionary) dictionary).Contains(key);
        }

        public void Add(object key, object value)
        {
            lock (syncRoot)
            {
                ((IDictionary) dictionary).Add(key, value);
            }
        }

        public void Clear()
        {
            lock (syncRoot)
            {
                dictionary.Clear();
            }
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary) dictionary).GetEnumerator();
        }

        public void Remove(object key)
        {
            lock (syncRoot)
            {
                ((IDictionary) dictionary).Remove(key);
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey,TValue>>)dictionary).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (syncRoot)
            {
                ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).CopyTo(array,arrayIndex);
            }
        }

        public void CopyTo(Array array, int index)
        {
            lock (syncRoot)
            {
                ((IDictionary) dictionary).CopyTo(array, index);
            }
        }

        public int Count
        {
            get
            {
                return dictionary.Count;
            }
        }

        public object SyncRoot
        {
            get { return syncRoot; }
        }

        public bool IsSynchronized
        {
            get { return true; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (syncRoot)
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).Remove(item);
            }
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey, TValue> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)dictionary).GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)dictionary).GetEnumerator();
        }

        #endregion

        #region ICloneableDictionary<TKey,TValue> Members
        public ICloneableDictionary<TKey, TValue> Clone()
        {
            lock (syncRoot)
            {
                return new CloneSafeSynchronizedDictionary<TKey, TValue> {
                    dictionary = new Dictionary<TKey, TValue>(dictionary)
                };
            }
        }
        #endregion
    }
}
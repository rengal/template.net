using System;
using System.Collections.Generic;
using System.Threading;
using Resto.Framework.Attributes.JetBrains;

namespace Resto.Framework.Common
{
    public sealed class Cache<TKey, TValue>
    {
        #region Fields
        private readonly Dictionary<TKey, TValue> cache = new Dictionary<TKey, TValue>();

        private readonly Func<TKey, TValue> evaluator;
        #endregion

        #region Ctor
        public Cache([NotNull] Func<TKey, TValue> evaluator)
        {
            if (evaluator == null)
                throw new ArgumentNullException(nameof(evaluator));

            this.evaluator = evaluator;
        }
        #endregion

        #region Props
        public TValue this[TKey key]
        {
            get
            {
                var inCache = cache.TryGetValue(key, out var val);

                if (inCache)
                    return val;

                val = evaluator(key);
                cache.Add(key, val);

                return val;
            }
        }
        #endregion

        #region Methods
        public void Invalidate()
        {
            cache.Clear();
        }
        #endregion
    }

    public sealed class ThreadSafeCache<TKey, TValue>
    {
        #region Fields
        private readonly ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();
        private readonly Dictionary<TKey, TValue> cache = new Dictionary<TKey, TValue>();

        private readonly Func<TKey, TValue> evaluator;
        #endregion

        #region Ctor
        public ThreadSafeCache([NotNull] Func<TKey, TValue> evaluator)
        {
            if (evaluator == null)
                throw new ArgumentNullException(nameof(evaluator));

            this.evaluator = evaluator;
        }
        #endregion

        #region Props
        public TValue this[TKey key]
        {
            get
            {
                TValue val;

                bool inCache;
                using (cacheLock.GetReadLock())
                {
                    inCache = cache.TryGetValue(key, out val);
                }

                if (inCache)
                    return val;
                
                using (cacheLock.GetWriteLock())
                {
                    if (!cache.TryGetValue(key, out val))
                    {
                        val = evaluator(key);
                        cache.Add(key, val);
                    }
                }

                return val;
            }
        }
        #endregion

        #region Methods
        public void Invalidate()
        {
            cacheLock.EnterWriteLock();
            cache.Clear();
            cacheLock.ExitWriteLock();
        }
        #endregion
    }

    public static class CacheExtensions
    {
        public static Func<TKey, TValue> Memoize<TKey, TValue>([NotNull] this Func<TKey, TValue> func, bool threadSafe = false)
        {
            if (threadSafe)
            {
                var cache = new ThreadSafeCache<TKey, TValue>(func);
                return key => cache[key];
            }
            else
            {
                var cache = new Cache<TKey, TValue>(func);
                return key => cache[key];
            }
        }
    }
}

namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Caching;

    using SRC = System.Runtime.Caching;

    /// <summary>
    /// Memory cache
    /// <see cref="http://msdn.microsoft.com/en-us/library/dd997357.aspx"/>
    /// <see cref="http://deanhume.com/Home/BlogPost/object-caching----net-4/37"/>
    /// </summary>
    public class CustomerSettingCache : ICustomerSettingCache
    {
        /// <summary>
        /// Contains a list of keys of current cache instance.
        /// </summary>
        private static volatile HashSet<string> listKey;

        /// <summary>
        /// Singleton object cache instance.
        /// </summary>
        private static volatile ObjectCache objCache;

        /// <summary>
        /// Use to double check create singleton instance.
        /// </summary>
        private static object syncRoot = new object();

        #region ICache Members

        /// <summary>
        /// Gets the number of item in current cache instance.
        /// </summary>
        public int Count
        {
            get
            {
                return (int)CacheInstance.GetCount();
            }
        }

        /// <summary>
        /// Gets all keys in current cache instance.
        /// </summary>
        public HashSet<string> Keys
        {
            get
            {
                if (listKey == null)
                {
                    lock (syncRoot)
                    {
                        if (listKey == null)
                        {
                            listKey = new HashSet<string>();
                        }
                    }
                }

                return listKey;
            }
        }

        /// <summary>
        /// Gets an ObjectCache cache instance.
        /// </summary>
        private static ObjectCache CacheInstance
        {
            get
            {
                if (objCache == null)
                {
                    lock (syncRoot)
                    {
                        if (objCache == null)
                        {
                            objCache = SRC.MemoryCache.Default;
                        }
                    }
                }

                return objCache;
            }
        }

        /// <summary>
        /// Determines whether cache contains the specified key.
        /// </summary>
        /// <param name="key">The string key.</param>
        /// <returns>
        ///   <c>True</c> if [contains] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string key)
        {
            return CacheInstance.Contains(key);
        }

        /// <summary>
        /// Gets cache item by the specified key.
        /// </summary>
        /// <typeparam name="T">Type of cache item.</typeparam>
        /// <param name="key">The string key.</param>
        /// <returns>
        /// The cache item.
        /// </returns>
        public T Get<T>(string key)
        {
            return this.Contains(key) ? (T)CacheInstance[key] : default(T);
        }

        /// <summary>
        /// Get cache item by specified key, and cast to type T
        /// if cast failed return out T.
        /// </summary>
        /// <typeparam name="T">Type of cache item.</typeparam>
        /// <param name="key">The string key.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// True/false to indicate whether getting cache item and casting successful or not.
        /// </returns>
        public bool TryGet<T>(string key, out T value)
        {
            bool checkTryGet = this.Contains(key);
            if (checkTryGet)
            {
                value = (T)Get<T>(key);
            }
            else
            {
                value = default(T);
            }

            return checkTryGet;
        }

        /// <summary>
        /// Adds item with a specified key to cache, if the key already exist then update.
        /// </summary>
        /// <typeparam name="T"> Type of cache item.</typeparam>
        /// <param name="key">The string key.</param>
        /// <param name="value">The value.</param>
        /// <param name="cacheItemPolicy">The cache item policy.</param>
        public void Set<T>(string key, T value, CacheItemPolicy cacheItemPolicy)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentNullException();
            }

            CacheItem cacheItem = new CacheItem(key, value);
            CacheInstance.Set(cacheItem, cacheItemPolicy);

            //// Because HashSet is not thread safe
            lock (syncRoot)
            {
                this.Keys.Add(key);
            }
        }

        /// <summary>
        /// Sets the specified key.
        /// </summary>
        /// <typeparam name="T">Type of cache item.</typeparam>
        /// <param name="key">The string key.</param>
        /// <param name="value">The value.</param>
        /// <param name="timeOut">The second time out, default is 120s.</param>
        public void Set<T>(string key, T value, int timeOut = 120)
        {
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            this.Set<T>(key, value, DateTime.Now.AddSeconds(timeOut));
        }

        /// <summary>
        ///   <para>Let’s say, the cache item is created at 8:05:00.</para>
        ///   <para>and the lifetime is given to 5 seconds. </para>
        ///   <para>Then the lifetime of the cache item expires at 8:05:05.</para>
        /// </summary>
        /// <typeparam name="T">Type of cache item.</typeparam>
        /// <param name="key">The string key.</param>
        /// <param name="value">The value.</param>
        /// <param name="absoluteExpiration">DateTime AbsoluteExpiration.</param>
        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.AbsoluteExpiration = absoluteExpiration;
            this.Set<T>(key, value, cacheItemPolicy);
        }

        /// <summary>
        ///   <para>Let’s say, the cache item is created at 8:05:00.</para>
        ///   <para>and the lifetime is given to 5 seconds.</para>
        ///   <para>If an access to the cache item happen at 8:05:04.</para>
        ///   <para>the expiring time of the cache item will be updated to 8:05:09.</para>
        /// </summary>
        /// <typeparam name="T">Type of cache item.</typeparam>
        /// <param name="key">The string key.</param>
        /// <param name="value">The value.</param>
        /// <param name="slidingExpiration">Sliding Expiration.</param>
        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            cacheItemPolicy.SlidingExpiration = slidingExpiration;

            this.Set<T>(key, value, cacheItemPolicy);
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The string key.</param>
        public void Remove(string key)
        {
            this.Keys.Remove(key);
            CacheInstance.Remove(key);
        }

        /// <summary>
        /// Clears all.
        /// </summary>
        public void Clear()
        {
            var keys = this.Keys.ToList<string>();

            while (this.Keys.Count > 0)
            {
                this.Remove(this.Keys.First());
            }
        }

        #endregion ICache Members

        #region IDisposable Members

        public void Dispose()
        {
            objCache = null;
            listKey = null;
        }

        #endregion IDisposable Members
    }
}
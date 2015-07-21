namespace Fanex.CustomerSetting.Common
{
    using System;
    using System.Runtime.Caching;

    /// <summary>
    /// Memory cache
    /// <see cref="http://msdn.microsoft.com/en-us/library/dd997357.aspx"/>
    /// <see cref="http://deanhume.com/Home/BlogPost/object-caching----net-4/37"/>
    /// </summary>
    public class CacheManager : ICacheManager
    {
        private static volatile ObjectCache objCache;

        private static object syncRoot = new object();

        public int Count
        {
            get
            {
                return (int)CacheInstance.GetCount();
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
                            objCache = MemoryCache.Default;
                        }
                    }
                }

                return objCache;
            }
        }

        public bool Contains(string key)
        {
            return CacheInstance.Contains(key);
        }

        public T Get<T>(string key)
        {
            return this.Contains(key) ? (T)CacheInstance[key] : default(T);
        }

        public bool TryGet<T>(string key, out T value)
        {
            bool checkTryGet = this.Contains(key);

            value = checkTryGet ? (T)Get<T>(key) : default(T);

            return checkTryGet;
        }

        public void Set<T>(string key, T value, CacheItemPolicy cacheItemPolicy)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentOutOfRangeException("key");
            }

            CacheInstance.Set(new CacheItem(key, value), cacheItemPolicy);
        }

        public void Set<T>(string key, T value, int timeOut = 120)
        {
            this.Set<T>(key, value, DateTime.Now.AddSeconds(timeOut));
        }

        public void Set<T>(string key, T value, DateTime absoluteExpiration)
        {
            this.Set<T>(key, value, new CacheItemPolicy { AbsoluteExpiration = absoluteExpiration });
        }

        public void Set<T>(string key, T value, TimeSpan slidingExpiration)
        {
            this.Set<T>(key, value, new CacheItemPolicy { SlidingExpiration = slidingExpiration });
        }

        public void Remove(string key)
        {
            CacheInstance.Remove(key);
        }

        public void Dispose()
        {
            objCache = null;
        }
    }
}
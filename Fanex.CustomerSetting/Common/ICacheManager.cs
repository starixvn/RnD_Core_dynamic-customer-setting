namespace Fanex.CustomerSetting.Common
{
    using System;

    public interface ICacheManager
    {
        int Count { get; }

        bool Contains(string key);

        T Get<T>(string key);

        bool TryGet<T>(string key, out T value);

        void Set<T>(string key, T value, int timeOut = 120);

        void Set<T>(string key, T value, DateTime absoluteExpiration);

        void Set<T>(string key, T value, TimeSpan slidingExpiration);

        void Remove(string key);
    }
}

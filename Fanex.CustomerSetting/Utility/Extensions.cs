namespace Fanex.CustomerSetting.Utility
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Extensions class.
    /// </summary>
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
    }
}
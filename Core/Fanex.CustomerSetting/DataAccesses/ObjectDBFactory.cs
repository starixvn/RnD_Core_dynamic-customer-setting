namespace Fanex.CustomerSetting.DataAccesses
{
    using Fanex.Data;
    using System;

    internal static class ObjectDbFactory
    {
        private static Func<string, IObjectDb> _objectDb;

        internal static Func<string, IObjectDb> ObjectDb
        {
            get
            {
                return _objectDb ?? ((settingKey) => CreateObjectDb(settingKey));
            }
            set
            {
                _objectDb = value;
            }
        }

        internal static IObjectDb CreateObjectDb(string settingKey) => new ObjectDb(settingKey);
    }
}

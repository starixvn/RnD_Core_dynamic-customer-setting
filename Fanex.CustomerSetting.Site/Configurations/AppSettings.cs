namespace Fanex.CustomerSetting.Site.Configurations
{
    using System;
    using System.Configuration;
    using Fanex.CustomerSetting.Enumerations;

    /// <summary>
    /// AppSettings class.
    /// </summary>
    public static class AppSettings
    {
        #region Constructors

        static AppSettings()
        {
            CustId = GetValue<int>("CustId");
            SubAccountId = GetValue<int>("SubAccountId");
            RoleType = GetValue<RoleType>("RoleType");
            Language = GetValue("Language");
        }

        #endregion Constructors

        #region Properties

        public static int CustId { get; private set; }

        public static int SubAccountId { get; private set; }

        public static RoleType RoleType { get; private set; }

        public static string Language { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Gets the setting value.
        /// </summary>
        /// <param name="key">The setting key.</param>
        /// <returns>Returns setting value by string.</returns>
        private static string GetValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToLower();
            }
            catch
            {
                var exception = new InvalidOperationException(string.Format("Key '{0}' not found in appSettings.", key));
                throw exception;
            }
        }

        /// <summary>
        /// Gets the setting value.
        /// </summary>
        /// <typeparam name="T">Type of returned object.</typeparam>
        /// <param name="key">The setting key.</param>
        /// <returns>Returns value by setting key.</returns>
        private static T GetValue<T>(string key)
        {
            var value = GetValue(key);
            Type type = typeof(T);
            if (type.IsPrimitive)
            {
                return (T)Convert.ChangeType(value, type);
            }
            else if (type.IsEnum)
            {
                return (T)Enum.ToObject(type, Convert.ToInt32(value));
            }
            else
            {
                var exception = new InvalidOperationException("Not suppored");
                throw exception;
            }
        }

        private static void ThrowInvalidValueException(string key)
        {
            var exception = new InvalidOperationException(string.Format("Invalid value of key {0}", key));
            throw exception;
        }

        #endregion Methods
    }
}
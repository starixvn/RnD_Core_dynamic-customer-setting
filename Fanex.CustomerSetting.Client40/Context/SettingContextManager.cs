namespace Fanex.CustomerSetting.Client40.Context
{
    using System;

    public static class SettingContextManager
    {
        private static SettingContext settingContext;

        public static void Register(Func<SettingContext> fetchSettingContext)
        {
            if (fetchSettingContext != null)
            {
                settingContext = fetchSettingContext.Invoke();
            }
        }

        internal static SettingContext SettingContext
        {
            get
            {
                if (settingContext != null)
                {
                    return settingContext;
                }
                else
                {
                    return new SettingContext();
                }
            }
        }
    }
}
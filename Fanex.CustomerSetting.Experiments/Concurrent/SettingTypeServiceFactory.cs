namespace Fanex.CustomerSetting.Experiments.Concurrent
{
    using System.Collections.Concurrent;

    public class SettingTypeServiceFactory
    {
        private const string CacheKey = "SettingTypeSerivce";

        private static ConcurrentDictionary<string, SettingTypeService> settingTypeServiceInstances = new ConcurrentDictionary<string, SettingTypeService>();

        public SettingTypeService GetConcurrentClass()
        {
            var settingTypeService = GetInstanceFromCache();

            if (settingTypeService == null)
            {
                settingTypeService = CreateInstanceAndCache();
            }

            return settingTypeService;
        }

        private SettingTypeService GetInstanceFromCache()
        {
            SettingTypeService settingTypeService = null;

            settingTypeServiceInstances.TryGetValue(CacheKey, out settingTypeService);

            return settingTypeService;
        }

        private SettingTypeService CreateInstanceAndCache()
        {
            var settingTypeService = new SettingTypeService();

            settingTypeServiceInstances.TryAdd(CacheKey, settingTypeService);

            return settingTypeService;
        }
    }
}
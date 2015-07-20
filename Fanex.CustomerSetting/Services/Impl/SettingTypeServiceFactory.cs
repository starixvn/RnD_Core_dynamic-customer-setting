namespace Fanex.CustomerSetting.Handlers
{
    using System;
    using System.Collections.Concurrent;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;

    public class SettingTypeServiceFactory : ISettingTypeServiceFactory
    {
        private const string AssemblyNameFormat = "Fanex.CustomerSetting.SettingType{0}.Template{1}";
        private const string TypeNameFormat = "{0}.Services.SettingTypeService";

        private static ConcurrentDictionary<string, ISettingTypeService> settingTypeServiceInstances = new ConcurrentDictionary<string, ISettingTypeService>();
        private static ConcurrentDictionary<string, string> fullTypeNameDictionary = new ConcurrentDictionary<string, string>();

        public ISettingTypeService GetInstance(SettingType settingType)
        {
            Check.Argument.IsNotNull(settingType, "settingType");

            var settingTypeService = GetInstanceFromCache(settingType);

            if (settingTypeService == null)
            {
                settingTypeService = CreateInstanceAndCache(settingType);
            }

            return settingTypeService;
        }

        private ISettingTypeService GetInstanceFromCache(SettingType settingType)
        {
            ISettingTypeService settingTypeService = null;
            var cacheKey = GetCacheKey(settingType);

            settingTypeServiceInstances.TryGetValue(cacheKey, out settingTypeService);

            return settingTypeService;
        }

        private ISettingTypeService CreateInstanceAndCache(SettingType settingType)
        {
            ISettingTypeService settingTypeService = null;
            var fullTypeName = GetFullTypeServiceName(settingType);
            Type settingTypeServiceType = Type.GetType(fullTypeName);

            if (settingTypeServiceType == null)
            {
                throw new DllNotFoundException(string.Format("{0} not found", fullTypeName));
            }

            settingTypeService = Activator.CreateInstance(settingTypeServiceType) as ISettingTypeService;

            var cacheKey = GetCacheKey(settingType);
            settingTypeServiceInstances.TryAdd(cacheKey, settingTypeService);

            return settingTypeService;
        }

        private string GetCacheKey(SettingType settingType)
        {
            var cacheKey = string.Join("_", settingType.Id, settingType.SettingTemplate);

            return cacheKey;
        }

        private string GetFullTypeServiceName(SettingType settingType)
        {
            var assemblyName = string.Format(AssemblyNameFormat, settingType.Id, settingType.SettingTemplate);
            var typeName = string.Format(TypeNameFormat, assemblyName);
            return string.Join(", ", typeName, assemblyName);
        }
    }
}
namespace Fanex.CustomerSetting.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;

    public class SettingTypeServiceFactory : ISettingTypeServiceFactory
    {
        private const string CacheKeyFormat = "{0}_{1}";
        private const string AssemblyFormat = "Fanex.CustomerSetting.SettingType{0}.Template{1}";
        private const string SettingTypeServiceFormat = "{0}.Services.SettingType{1}Service";
        private const string TypeWithAssemblyFormat = "{0}, {1}";

        private static ConcurrentDictionary<string, ISettingTypeService> _settingTypeServiceCached = new ConcurrentDictionary<string, ISettingTypeService>();
        
        public ISettingTypeService CreateInstance(SettingType settingType)
        {
            ISettingTypeService settingTypeService = null;
            var cacheKey = GenerateCacheKey(settingType);

            if (SettingTypeServiceFactory._settingTypeServiceCached.ContainsKey(cacheKey))
            {
                settingTypeService = GetSettingTypeServiceFromCache(cacheKey);
            }
            else
            {
                settingTypeService = CreateSettingTypeServiceInstanceAndCache(settingType, cacheKey);
            }

            return settingTypeService;
        }

        private ISettingTypeService GetSettingTypeServiceFromCache(string cacheKey)
        {
            return SettingTypeServiceFactory._settingTypeServiceCached[cacheKey];
        }

        private ISettingTypeService CreateSettingTypeServiceInstanceAndCache(SettingType settingType, string cacheKey)
        {
            ISettingTypeService settingTypeService = null;
            string fullTypeName = GetFullTypeName(SettingTypeServiceFormat, settingType.Id, settingType.SettingTemplate);
            Type settingTypeServiceType = Type.GetType(fullTypeName);

            if (settingTypeServiceType != null)
            {
                settingTypeService = Activator.CreateInstance(settingTypeServiceType) as ISettingTypeService;

                SettingTypeServiceFactory._settingTypeServiceCached.TryAdd(cacheKey, settingTypeService);
            }

            return settingTypeService;
        }

        private string GetFullTypeName(
            string typeNameFormat,
            short settingTypeId,
            byte settingTemplate)
        {
            string assemblyName = string.Format(AssemblyFormat, settingTypeId, settingTemplate);
            string typeName = string.Format(typeNameFormat, assemblyName, settingTypeId);
            return string.Format(TypeWithAssemblyFormat, typeName, assemblyName);
        }

        private string GenerateCacheKey(SettingType settingType)
        {
            return string.Format(CacheKeyFormat, settingType.Id, settingType.SettingTemplate);
        }
    }
}
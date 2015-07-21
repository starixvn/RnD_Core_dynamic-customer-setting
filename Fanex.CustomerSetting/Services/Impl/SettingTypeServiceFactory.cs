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

        private static ConcurrentDictionary<string, ISettingTypeService> ServiceInstanceCached = new ConcurrentDictionary<string, ISettingTypeService>();

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

            ServiceInstanceCached.TryGetValue(settingType.ToString(), out settingTypeService);

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

            ServiceInstanceCached.TryAdd(settingType.ToString(), settingTypeService);

            return settingTypeService;
        }

        private string GetFullTypeServiceName(SettingType settingType)
        {
            var assemblyName = string.Format(AssemblyNameFormat, settingType.Id, settingType.SettingTemplate);
            var typeName = string.Format(TypeNameFormat, assemblyName);
            return string.Join(", ", typeName, assemblyName);
        }
    }
}
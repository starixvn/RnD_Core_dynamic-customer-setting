namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Concurrent;
    using Common.Helper;
    using Models;

    internal class SettingTypeServiceFactory : ISettingTypeServiceFactory
    {
        private const string AssemblyNameFormat = "Fanex.CustomerSetting.SettingType{0}";
        private const string TypeNameFormat = "{0}.Services.SettingTypeService";

        private static ConcurrentDictionary<string, ISettingTypeService> serviceInstanceCached = new ConcurrentDictionary<string, ISettingTypeService>();

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

            serviceInstanceCached.TryGetValue(settingType.ToString(), out settingTypeService);

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

            serviceInstanceCached.TryAdd(settingType.ToString(), settingTypeService);

            return settingTypeService;
        }

        private string GetFullTypeServiceName(SettingType settingType)
        {
            var assemblyName = string.Format(AssemblyNameFormat, settingType.Id);
            var typeName = string.Format(TypeNameFormat, assemblyName);
            return string.Join(", ", typeName, assemblyName);
        }
    }
}
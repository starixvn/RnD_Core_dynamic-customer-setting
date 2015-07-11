namespace Fanex.CustomerSetting.Handlers
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Models;

    public class SettingResolver : ISettingResolver
    {
        private const string CacheKeyFormat = "{0}_{1}";
        private const string AssemblyFormat = "Fanex.CustomerSetting.SettingType{0}.Template{1}";
        private const string DataAccessTypeFormat = "{0}.DataAccesses.SettingType{1}DataAccess";
        private const string SettingPreTasksTypeFormat = "{0}.Handlers.SettingType{1}PreTasks";
        private const string SettingPostTasksTypeFormat = "{0}.Handlers.SettingType{1}PostTasks";
        private const string TypeWithAssemblyFormat = "{0}, {1}";

        private static Dictionary<string, ISettingTypeDataAccess> _settingDataAccessesCached = new Dictionary<string, ISettingTypeDataAccess>();
        private static Dictionary<string, SettingPreTasks> _settingPreTasksCached = new Dictionary<string, SettingPreTasks>();
        private static Dictionary<string, SettingPostTasks> _settingPostTasksCached = new Dictionary<string, SettingPostTasks>();

        public ISettingTypeDataAccess ResolveSettingDataAccess(SettingType settingType)
        {
            ISettingTypeDataAccess settingDataAccess = null;
            var cacheKey = GenerateCacheKey(settingType);
            if (SettingResolver._settingDataAccessesCached.ContainsKey(cacheKey))
            {
                settingDataAccess = GetSettingDataAccessFromCache(cacheKey);
            }
            else
            {
                settingDataAccess = CreateSettingDataAccessInstanceAndCache(settingType, cacheKey);
            }

            return settingDataAccess;
        }

        private ISettingTypeDataAccess GetSettingDataAccessFromCache(string cacheKey)
        {
            return SettingResolver._settingDataAccessesCached[cacheKey];
        }

        private ISettingTypeDataAccess CreateSettingDataAccessInstanceAndCache(SettingType settingType, string cacheKey)
        {
            ISettingTypeDataAccess settingDataAccessFactory = null;
            string fullTypeName = GetFullTypeName(DataAccessTypeFormat, settingType.Id, settingType.SettingTemplate);
            Type settingDataAccessFactoryType = Type.GetType(fullTypeName);

            if (settingDataAccessFactoryType != null)
            {
                settingDataAccessFactory = Activator.CreateInstance(settingDataAccessFactoryType) as ISettingTypeDataAccess;
                SettingResolver._settingDataAccessesCached.Add(cacheKey, settingDataAccessFactory);
            }

            return settingDataAccessFactory;
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

        public SettingPreTasks ResolveSettingPreTasks(SettingType settingType)
        {
            SettingPreTasks settingPreTasks = null;
            var cacheKey = GenerateCacheKey(settingType);
            if (SettingResolver._settingPreTasksCached.ContainsKey(cacheKey))
            {
                settingPreTasks = GetSettingPreTasksFromCache(cacheKey);
            }
            else
            {
                settingPreTasks = CreateSettingPreTasksAndCache(settingType, cacheKey);
            }

            return settingPreTasks;
        }

        private SettingPreTasks GetSettingPreTasksFromCache(string cacheKey)
        {
            return SettingResolver._settingPreTasksCached[cacheKey];
        }

        private SettingPreTasks CreateSettingPreTasksAndCache(SettingType settingType, string cacheKey)
        {
            SettingPreTasks settingPreTasks = null;
            string fullTypeName = GetFullTypeName(SettingPreTasksTypeFormat, settingType.Id, settingType.SettingTemplate);
            Type preTasksType = Type.GetType(fullTypeName);

            if (preTasksType != null)
            {
                settingPreTasks = Activator.CreateInstance(preTasksType) as SettingPreTasks;
                SettingResolver._settingPreTasksCached.Add(cacheKey, settingPreTasks);
            }

            return settingPreTasks;
        }

        public SettingPostTasks ResolveSettingPostTasks(SettingType settingType)
        {
            Check.Argument.IsNotNull(settingType, "settingType");

            SettingPostTasks settingPostTasks = null;
            var cacheKey = GenerateCacheKey(settingType);
            if (SettingResolver._settingPostTasksCached.ContainsKey(cacheKey))
            {
                settingPostTasks = GetSettingPostTasksFromCache(cacheKey);
            }
            else
            {
                settingPostTasks = CreateSettingPostTasksAndCache(settingType, cacheKey);
            }

            return settingPostTasks;
        }

        private string GenerateCacheKey(SettingType settingType)
        {
            return string.Format(CacheKeyFormat, settingType.Id, settingType.SettingTemplate);
        }

        private SettingPostTasks GetSettingPostTasksFromCache(string cacheKey)
        {
            return SettingResolver._settingPostTasksCached[cacheKey];
        }

        private SettingPostTasks CreateSettingPostTasksAndCache(SettingType settingType, string cacheKey)
        {
            SettingPostTasks settingPostTasks = null;
            string fullTypeName = GetFullTypeName(SettingPostTasksTypeFormat, settingType.Id, settingType.SettingTemplate);
            Type postTasksType = Type.GetType(fullTypeName);

            if (postTasksType != null)
            {
                settingPostTasks = Activator.CreateInstance(postTasksType) as SettingPostTasks;
                SettingResolver._settingPostTasksCached.Add(cacheKey, settingPostTasks);
            }

            return settingPostTasks;
        }
    }
}
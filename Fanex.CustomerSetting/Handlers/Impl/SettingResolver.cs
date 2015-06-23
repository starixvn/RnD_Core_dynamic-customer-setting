namespace Fanex.CustomerSetting.Handlers.Impl
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
        private const string DataAccessFactoryTypeFormat = "{0}.DataAccesses.SettingType{1}DataAccessFactory";
        private const string SettingPreTasksTypeFormat = "{0}.Handlers.SettingType{1}PreTasks";
        private const string SettingPostTasksTypeFormat = "{0}.Handlers.SettingType{1}PostTasks";
        private const string TypeWithAssemblyFormat = "{0}, {1}";
        private static Dictionary<string, ISettingDataAccessFactory> _settingDataAccessFactoriesCached = new Dictionary<string, ISettingDataAccessFactory>();
        private static Dictionary<string, SettingPreTasks> _settingPreTasksCached = new Dictionary<string, SettingPreTasks>();
        private static Dictionary<string, SettingPostTasks> _settingPostTasksListCached = new Dictionary<string, SettingPostTasks>();
        private readonly ISettingContext _settingContext;

        public SettingResolver(ISettingContext settingContext)
        {
            _settingContext = settingContext;
        }

        public ISettingDataAccessFactory ResolveSettingDataAccessFactory(SettingType settingType)
        {
            ISettingDataAccessFactory settingDataAccessFactory = null;
            var cacheKey = GenerateCacheKey(settingType);
            if (SettingResolver._settingDataAccessFactoriesCached.ContainsKey(cacheKey))
            {
                settingDataAccessFactory = GetSettingDataAccessFactoryFromCache(cacheKey);
            }
            else
            {
                settingDataAccessFactory = CreateSettingDataAccessInstanceAndCache(settingType, cacheKey);
            }

            return settingDataAccessFactory;
        }

        private ISettingDataAccessFactory GetSettingDataAccessFactoryFromCache(string cacheKey)
        {
            return SettingResolver._settingDataAccessFactoriesCached[cacheKey];
        }

        private ISettingDataAccessFactory CreateSettingDataAccessInstanceAndCache(SettingType settingType, string cacheKey)
        {
            ISettingDataAccessFactory settingDataAccessFactory = null;
            string fullTypeName = GetFullTypeName(DataAccessFactoryTypeFormat, settingType.Id, settingType.SettingTemplate);
            Type settingDataAccessFactoryType = Type.GetType(fullTypeName);

            if (settingDataAccessFactoryType != null)
            {
                settingDataAccessFactory = Activator.CreateInstance(settingDataAccessFactoryType) as ISettingDataAccessFactory;
                SettingResolver._settingDataAccessFactoriesCached.Add(cacheKey, settingDataAccessFactory);
            }

            return settingDataAccessFactory;
        }

        private string GetFullTypeName(
            string typeNameFormat,
            byte settingTypeId,
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
                settingPreTasks = GetSettingPreTasksHandlerFromCache(cacheKey);
            }
            else
            {
                settingPreTasks = CreateSettingPreTasksHandlerAndCache(settingType, cacheKey);
            }

            return settingPreTasks;
        }

        private SettingPreTasks GetSettingPreTasksHandlerFromCache(string cacheKey)
        {
            return SettingResolver._settingPreTasksCached[cacheKey];
        }

        private SettingPreTasks CreateSettingPreTasksHandlerAndCache(SettingType settingType, string cacheKey)
        {
            SettingPreTasks settingPreTasks = null;
            string fullTypeName = GetFullTypeName(SettingPreTasksTypeFormat, settingType.Id, settingType.SettingTemplate);
            Type preTasksType = Type.GetType(fullTypeName);

            if (preTasksType != null)
            {
                settingPreTasks = Activator.CreateInstance(preTasksType, _settingContext) as SettingPreTasks;
                SettingResolver._settingPreTasksCached.Add(cacheKey, settingPreTasks);
            }

            return settingPreTasks;
        }

        public SettingPostTasks ResolveSettingPostTasks(SettingType settingType)
        {
            Check.Argument.IsNotNull(settingType, "settingType");

            SettingPostTasks settingPostTasks = null;
            var cacheKey = GenerateCacheKey(settingType);
            if (SettingResolver._settingPostTasksListCached.ContainsKey(cacheKey))
            {
                settingPostTasks = GetSettingPostTasksHandlerFromCache(cacheKey);
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

        private SettingPostTasks GetSettingPostTasksHandlerFromCache(string cacheKey)
        {
            return SettingResolver._settingPostTasksListCached[cacheKey];
        }

        private SettingPostTasks CreateSettingPostTasksAndCache(SettingType settingType, string cacheKey)
        {
            SettingPostTasks settingPostTasks = null;
            string fullTypeName = GetFullTypeName(SettingPostTasksTypeFormat, settingType.Id, settingType.SettingTemplate);
            Type postTasksType = Type.GetType(fullTypeName);

            if (postTasksType != null)
            {
                settingPostTasks = Activator.CreateInstance(postTasksType, _settingContext) as SettingPostTasks;
                SettingResolver._settingPostTasksListCached.Add(cacheKey, settingPostTasks);
            }

            return settingPostTasks;
        }
    }
}
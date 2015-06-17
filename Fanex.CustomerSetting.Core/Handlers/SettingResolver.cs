namespace Fanex.CustomerSetting.Core.Handlers
{
    using System;
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Core.DataAccesses;
    using Fanex.CustomerSetting.Core.Models;

    public class SettingResolver : ISettingResolver
    {
        private const string HandlerCachedKeyFormat = "{0}_{1}";
        private const string AssemblyFormat = "Fanex.CustomerSetting.SettingType{0}.Template{1}";
        private const string DataAccessFactoryTypeFormat = "{0}.DataAccess.SettingType{1}DataAccessFactory";
        private const string SettingPreTasksTypeFormat = "{0}.Tasks.SettingType{1}PreTasks";
        private const string SettingPostTasksTypeFormat = "{0}.Tasks.SettingType{1}PostTasks";
        private const string TypeWithAssemblyFormat = "{0}, {1}";
        private static Dictionary<string, ISettingDataAccessFactory> _settingDataAccessFactories = new Dictionary<string, ISettingDataAccessFactory>();
        private static Dictionary<string, SettingPreTasks> _settingPreTasksList = new Dictionary<string, SettingPreTasks>();
        private static Dictionary<string, SettingPostTasks> _settingPostTasksList = new Dictionary<string, SettingPostTasks>();
        private readonly ISettingContext _settingContext;

        public SettingResolver()
            : this(null)
        {
        }

        public SettingResolver(ISettingContext settingContext)
        {
            _settingContext = settingContext ?? SettingContextManager.SettingContext;
        }

        public ISettingDataAccessFactory ResolveSettingDataAccessFactory(SettingType settingType)
        {
            ISettingDataAccessFactory settingDataAccessFactory = null;
            var factoryKey = string.Format(HandlerCachedKeyFormat, settingType.Id, settingType.SettingTemplate);
            if (SettingResolver._settingDataAccessFactories.ContainsKey(factoryKey))
            {
                settingDataAccessFactory = GetSettingDataAccessFactoryFromCache(factoryKey);
            }
            else
            {
                settingDataAccessFactory = CreateSettingDataAccessInstanceAndCache(settingType, factoryKey);
            }

            return settingDataAccessFactory;
        }

        private ISettingDataAccessFactory GetSettingDataAccessFactoryFromCache(string factoryKey)
        {
            return SettingResolver._settingDataAccessFactories[factoryKey];
        }

        private ISettingDataAccessFactory CreateSettingDataAccessInstanceAndCache(SettingType settingType, string factoryKey)
        {
            ISettingDataAccessFactory settingDataAccessFactory = null;
            string fullTypeName = GetFullTypeName(DataAccessFactoryTypeFormat, settingType.Id, settingType.SettingTemplate);
            Type settingDataAccessFactoryType = Type.GetType(fullTypeName);

            if (settingDataAccessFactoryType != null)
            {
                settingDataAccessFactory = Activator.CreateInstance(settingDataAccessFactoryType) as ISettingDataAccessFactory;
                SettingResolver._settingDataAccessFactories.Add(factoryKey, settingDataAccessFactory);
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
            var factoryKey = string.Format(HandlerCachedKeyFormat, settingType.Id, settingType.SettingTemplate);
            if (SettingResolver._settingPreTasksList.ContainsKey(factoryKey))
            {
                settingPreTasks = GetSettingPreTasksHandlerFromCache(factoryKey);
            }
            else
            {
                settingPreTasks = CreateSettingPreTasksHandlerAndCache(settingType, factoryKey);
            }

            return settingPreTasks;
        }

        private SettingPreTasks GetSettingPreTasksHandlerFromCache(string factoryKey)
        {
            return SettingResolver._settingPreTasksList[factoryKey];
        }

        private SettingPreTasks CreateSettingPreTasksHandlerAndCache(SettingType settingType, string factoryKey)
        {
            SettingPreTasks settingPreTasks = null;
            string fullTypeName = GetFullTypeName(SettingPreTasksTypeFormat, settingType.Id, settingType.SettingTemplate);
            Type preTasksType = Type.GetType(fullTypeName);

            if (preTasksType != null)
            {
                settingPreTasks = Activator.CreateInstance(preTasksType, _settingContext) as SettingPreTasks;
                SettingResolver._settingPreTasksList.Add(factoryKey, settingPreTasks);
            }

            return settingPreTasks;
        }

        public SettingPostTasks ResolveSettingPostTasks(SettingType settingType)
        {
            Check.Argument.IsNotNull(settingType, "settingType");

            SettingPostTasks settingPostTasks = null;
            var factoryKey = string.Format(HandlerCachedKeyFormat, settingType.Id, settingType.SettingTemplate);
            if (SettingResolver._settingPostTasksList.ContainsKey(factoryKey))
            {
                settingPostTasks = GetSettingPostTasksHandlerFromCache(factoryKey);
            }
            else
            {
                settingPostTasks = CreateSettingPostTasksAndCache(settingType, factoryKey);
            }

            return settingPostTasks;
        }

        private SettingPostTasks GetSettingPostTasksHandlerFromCache(string factoryKey)
        {
            return SettingResolver._settingPostTasksList[factoryKey];
        }

        private SettingPostTasks CreateSettingPostTasksAndCache(SettingType settingType, string factoryKey)
        {
            SettingPostTasks settingPostTasks = null;
            string fullTypeName = GetFullTypeName(SettingPostTasksTypeFormat, settingType.Id, settingType.SettingTemplate);
            Type postTasksType = Type.GetType(fullTypeName);

            if (postTasksType != null)
            {
                settingPostTasks = Activator.CreateInstance(postTasksType, _settingContext) as SettingPostTasks;
                SettingResolver._settingPostTasksList.Add(factoryKey, settingPostTasks);
            }

            return settingPostTasks;
        }
    }
}
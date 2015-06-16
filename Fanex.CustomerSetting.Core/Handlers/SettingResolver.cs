namespace Fanex.CustomerSetting.Core.Handlers
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.DataAccesses;
    using Fanex.CustomerSetting.Core.Dtos.Messages;
    using Fanex.CustomerSetting.Core.Models;

    public class SettingResolver : ISettingResolver
    {
        private const string HandlerKeyFormat = "{0}_{1}";
        private const string AssemblyFormat = "Fanex.CustomerSetting.SettingType{0}.Template{1}";
        private const string DataAccessFactoryTypeFormat = "{0}.DataAccess.SettingType{1}DataAccessFactory";
        private const string SettingPreTasksTypeFormat = "{0}.Tasks.SettingType{1}PreTasks";
        private const string SettingPostTasksTypeFormat = "{0}.Tasks.SettingType{1}PostTasks";
        private const string TypeWithAssemblyFormat = "{0}, {1}";
        private static Dictionary<string, ISettingDataAccessFactory> _settingDataAccessFactories = new Dictionary<string, ISettingDataAccessFactory>();
        private static Dictionary<string, SettingPreTasks> _settingPreTasksList = new Dictionary<string, SettingPreTasks>();
        private static Dictionary<string, SettingPostTasks> _settingPostTasksList = new Dictionary<string, SettingPostTasks>();

        public ISettingDataAccessFactory ResolveSettingDataAccessFactory(SettingType settingType)
        {
            ISettingDataAccessFactory settingDataAccessFactory = null;
            var factoryKey = string.Format(HandlerKeyFormat, settingType.Id, settingType.SettingTemplate);
            if (SettingResolver._settingDataAccessFactories.ContainsKey(factoryKey))
            {
                settingDataAccessFactory = SettingResolver._settingDataAccessFactories[factoryKey];
            }
            else
            {
                string fullTypeName = GetFullTypeName(DataAccessFactoryTypeFormat, settingType.Id, settingType.SettingTemplate);
                Type settingDataAccessFactoryType = Type.GetType(fullTypeName);

                if (settingDataAccessFactoryType != null)
                {
                    settingDataAccessFactory = Activator.CreateInstance(settingDataAccessFactoryType) as ISettingDataAccessFactory;
                    SettingResolver._settingDataAccessFactories.Add(factoryKey, settingDataAccessFactory);
                }
            }

            return settingDataAccessFactory;
        }

        private static string GetFullTypeName(
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
            var factoryKey = string.Format(HandlerKeyFormat, settingType.Id, settingType.SettingTemplate);
            if (SettingResolver._settingPreTasksList.ContainsKey(factoryKey))
            {
                settingPreTasks = SettingResolver._settingPreTasksList[factoryKey];
            }
            else
            {
                string fullTypeName = GetFullTypeName(SettingPreTasksTypeFormat, settingType.Id, settingType.SettingTemplate);
                Type preTasksType = Type.GetType(fullTypeName);

                if (preTasksType != null)
                {
                    settingPreTasks = Activator.CreateInstance(preTasksType) as SettingPreTasks;
                    SettingResolver._settingPreTasksList.Add(factoryKey, settingPreTasks);
                }
            }

            return settingPreTasks;
        }

        public SettingPostTasks ResolveSettingPostTasks(SettingType settingType)
        {
            SettingPostTasks settingPostTasks = null;
            var factoryKey = string.Format(HandlerKeyFormat, settingType.Id, settingType.SettingTemplate);
            if (SettingResolver._settingPostTasksList.ContainsKey(factoryKey))
            {
                settingPostTasks = SettingResolver._settingPostTasksList[factoryKey];
            }
            else
            {
                string fullTypeName = GetFullTypeName(SettingPostTasksTypeFormat, settingType.Id, settingType.SettingTemplate);
                Type postTasksType = Type.GetType(fullTypeName);

                if (postTasksType != null)
                {
                    settingPostTasks = Activator.CreateInstance(postTasksType) as SettingPostTasks;
                    SettingResolver._settingPostTasksList.Add(factoryKey, settingPostTasks);
                }
            }

            return settingPostTasks;
        }
    }
}
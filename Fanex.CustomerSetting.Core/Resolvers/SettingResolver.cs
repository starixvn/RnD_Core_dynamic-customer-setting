namespace Fanex.CustomerSetting.Core.Resolvers
{
    using System;
    using Fanex.CustomerSetting.Core.DataAccesses;
    using Fanex.CustomerSetting.Core.Messages;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Tasks;

    public class SettingResolver : ISettingResolver
    {
        public ISettingDataAccessFactory ResolveSettingDataAccessFactory(SettingType settingType)
        {
            throw new NotImplementedException();
        }

        public SettingPreTasks ResolveSettingPreTasks(SettingType settingType)
        {
            throw new NotImplementedException();
        }

        public SettingPostTasks ResolveSettingPostTasks(SettingType settingType)
        {
            throw new NotImplementedException();
        }

        public IMessageBuilder ResolveMessageBuilder(SettingType settingType)
        {
            throw new NotImplementedException();
        }
    }
}
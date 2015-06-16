namespace Fanex.CustomerSetting.Core.Handlers
{
    using Fanex.CustomerSetting.Core.DataAccesses;
    using Fanex.CustomerSetting.Core.Dtos.Messages;
    using Fanex.CustomerSetting.Core.Models;

    public interface ISettingResolver
    {
        ISettingDataAccessFactory ResolveSettingDataAccessFactory(SettingType settingType);

        SettingPreTasks ResolveSettingPreTasks(SettingType settingType);

        SettingPostTasks ResolveSettingPostTasks(SettingType settingType);

        IMessageBuilder ResolveMessageBuilder(SettingType settingType);

        ITranslator ResolverTranslator(SettingType settingType);
    }
}
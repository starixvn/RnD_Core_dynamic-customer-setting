namespace Fanex.CustomerSetting.Core.Handlers
{
    using Fanex.CustomerSetting.Core.DataAccesses;
    using Fanex.CustomerSetting.Core.Messages;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Tasks;

    public interface ISettingResolver
    {
        ISettingDataAccessFactory ResolveSettingDataAccessFactory(SettingType settingType);

        SettingPreTasks ResolveSettingPreTasks(SettingType settingType);

        SettingPostTasks ResolveSettingPostTasks(SettingType settingType);

        IMessageBuilder ResolveMessageBuilder(SettingType settingType);

        ITranslator ResolverTranslator(SettingType settingType);
    }
}
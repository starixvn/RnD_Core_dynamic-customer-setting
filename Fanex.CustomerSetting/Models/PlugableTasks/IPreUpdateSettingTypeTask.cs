namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Enumerations;

    internal interface IPreUpdateSettingTypeTask
    {
        void Invoke(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingTypeData);
    }
}
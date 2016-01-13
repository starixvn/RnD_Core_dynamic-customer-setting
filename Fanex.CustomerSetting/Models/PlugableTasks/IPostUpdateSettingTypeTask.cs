namespace Fanex.CustomerSetting.Models.PlugableTasks
{
    using Enumerations;
    using Results;

    internal interface IPostUpdateSettingTypeTask
    {
        void Invoke(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingTypeData,
            SettingResult updateResult);
    }
}
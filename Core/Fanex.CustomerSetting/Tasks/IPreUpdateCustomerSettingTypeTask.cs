namespace Fanex.CustomerSetting.Tasks
{
    using Models;
    using Models.Enumerations;

    internal interface IPreUpdateCustomerSettingTypeTask
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
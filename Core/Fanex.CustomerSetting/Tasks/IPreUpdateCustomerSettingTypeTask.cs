namespace Fanex.CustomerSetting.Tasks
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;

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
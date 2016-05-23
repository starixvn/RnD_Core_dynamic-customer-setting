namespace Fanex.CustomerSetting.Tasks
{
    using Models;
    using Models.Enumerations;
    using Models.Results;

    internal interface IPostUpdateCustomerSettingTypeTask
    {
        void Invoke(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingTypeData,
            IResult updateResult);
    }
}
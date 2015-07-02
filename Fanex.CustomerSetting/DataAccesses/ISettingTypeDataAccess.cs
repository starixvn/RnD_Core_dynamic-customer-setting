namespace Fanex.CustomerSetting.DataAccesses
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public interface ISettingTypeDataAccess
    {
        SettingType GetCustomerSettingType(User actor, QuerySettingParam queryParam);

        IResult UpdateCustomerSettingType(ISettingContext settingContext, Customer customer, SettingType settingType, byte productId, CustomerLevel? uplineLevel);
    }
}
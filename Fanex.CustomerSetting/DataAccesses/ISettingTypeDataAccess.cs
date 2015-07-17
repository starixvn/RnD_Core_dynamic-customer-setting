namespace Fanex.CustomerSetting.DataAccesses
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public interface ISettingTypeDataAccess
    {
        SettingType GetCustomerSettingType(User actor, QuerySettingParam querySettingParam);

        IResult UpdateCustomerSettingType(ISettingContext settingContext, int customerId, Level customerLevel, SettingType settingType, byte productId, Level uplineLevel);
    }
}
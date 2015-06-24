namespace Fanex.CustomerSetting.DataAccesses
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public interface ISettingDataAccess
    {
        SettingType GetCustomerSettingType(User actor, QuerySettingParams settingQuery);

        IResult UpdateCustomerSetting(ISettingContext settingContext, Customer customer, SettingType settingType, CustomerLevel? uplineLevel);
    }
}
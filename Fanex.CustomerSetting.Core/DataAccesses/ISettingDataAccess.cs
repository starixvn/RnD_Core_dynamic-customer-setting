namespace Fanex.CustomerSetting.Core.DataAccesses
{
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Models;

    public interface ISettingDataAccess
    {
        SettingType GetCustomerSettingType(User actor, SettingQuery settingQuery);

        IResult UpdateCustomerSetting(ISettingContext settingContext, Customer customer, SettingType settingType, CustomerLevel? uplineLevel);
    }
}
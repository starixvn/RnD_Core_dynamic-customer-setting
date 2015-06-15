namespace Fanex.CustomerSetting.Core.DataAccesses
{
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Models.Params;
    using Fanex.CustomerSetting.Core.Results;

    public interface ISettingDataAccess
    {
        SettingType GetCustomerSettingType(User actor, SettingQuery settingQuery);

        IResult UpdateCustomerSetting(UpdateCustomerSettingParams updateCustomerSettingParams);
    }
}
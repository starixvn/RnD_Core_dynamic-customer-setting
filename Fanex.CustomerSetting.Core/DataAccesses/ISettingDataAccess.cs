namespace Fanex.CustomerSetting.Core.DataAccesses
{
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;

    public interface ISettingDataAccess
    {
        SettingType GetCustomerSettingType(User actor, SettingQuery settingQuery);

        IResult UpdateCustomerSetting(UpdateCustomerSettingParams updateCustomerSettingParams);
    }
}
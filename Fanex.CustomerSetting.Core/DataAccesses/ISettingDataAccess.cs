namespace Fanex.CustomerSetting.Core.DataAccesses
{
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

    public interface ISettingDataAccess
    {
        SettingType GetCustomerSettingType(User actor, SettingQuery settingQuery);

        IResult UpdateCustomerSetting(UpdateCustomerSettingParams updateCustomerSettingParams);
    }
}
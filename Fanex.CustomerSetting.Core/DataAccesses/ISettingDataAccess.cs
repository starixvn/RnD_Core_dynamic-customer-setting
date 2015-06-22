namespace Fanex.CustomerSetting.Core.DataAccesses
{
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Models;

    public interface ISettingDataAccess
    {
        SettingType GetCustomerSettingType(GetCustomerSettingTypeParams getParams);

        IResult UpdateCustomerSetting(UpdateCustomerSettingParams updateParams);
    }
}
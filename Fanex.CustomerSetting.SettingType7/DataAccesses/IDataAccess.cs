namespace Fanex.CustomerSetting.SettingType7.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType7.Dtos;

    internal interface IDataAccess
    {
        IEnumerable<StakeLevelDto> GetCustomerStakeLevelRawData(CustomerSettingQuery query);

        IResult UpdateCustomerStakeLevel(ISettingContext settingContext, int customerId, Level customerLevel, Level uplineLevel, string stakeLevelXml);
    }
}
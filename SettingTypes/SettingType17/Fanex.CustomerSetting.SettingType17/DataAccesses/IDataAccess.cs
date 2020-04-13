namespace Fanex.CustomerSetting.SettingType17.DataAccesses
{
    using System.Collections.Generic;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Dtos;

    internal interface IDataAccess
    {
        IEnumerable<StakeLevelDto> GetCustomerStakeLevelRawData(CustomerSettingQuery query);

        IResult UpdateCustomerStakeLevel(ISettingContext settingContext, int customerId, Level customerLevel, Level uplineLevel, string stakeLevelXml);
    }
}
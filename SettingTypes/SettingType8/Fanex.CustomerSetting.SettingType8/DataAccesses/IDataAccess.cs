namespace Fanex.CustomerSetting.SettingType8.DataAccesses
{
    using System.Collections.Generic;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Dtos;

    internal interface IDataAccess
    {
        IEnumerable<StakeLevelGroupDto> GetCustomerStakeLevelGroups(CustomerSettingQuery query);

        IEnumerable<StakeLevelDetailDto> GetCustomerStakeLevelGroupsDetails(
            CustomerSettingQuery query,
            short sportTypeId,
            out int numberOfDetailedItems,
            bool showAll = true);

        IResult UpdateCustomerStakeLevelGroups(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string stakeLevelGroupXml);
    }
}
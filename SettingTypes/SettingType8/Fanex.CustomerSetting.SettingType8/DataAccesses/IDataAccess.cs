namespace Fanex.CustomerSetting.SettingType8.DataAccesses
{
    using System.Collections.Generic;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Results;
    using Dtos;

    internal interface IDataAccess
    {
        IEnumerable<StakeLevelGroupDto> GetCustomerStakeLevelGroups(int customerId, byte productId, byte settingStatus);

        IEnumerable<StakeLevelDetailDto> GetCustomerStakeLevelGroupsDetails(
            int customerId,
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
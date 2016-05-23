namespace Fanex.CustomerSetting.SettingType1.DataAccesses
{
    using System.Collections.Generic;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;

    internal interface IDataAccess
    {
        SettingType GetCustomerPT(CustomerSettingQuery query);

        IResult UpdateCustomerPT(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string positionTakingXml);

        IEnumerable<int> GetGroupedMemberIds(string memberIds);

        IResult UpdateGroupedMembersSetting(
            int actorId,
            int subAccountId,
            string groupedMemberIds,
            Level uplineLevel,
            string positionTakingXml,
            string ip);
    }
}
namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Params;
    using Fanex.CustomerSetting.Results;

    public interface IDataAccess
    {
        SettingType GetCustomerPT(QueryCustomerSettingParam queryParam);

        IResult UpdateCustomerPT(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
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

namespace Fanex.CustomerSetting.SettingType2.Template1.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType2.Template1.Dtos;

    public interface IDataAccess
    {
        IEnumerable<BetLimitItemDto> GetCustomerBetLimitItemsRawData(CustomerSettingQuery query);

        IResult UpdateCustomerBetLimit(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            string betLimitXml);
    }
}
namespace Fanex.CustomerSetting.SettingType2.Template1.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Requests;
    using Fanex.CustomerSetting.Results;
    using Fanex.CustomerSetting.SettingType2.Template1.Dtos;

    public interface IDataAccess
    {
        IEnumerable<BetLimitItemDto> GetCustomerBetLimitItemsRawData(CustomerSettingQuery queryParam);

        IResult UpdateCustomerBetLimit(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            string betLimitXml);
    }
}
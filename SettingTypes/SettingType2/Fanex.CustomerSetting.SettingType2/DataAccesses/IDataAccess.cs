namespace Fanex.CustomerSetting.SettingType2.DataAccesses
{
    using System.Collections.Generic;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Dtos;

    internal interface IDataAccess
    {
        IEnumerable<BetLimitItemDto> GetCustomerBetLimitItemsRawData(CustomerSettingQuery query);

        IResult UpdateCustomerBetLimit(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string betLimitXml);
    }
}
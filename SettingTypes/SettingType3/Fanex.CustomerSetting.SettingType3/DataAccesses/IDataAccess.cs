namespace Fanex.CustomerSetting.SettingType3.DataAccesses
{
    using System.Collections.Generic;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Dtos;

    internal interface IDataAccess
    {
        IEnumerable<CommissionItemDto> GetCustomerCommissionItemsRawData(CustomerSettingQuery query);

        IResult UpdateCustomerCommission(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string commissionXml);
    }
}
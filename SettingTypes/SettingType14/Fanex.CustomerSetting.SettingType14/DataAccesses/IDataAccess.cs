namespace Fanex.CustomerSetting.SettingType14.DataAccesses
{
    using System.Collections.Generic;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using Dtos;

    internal interface IDataAccess
    {
        IEnumerable<TicketCommissionDto> GetCustomerTicketCommissionRawData(CustomerSettingQuery query);

        IResult UpdateCustomerTicketCommission(
            ISettingContext settingContext,
            int customerId,
            string commissionXml);
    }
}
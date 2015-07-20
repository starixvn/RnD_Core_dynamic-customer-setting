namespace Fanex.CustomerSetting.SettingType3.Template1.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType3.Template1.Dtos;

    public interface IDataAccess
    {
        IEnumerable<CommissionItemDto> GetCustomerCommissionItemsRawData(QuerySettingParam queryParam);

        IResult UpdateCustomerCommission(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            string commissionXml);
    }
}
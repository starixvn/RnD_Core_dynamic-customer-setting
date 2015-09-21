namespace Fanex.CustomerSetting.SettingType4.DataAccesses
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType4.Dtos;

    internal interface IDataAccess
    {
        IEnumerable<ProductStatusItemDto> GetCustomerProductStatusItemsRawData(CustomerSettingQuery query);

        IResult UpdateCustomerProductStatus(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            string productStatusXml);
    }
}
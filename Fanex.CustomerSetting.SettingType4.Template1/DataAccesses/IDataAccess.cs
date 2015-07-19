namespace Fanex.CustomerSetting.SettingType4.Template1.DataAccesses
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public interface IDataAccess
    {
        SettingType GetCustomerProductStatus(QuerySettingParam queryParam);

        IResult UpdateCustomerProductStatus(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            string productStatusXml);
    }
}
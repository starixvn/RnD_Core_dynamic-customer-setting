namespace Fanex.CustomerSetting.WebAPI.Models.Request
{
    using Fanex.CustomerSetting.Models.Requests;

    public class QuerySettingRequest : SettingRequest
    {
        internal CustomerSettingQuery GetCustomerSettingQuery()
        {
            var query = new CustomerSettingQuery(
             this.CustomerId,
             this.CustomerLevel,
             this.ProductId,
             this.UplineLevel);

            return query;
        }
    }
}
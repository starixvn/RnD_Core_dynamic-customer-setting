namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Requests;

    public interface IQuerySettingService
    {
        ISetting GetCustomerSetting(CustomerSettingQuery query);
    }
}

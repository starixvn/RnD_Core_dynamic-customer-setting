namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;

    public interface IQuerySettingService
    {
        Setting GetCustomerSetting(CustomerSettingQuery query);
    }
}

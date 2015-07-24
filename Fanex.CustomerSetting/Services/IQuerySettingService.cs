namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Params;

    public interface IQuerySettingService
    {
        ISetting GetCustomerSetting(QueryCustomerSettingParam queryParam);
    }
}

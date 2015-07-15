namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Models;

    public interface IQuerySettingService
    {
        ISetting GetCustomerSetting(QuerySettingParam queryParam);
    }
}

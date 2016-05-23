namespace Fanex.CustomerSetting.Services
{
    using Models;
    using Models.Requests;

    public interface IQuerySettingService : ISettingService
    {
        ISetting GetCustomerSetting(CustomerSettingQuery query);
    }
}
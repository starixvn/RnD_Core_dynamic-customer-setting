namespace Fanex.CustomerSetting.Tasks.Product18.Services
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;
    public interface ITasksProduct18Service
    {
        string GetProductsName(ISettingContext settingContext, CustomerSettingQuery query,ISetting setting);
    }
}
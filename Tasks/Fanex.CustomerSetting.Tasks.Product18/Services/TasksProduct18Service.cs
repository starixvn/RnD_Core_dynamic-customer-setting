namespace Fanex.CustomerSetting.Tasks.Product18.Services
{
    using DataAccesses;
    using System.Linq;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;

    public class TasksProduct18Service : ITasksProduct18Service
    {
        private const string DefaultLanguage = "en-US";
        private readonly IDataAccess _productsDataAccess;
        private readonly IResourceService _resourceService;

        internal TasksProduct18Service()
        {
            _productsDataAccess = new DataAccess();
            _resourceService = new ResourceService();
        }

        public string GetProductsName(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            var productsRawData = _productsDataAccess.GetProducts(
                setting.CustomerInfo.Currency,
                query.CustomerId,
                query.SettingStatus,
                setting.CustomerInfo.Site
            );
            var productsName = productsRawData
                .Select(product => _resourceService.GetProductName(product.ProductID, DefaultLanguage)).ToList();
            return string.Join(", ", productsName);
        }
    }
}
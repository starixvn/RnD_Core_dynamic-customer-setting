namespace Fanex.CustomerSetting.NumberGames2.Services.Impl
{
    using System.Linq;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.NumberGames2.Resources;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;

    public class QueryNumberGames2SettingService : QuerySettingService
    {
        public QueryNumberGames2SettingService(ISettingContext settingContext)
            : base(settingContext)
        {
        }

        public QueryNumberGames2SettingService(ISettingContext settingContext, IResourceService resourceService, ITaskService pluggableTaskService)
            : base(settingContext, resourceService: resourceService, pluggableTaskService: pluggableTaskService)
        {
        }

        public override ISetting GetCustomerSetting(CustomerSettingQuery query)
        {
            Validate(query);

            // Run plugged tasks before querying data.
            TaskService.Run(query.ProductId, query);

            const short betSettingSettingTypeId = 11;

            var customerInfo = GetCustomerInfo(query.CustomerId);
            var product = GetProductInfo(query.ProductId, SettingContext.System);
            var customerSettingTypes = GetCustomerSettingTypes(query, product.SettingTypes);

            var setting = new Setting(
                query.CustomerId,
                query.CustomerLevel,
                customerInfo,
                product.Id,
                product.Name,
                customerSettingTypes);

            if (customerSettingTypes.Any(settingType => settingType.Id == betSettingSettingTypeId))
            {
                setting.AddWarningMessages(GenerateBetSettingReferenceNote());
            }

            // Run plugged tasks after querying setting data of a product.
            TaskService.Run(query.ProductId, query, setting);

            return setting;
        }

        private string GenerateBetSettingReferenceNote()
        {
            var betTypeIds = new short[] { 2502, 2523, 2534, 2525, 2526, 2527, 2504, 2528 };
            var betTypeNames = betTypeIds.Select(betTypeId => ResourceService.GetBetTypeName(betTypeId, SettingContext.Language)).ToArray();

            return string.Format(Messages.BetSettingReferenceNote, betTypeNames);
        }
    }
}
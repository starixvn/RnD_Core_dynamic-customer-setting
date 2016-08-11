namespace Fanex.CustomerSetting.NumberGames2.Services.Impl
{
    using CustomerSetting.Models;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Data.Repository;
    using Logging;

    public class QueryNumberGames2SettingService : QuerySettingService
    {
        public QueryNumberGames2SettingService(
            ISettingContext settingContext,
            IValidationService validationService = null,
            IProductService productService = null,
            IResourceService resourceService = null,
            ITaskService pluggableTaskService = null,
            ILogger logger = null,
            ISettingTypeServiceFactory settingTypeServiceFactory = null,
            IDynamicRepository dynamicRepository = null)
            : base(
                  settingContext,
                  validationService,
                  productService,
                  resourceService,
                  pluggableTaskService,
                  logger,
                  settingTypeServiceFactory,
                  dynamicRepository)
        {
        }

        public QueryNumberGames2SettingService(ISettingContext settingContext)
            : this(settingContext, null, null, null, null, null, null, null)
        {
        }
    }
}
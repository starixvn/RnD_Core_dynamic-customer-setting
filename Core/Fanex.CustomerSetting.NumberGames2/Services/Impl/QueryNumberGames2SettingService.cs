namespace Fanex.CustomerSetting.NumberGames2.Services.Impl
{
    using System.Linq;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Data.Repository;
    using Logging;
    using Resources;

    public class QueryNumberGames2SettingService : QuerySettingService
    {
        internal const short NumberGames2ProductStatusSettingType = 11;
        internal const int ProductStatusNoteForExternalSitesCode = -9102;

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

        public override ISetting GetCustomerSetting(CustomerSettingQuery query)
        {
            ISetting setting = base.GetCustomerSetting(query);

            if (setting != null
                && ContainsSettingType11OnExternalSites(SettingContext, setting)
                && CheckIfEditingCustomerIsMasterOrAgent(query))
            {
                setting.WarningMessage = new Result(ProductStatusNoteForExternalSitesCode, Messages.ProductStatusNoteForExternalSites);
            }

            return setting;
        }

        private bool ContainsSettingType11OnExternalSites(ISettingContext settingContext, ISetting setting)
        {
            if (settingContext.System == (byte)SystemType.Nap)
            {
                return false;
            }

            return setting.SettingTypes.Any(settingType => settingType.Id == NumberGames2ProductStatusSettingType);
        }

        private bool CheckIfEditingCustomerIsMasterOrAgent(CustomerSettingQuery query)
            => query.CustomerLevel.IsMasterLevel() || query.CustomerLevel.IsAgentLevel();
    }
}
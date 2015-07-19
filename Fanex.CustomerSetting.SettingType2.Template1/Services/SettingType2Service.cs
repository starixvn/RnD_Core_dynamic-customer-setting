namespace Fanex.CustomerSetting.SettingType2.Template1.Services
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType2.Template1.DataAccesses;
    using Fanex.Logging;

    public class SettingType2Service : SettingTypeService, ISettingTypeService
    {
        private readonly IDataAccess _betLimitDataAccess;

        public SettingType2Service()
            : this(null, null, null, null)
        {
        }

        public SettingType2Service(
            IMessageService messageService,
            IResourceService resourceService,
            ILogger logger,
            IDataAccess betLimitDataAccess)
            : base(resourceService, logger)
        {
            MessageService = messageService ?? new MessageService(ResourceService);
            _betLimitDataAccess = betLimitDataAccess ?? new DataAccess();
        }

        public override SettingType GetCustomerSettingType(ISettingContext settingContext, QuerySettingParam queryParam, SettingType productSettingType)
        {
            var customerBetLimit = _betLimitDataAccess.GetCustomerBetLimit(queryParam);

            return SetSportTypeBetTypeName(customerBetLimit, settingContext.Language);
        }

        public override SettingResult UpdateCustomerSettingType(ISettingContext settingContext, int customerId, Level customerLevel, Level uplineLevel, byte productId, SettingType settingType)
        {
            var result = _betLimitDataAccess.UpdateCustomerBetLimit(settingContext, customerId, customerLevel, uplineLevel, productId, settingType.ToXml());

            var settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(settingResult, Message.UpdateBetSettingSuccessfully, settingType.SettingLevel, settingContext.Language);
        }
    }
}
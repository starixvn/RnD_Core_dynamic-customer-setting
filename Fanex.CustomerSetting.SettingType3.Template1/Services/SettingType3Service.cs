namespace Fanex.CustomerSetting.SettingType3.Template1.Services
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType3.Template1.DataAccesses;
    using Fanex.Logging;

    public class SettingType3Service : BaseSettingTypeService, ISettingTypeService
    {
        private readonly ICommissionDataAccess _commissionDataAccess;

        public SettingType3Service()
            : this(null, null, null, null)
        {
        }

        public SettingType3Service(
            IMessageService messageService,
            IResourceService resourceService,
            ILogger logger,
            ICommissionDataAccess commissionDataAccess)
            : base(resourceService, logger)
        {
            MessageService = messageService ?? new CommissionMessageService(ResourceService);
            _commissionDataAccess = commissionDataAccess ?? new CommissionDataAccess();
        }

        public override SettingType GetCustomerSettingType(ISettingContext settingContext, QuerySettingParam queryParam, SettingType productSettingType)
        {
            var customerCommission = _commissionDataAccess.GetCustomerCommission(queryParam);

            return SetSportTypeBetTypeName(customerCommission, settingContext.Language);
        }

        public override SettingResult UpdateCustomerSettingType(ISettingContext settingContext, int customerId, Enumerations.Level customerLevel, Level uplineLevel, byte productId, SettingType settingType)
        {
            var result = _commissionDataAccess.UpdateCustomerCommission(settingContext, customerId, customerLevel, uplineLevel, productId, settingType.ToXml());

            var settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(settingResult, Message.UpdateCommissionSuccessfully, settingType.SettingLevel, settingContext.Language);
        }
    }
}
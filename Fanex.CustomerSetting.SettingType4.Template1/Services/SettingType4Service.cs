namespace Fanex.CustomerSetting.SettingType4.Template1.Services
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType4.Template1.DataAccesses;
    using Fanex.Logging;

    public class SettingType4Service : BaseSettingTypeService, ISettingTypeService
    {
        private readonly IProductStatusDataAccess _productStatusDataAccess;

        public SettingType4Service()
            : this(null, null, null, null)
        {
        }

        public SettingType4Service(
            IMessageService messageService,
            IResourceService resourceService,
            ILogger logger,
            IProductStatusDataAccess productDataAccess)
            : base(resourceService, logger)
        {
            MessageService = messageService ?? new ProductStatusMessageService(ResourceService);
            _productStatusDataAccess = productDataAccess ?? new ProductStatusDataAccess();
        }

        public override SettingType GetCustomerSettingType(ISettingContext settingContext, QuerySettingParam queryParam, SettingType productSettingType)
        {
            if (queryParam.CustomerLevel.IsMemberLevel
                && !queryParam.UplineLevel.IsDirectUplineLevelOf(queryParam.CustomerLevel))
            {
                return new SettingType();
            }

            var customerBetLimit = _productStatusDataAccess.GetCustomerProductStatus(queryParam);

            return SetSportTypeBetTypeName(customerBetLimit, settingContext.Language);
        }

        public override SettingResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingType)
        {
            var result = _productStatusDataAccess.UpdateCustomerProductStatus(settingContext, customerId, customerLevel, uplineLevel, productId, settingType.ToXml());

            var settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(settingResult, Message.UpdateProductStatusSuccessfully, settingType.SettingLevel, settingContext.Language);
        }
    }
}
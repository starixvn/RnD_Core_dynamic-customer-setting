namespace Fanex.CustomerSetting.SettingType4.Template1.Services
{
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType4.Template1.DataAccesses;
    using Fanex.CustomerSetting.SettingType4.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType4.Template1.Models;
    using Fanex.Logging;

    public class SettingTypeService : SettingTypeServiceBase, ISettingTypeService
    {
        private readonly IDataAccess _productStatusDataAccess;

        public SettingTypeService()
            : this(null, null, null, null)
        {
        }

        public SettingTypeService(
            IMessageService messageService,
            IResourceService resourceService,
            ILogger logger,
            IDataAccess productDataAccess)
            : base(resourceService, logger)
        {
            MessageService = messageService ?? new MessageService(ResourceService);
            _productStatusDataAccess = productDataAccess ?? new DataAccess();
        }

        public override SettingType GetCustomerSettingType(
            ISettingContext settingContext, 
            CustomerSettingQuery query)
        {
            if (query.CustomerLevel.IsMemberLevel()
                && query.UplineLevel.IsNotAgentLevel())
            {
                return new SettingType();
            }

            var productStatusItemsRawData = _productStatusDataAccess.GetCustomerProductStatusItemsRawData(query);

            var customerBetLimit = new SettingType
            {
                Name = Label.ProductStatus,
                SettingItems = productStatusItemsRawData
                .Select<ProductStatusItemDto, ProductStatusItem>(productStatusItemDto => new ProductStatusItem(productStatusItemDto))
            };

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
            Check.Argument.IsNotNull(settingType, "settingType");

            var result = _productStatusDataAccess.UpdateCustomerProductStatus(
                settingContext, 
                customerId, 
                customerLevel, 
                uplineLevel, 
                settingType.ToXml());

            var settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(
                settingResult, 
                Message.UpdateProductStatusSuccessfully, 
                settingType.SettingLevel, 
                settingContext.Language);
        }
    }
}
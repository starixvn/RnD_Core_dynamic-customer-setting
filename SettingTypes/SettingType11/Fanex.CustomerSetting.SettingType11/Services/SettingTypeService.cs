namespace Fanex.CustomerSetting.SettingType11.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using DataAccesses;
    using Dtos;
    using Logging;
    using Models;
    using Resources;

    public class SettingTypeService : SettingTypeServiceBase, ISettingTypeService
    {
        private readonly IDataAccess _productStatusDataAccess;

        public SettingTypeService()
            : this(null, null, null, null)
        {
        }

        internal SettingTypeService(
            IMessageService messageService,
            IResourceService resourceService,
            ILogger logger,
            IDataAccess productDataAccess)
            : base(resourceService, logger)
        {
            MessageService = messageService ?? new MessageService(ResourceService);
            _productStatusDataAccess = productDataAccess ?? new DataAccess();
        }

        public override SettingType GetCustomerSettingType(ISettingContext settingContext, CustomerSettingQuery query)
        {
            IEnumerable<ProductStatusItemDto> productStatusItemsRawData = _productStatusDataAccess.GetCustomerProductStatus(query);
            var customerProductStatus = new SettingType
            {
                Name = Label.ProductStatus,
                SettingItems = productStatusItemsRawData.Select(productStatusItemDto => new ProductStatusItem(productStatusItemDto))
            };

            SetSettingTypeReadOnlyWhenIndirectUplineEditMember(customerProductStatus, query);

            return SetSportTypeBetTypeName(customerProductStatus, settingContext.Language);
        }

        public override SettingResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingType)
        {
            Check.Argument.IsNotNull(settingType, nameof(settingType));

            IResult result = _productStatusDataAccess.UpdateCustomerProductStatus(
                settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                settingType.ToXml());
            SettingResult settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(
                settingResult,
                Message.UpdateProductStatusSuccessfully,
                settingType.SettingLevel,
                settingContext.Language);
        }
    }
}
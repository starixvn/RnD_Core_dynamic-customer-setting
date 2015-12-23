namespace Fanex.CustomerSetting.SettingType7.Services
{
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
        private readonly IDataAccess _chargedCommissionDataAccess;

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
            _chargedCommissionDataAccess = productDataAccess ?? new DataAccess();
        }

        public override SettingType GetCustomerSettingType(
            ISettingContext settingContext,
            CustomerSettingQuery query)
        {
            var chargedCommissionRawData = _chargedCommissionDataAccess.GetCustomerChargedCommissionRawData(query);
            var customerChargedCommission = new SettingType
            {
                Name = Label.ChargedCommission,
                SettingItems = chargedCommissionRawData.Select<ChargedCommissionDto, ChargedCommissionItem>(
                    productStatusItemDto => new ChargedCommissionItem(productStatusItemDto))
            };

            SetSettingTypeReadOnlyWhenIndirectUplineEditMember(customerChargedCommission, query);

            return SetSportTypeBetTypeName(customerChargedCommission, settingContext.Language);
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

            var result = _chargedCommissionDataAccess.UpdateCustomerChargedCommission(
                settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                settingType.ToXml());
            var settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(
                settingResult,
                Message.UpdatedChargedCommissionSuccessfully,
                settingType.SettingLevel,
                settingContext.Language);
        }
    }
}
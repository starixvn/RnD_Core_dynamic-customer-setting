namespace Fanex.CustomerSetting.SettingType5.Services
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
        private readonly IDataAccess _chargeCommissionDataAccess;

        public SettingTypeService()
            : this(null, null, null, null)
        {
        }

        internal SettingTypeService(
            IMessageService messageService,
            IResourceService resourceService,
            ILogger logger,
            IDataAccess chargeCommissionDataAccess)
            : base(resourceService, logger)
        {
            MessageService = messageService ?? new MessageService(ResourceService);
            _chargeCommissionDataAccess = chargeCommissionDataAccess ?? new DataAccess();
        }

        public override SettingType GetCustomerSettingType(ISettingContext settingContext, CustomerSettingQuery query)
        {
            IEnumerable<ChargeCommissionDto> chargeCommissionRawData = _chargeCommissionDataAccess.GetCustomerChargeCommissionRawData(query);
            var customerChargeCommission = new SettingType
            {
                Name = Label.ChargeCommission,
                SettingItems = chargeCommissionRawData.Select(productStatusItemDto => new ChargeCommissionItem(productStatusItemDto))
            };

            SetSettingTypeReadOnlyWhenIndirectUplineEditMember(customerChargeCommission, query);

            return SetSportTypeBetTypeName(customerChargeCommission, settingContext.Language);
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

            IResult result = _chargeCommissionDataAccess.UpdateCustomerChargeCommission(
                settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                settingType.ToXml());
            SettingResult settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(
                settingResult,
                Message.UpdatedChargeCommissionSuccessfully,
                settingType.SettingLevel,
                settingContext.Language);
        }
    }
}
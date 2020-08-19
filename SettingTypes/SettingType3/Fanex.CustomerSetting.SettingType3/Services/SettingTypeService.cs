namespace Fanex.CustomerSetting.SettingType3.Services
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
        private readonly IDataAccess _commissionDataAccess;

        public SettingTypeService()
            : this(null, null, null, null)
        {
        }

        internal SettingTypeService(
            IMessageService messageService,
            IResourceService resourceService,
            ILogger logger,
            IDataAccess commissionDataAccess)
            : base(resourceService, logger)
        {
            MessageService = messageService ?? new MessageService(ResourceService);
            _commissionDataAccess = commissionDataAccess ?? new DataAccess();
        }

        public override SettingType GetCustomerSettingType(
            ISettingContext settingContext,
            CustomerSettingQuery query)
        {
            var commissionItemsRawData = _commissionDataAccess.GetCustomerCommissionItemsRawData(query);

            var customerCommission = new SettingType
            {
                Name = Label.Commission,
                SettingItems = commissionItemsRawData.Select<CommissionItemDto, CommissionItem>(commItemDto => new CommissionItem(commItemDto))
            };

            SetSettingTypeReadOnlyWhenIndirectUplineEditMember(customerCommission, query);

            return SetSportTypeBetTypeName(customerCommission, settingContext.Language);
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

            var result = _commissionDataAccess.UpdateCustomerCommission(
                settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                settingType.ToXml(),
                settingType.Id);

            var settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(
                settingResult,
                Message.UpdateCommissionSuccessfully,
                settingType.SettingLevel,
                settingContext.Language);
        }
    }
}
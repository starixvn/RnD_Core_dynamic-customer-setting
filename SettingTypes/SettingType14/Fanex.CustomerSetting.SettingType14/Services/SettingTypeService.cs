namespace Fanex.CustomerSetting.SettingType14.Services
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

        public override SettingType GetCustomerSettingType(ISettingContext settingContext, CustomerSettingQuery query)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));

            IEnumerable<TicketCommissionDto> ticketCommissionRawData = _commissionDataAccess.GetCustomerTicketCommissionRawData(query);
            var customerTicketCommission = new TicketCommissionSettingType
            {
                Name = Label.TicketCommission,
                SettingItems = ticketCommissionRawData.Select(dto => new TicketCommissionItem(dto, query.CustomerLevel, query.UplineLevel))
            };

            SetSettingTypeReadOnlyWhenIndirectUplineEditMember(customerTicketCommission, query);

            return SetSportTypeBetTypeName(customerTicketCommission, settingContext.Language);
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

            IResult result = _commissionDataAccess.UpdateCustomerTicketCommission(
                settingContext,
                customerId,
                settingType.ToXml());
            SettingResult settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(
                settingResult,
                Message.UpdatedTicketCommissionSuccessfully,
                settingType.SettingLevel,
                settingContext.Language);
        }
    }
}
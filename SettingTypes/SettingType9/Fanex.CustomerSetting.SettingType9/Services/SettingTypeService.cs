namespace Fanex.CustomerSetting.SettingType9.Services
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
        private readonly IDataAccess _betLimitDataAccess;

        public SettingTypeService()
            : this(null, null, null, null)
        {
        }

        internal SettingTypeService(
            IMessageService messageService,
            IResourceService resourceService,
            ILogger logger,
            IDataAccess betLimitDataAccess)
            : base(resourceService, logger)
        {
            MessageService = messageService ?? new MessageService(ResourceService);
            _betLimitDataAccess = betLimitDataAccess ?? new DataAccess();
        }

        public override SettingType GetCustomerSettingType(
            ISettingContext settingContext,
            CustomerSettingQuery query)
        {
            IEnumerable<BetLimitDto> rawData = _betLimitDataAccess.GetCustomerBetLimit(query);
            var customerBetLimit = new SettingType
            {
                Name = Label.BetSetting,
                SettingItems = rawData.Select(betLimitDto => new BetLimitItem(betLimitDto))
            };

            SetSettingTypeReadOnlyWhenIndirectUplineEditMember(customerBetLimit, query);

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
            Check.Argument.IsNotNull(settingType, nameof(settingType));

            IResult result = _betLimitDataAccess.UpdateCustomerBetLimit(
                settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                settingType.ToXml());
            SettingResult settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(
                settingResult,
                Message.UpdateBetSettingSuccessfully,
                settingType.SettingLevel,
                settingContext.Language);
        }
    }
}
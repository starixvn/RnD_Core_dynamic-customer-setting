namespace Fanex.CustomerSetting.SettingType12.Services
{
    using System.Collections.Generic;
    using Common.Helper;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using DataAccesses;
    using Logging;
    using Models;
    using Models.Requests;
    using Newtonsoft.Json;
    using Resources;

    public class SettingTypeService : SettingTypeServiceBase, ISettingTypeService, IAdditionalRequestHandler
    {
        private readonly IDataAccess _lossLimitDataAccess;

        public SettingTypeService()
            : this(null, null, null, null)
        {
        }

        internal SettingTypeService(
            IMessageService messageService,
            IResourceService resourceService,
            ILogger logger,
            IDataAccess lossLimitDataAccess)
            : base(resourceService, logger)
        {
            MessageService = messageService ?? new MessageService(ResourceService);
            _lossLimitDataAccess = lossLimitDataAccess ?? new DataAccess();
        }

        public override SettingType GetCustomerSettingType(ISettingContext settingContext, CustomerSettingQuery query)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));

            IEnumerable<LossLimitItem> lossLimits = _lossLimitDataAccess.GetCustomerLossLimit(query);
            var lossLimit = new SettingType
            {
                Name = Label.LossLimit,
                SettingItems = lossLimits
            };

            SetSettingTypeReadOnlyWhenIndirectUplineEditMember(lossLimit, query);

            return SetSportTypeBetTypeName(lossLimit, settingContext.Language);
        }

        public override SettingResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingType)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(settingType, nameof(settingType));

            IResult result = _lossLimitDataAccess.UpdateCustomerLossLimit(
                    settingContext,
                    customerId,
                    customerLevel,
                    uplineLevel,
                    settingType.ToXml());
            SettingResult settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(
                settingResult,
                Message.UpdatedLossLimitSuccessfully,
                settingType.SettingLevel,
                settingContext.Language);
        }

        public object Handle(ISettingContext settingContext, byte actionId, ActionType actionType, string jsonParameters)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(jsonParameters, nameof(jsonParameters));

            CurrentLossAmountQuery query = JsonConvert.DeserializeObject<CurrentLossAmountQuery>(jsonParameters);
            CurrentLossAmountItem currentLossAmount = _lossLimitDataAccess.GetCustomerCurrentLossAmount(query);

            return currentLossAmount;
        }
    }
}
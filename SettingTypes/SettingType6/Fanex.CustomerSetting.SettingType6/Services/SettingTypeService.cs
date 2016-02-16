namespace Fanex.CustomerSetting.SettingType6.Services
{
    using System.Collections.Generic;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType6.DataAccesses;
    using Fanex.CustomerSetting.SettingType6.Models;
    using Fanex.CustomerSetting.SettingType6.Resources;
    using Fanex.Logging;

    public class SettingTypeService : SettingTypeServiceBase, ISettingTypeService, IAdditionalRequestHandler
    {
        private readonly IDataAccess _winLossLimitDataAccess;

        public SettingTypeService()
            : this(null, null, null, null)
        {
        }

        internal SettingTypeService(
            IMessageService messageService,
            IResourceService resourceService,
            ILogger logger,
            IDataAccess winLossLimitDataAccess)
            : base(resourceService, logger)
        {
            MessageService = messageService ?? new MessageService(ResourceService);
            _winLossLimitDataAccess = winLossLimitDataAccess ?? new DataAccess();
        }

        public override SettingType GetCustomerSettingType(ISettingContext settingContext, CustomerSettingQuery query)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));

            IEnumerable<WinLossLimitItem> winLossLimits = _winLossLimitDataAccess.GetCustomerWinLossLimit(query);
            var winLossLimit = new SettingType
            {
                Name = Label.WinLossLimit,
                SettingItems = winLossLimits
            };

            SetSettingTypeReadOnlyWhenIndirectUplineEditMember(winLossLimit, query);

            return SetSportTypeBetTypeName(winLossLimit, settingContext.Language);
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

            IResult result = _winLossLimitDataAccess.UpdateCustomerWinLossLimit(
                    settingContext,
                    customerId,
                    customerLevel,
                    uplineLevel,
                    settingType.ToXml());
            SettingResult settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(
                settingResult,
                Message.UpdatedWinLossLimitSuccessfully,
                settingType.SettingLevel,
                settingContext.Language);
        }

        public object Handle(byte actionId, ActionType actionType, dynamic parameters)
        {
            CurrentWinLossAmountItem currentWinLossAmount = _winLossLimitDataAccess.GetCustomerCurrentWinLossAmount(
                parameters.CustomerId,
                parameters.SportTypeId,
                parameters.BetTypeId);

            return currentWinLossAmount;
        }
    }
}
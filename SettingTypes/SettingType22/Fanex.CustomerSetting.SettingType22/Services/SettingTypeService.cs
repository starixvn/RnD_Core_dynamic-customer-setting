namespace Fanex.CustomerSetting.SettingType22.Services
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
    using Logging;
    using Models;
    using Models.Requests;
    using Newtonsoft.Json;
    using Resources;

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

        public object Handle(ISettingContext settingContext, RedirectCommand command)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(command, nameof(command));

            CurrentWinLossAmountQuery query = JsonConvert.DeserializeObject<CurrentWinLossAmountQuery>(command.JsonParameters);

            query.CustomerId = command.CustomerId;
            query.SettingStatus = command.SettingStatus;
            return SetSporttypeName(_winLossLimitDataAccess.GetCustomerCurrentWinLossAmount(query), settingContext.Language);
        }

        private IEnumerable<CurrentWinLossAmountItem> SetSporttypeName(IEnumerable<CurrentWinLossAmountItem> currentWinLossAmounts, string language)
            => currentWinLossAmounts.Select(item => 
            {
                item.SportTypeName = base.ResourceService.GetSportTypeName(item.SportType, language);
                return item;
            });
    }
}
namespace Fanex.CustomerSetting.SettingType7.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType7.Constants;
    using Fanex.CustomerSetting.SettingType7.DataAccesses;
    using Fanex.CustomerSetting.SettingType7.Dtos;
    using Fanex.CustomerSetting.SettingType7.Models;
    using Fanex.CustomerSetting.SettingType7.Resources;
    using Fanex.Logging;

    public class SettingTypeService : SettingTypeServiceBase, ISettingTypeService
    {
        private readonly IDataAccess _stakeLevelDataAccess;

        public SettingTypeService()
            : this(null, null, null, null)
        {
        }

        internal SettingTypeService(
            IMessageService messageService,
            IResourceService resourceService,
            ILogger logger,
            IDataAccess stakeLevelDataAccess)
            : base(resourceService, logger)
        {
            MessageService = messageService ?? new MessageService(ResourceService);
            _stakeLevelDataAccess = stakeLevelDataAccess ?? new DataAccess();
        }

        public override SettingType GetCustomerSettingType(ISettingContext settingContext, CustomerSettingQuery query)
        {
            IEnumerable<StakeLevelDto> stakeLevelRawData = _stakeLevelDataAccess.GetCustomerStakeLevelRawData(query);
            var customerStakeLevels = new SettingType
            {
                Name = Label.BetLimit,
                SettingItems = MapStakeLevelItems(stakeLevelRawData)
            };

            SetSettingTypeReadOnlyWhenIndirectUplineEditMember(customerStakeLevels, query);

            return SetSportTypeBetTypeName(customerStakeLevels, settingContext.Language);
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

            SettingResult settingResult = ValidateMemberSelectedStakeValue(customerId, customerLevel, settingType);

            if (settingResult.IsSuccessful)
            {
                IResult result = _stakeLevelDataAccess.UpdateCustomerStakeLevel(
                    settingContext,
                    customerId,
                    customerLevel,
                    uplineLevel,
                    settingType.ToXml());

                settingResult = CreateSettingResult(result, customerId, settingType);
            }

            return TranslateUpdateResultMessage(
                settingResult,
                Message.UpdatedBetLimitSuccessfully,
                settingType.SettingLevel,
                settingContext.Language);
        }

        private List<StakeLevelItem> MapStakeLevelItems(IEnumerable<StakeLevelDto> stakeLevelRawData)
        {
            return stakeLevelRawData
                .GroupBy(rawItem => new { rawItem.SportTypeId, rawItem.BetTypeId })
                .Select(group =>
                {
                    var stakeLevelItem = new StakeLevelItem();

                    stakeLevelItem.SportTypeId = group.Key.SportTypeId;
                    stakeLevelItem.BetTypeId = group.Key.BetTypeId;
                    stakeLevelItem.StakeLevelValues = group
                        .Select(item => new StakeLevelValue(item.StakeLevelId, item.StakeLevelRange, item.Selected))
                        .ToList();

                    return stakeLevelItem;
                })
                .ToList();
        }

        private SettingResult ValidateMemberSelectedStakeValue(int customerId, Level customerLevel, SettingType settingType)
        {
            var result = new SettingResult(Result.Success);

            if (customerLevel.IsMemberLevel())
            {
                var violatedItems = settingType.SettingItems.Cast<StakeLevelItem>().Where(stakeLevelItem =>
                {
                    return stakeLevelItem?.StakeLevelValues?.Count(stakeLevelValue => stakeLevelValue.Selected) != 1;
                });

                if (violatedItems.Any())
                {
                    var violatedSportsBetTypes = violatedItems.Select(item => $"{item.SportTypeName} - {item.BetTypeName}");

                    return new SettingResult(
                        ErrorCode.MemberStakeLevelMustBeSelectedForSportBetType,
                        Message.BetLimitMustBeSelectedForSportSettingType,
                        MessageType.StakeLevelViolation,
                        customerId,
                        settingType.Id,
                        settingType.Name,
                        violatedSportsBetTypes);
                }
            }

            return result;
        }
    }
}
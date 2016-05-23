namespace Fanex.CustomerSetting.SettingType7.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Enumerations;
    using CustomerSetting.Models.Requests;
    using CustomerSetting.Models.Results;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using DataAccesses;
    using Dtos;
    using Exceptions;
    using Logging;
    using Models;
    using Resources;

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

            if (customerLevel.IsMemberLevel())
            {
                IResult validationResult = ValidateMemberSelectedStakeValue(settingType);

                if (validationResult.IsFailed)
                {
                    throw new CustomerSettingException(validationResult.Message);
                }
            }

            IResult result = _stakeLevelDataAccess.UpdateCustomerStakeLevel(
                    settingContext,
                    customerId,
                    customerLevel,
                    uplineLevel,
                    settingType.ToXml());
            SettingResult settingResult = CreateSettingResult(result, customerId, settingType);

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
                    stakeLevelItem.StakeRanges = group
                        .Select(item => new StakeRange(item.StakeLevelId, item.StakeLevelRange, item.Selected))
                        .ToList();

                    return stakeLevelItem;
                })
                .ToList();
        }

        private IResult ValidateMemberSelectedStakeValue(SettingType settingType)
        {
            var result = new SettingResult(Result.Success);

            var hasViolatedItem = settingType.SettingItems.Cast<StakeLevelItem>().Any(stakeLevelItem =>
            {
                return stakeLevelItem?.StakeRanges?.Count(stakeLevelValue => stakeLevelValue.Selected) != 1;
            });

            if (hasViolatedItem)
            {
                return new Result(
                    ErrorCode.MemberStakeLevelMustBeSelectedForSportBetType,
                    Message.MemberStakeLevelMustBeSelectedForBetType);
            }

            return Result.Success;
        }
    }
}
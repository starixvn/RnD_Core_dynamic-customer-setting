namespace Fanex.CustomerSetting.SettingType8.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Helper;
    using Constants;
    using CustomerSetting.Exceptions;
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
        private readonly IDataAccess _groupStakeLevelDataAccess;

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
            _groupStakeLevelDataAccess = stakeLevelDataAccess ?? new DataAccess();
        }

        public override SettingType GetCustomerSettingType(ISettingContext settingContext, CustomerSettingQuery query)
        {
            IEnumerable<StakeLevelGroupDto> stakeLevelGroups = _groupStakeLevelDataAccess.GetCustomerStakeLevelGroups(query);
            IEnumerable<StakeLevelDetailDto> stakeLevelGroupsDetails = _groupStakeLevelDataAccess.GetCustomerStakeLevelGroupsDetails(query);
            var customerStakeLevels = new SettingType
            {
                Name = Label.BetLimit,
                SettingItems = MapStakeLevelGroupItems(stakeLevelGroups, stakeLevelGroupsDetails)
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
                IResult validationResult = ValidateMemberSelectedGroup(settingType);

                if (validationResult.IsFailed)
                {
                    throw new CustomerSettingException(validationResult.Message);
                }
            }

            IResult result = _groupStakeLevelDataAccess.UpdateCustomerStakeLevelGroups(
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

        private List<StakeLevelGroupItem> MapStakeLevelGroupItems(
            IEnumerable<StakeLevelGroupDto> stakeLevelGroupsRawData,
            IEnumerable<StakeLevelDetailDto> stakeLevelGroupsDetailsRawData)
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

        private IResult ValidateMemberSelectedGroup(SettingType settingType)
        {
            var result = new SettingResult(Result.Success);

            var hasViolatedItem = settingType.SettingItems.Cast<StakeLevelGroupItem>().Any(item =>
            {
                return item?.LevelGroups?.Count(group => group.Selected) != 1;
            });

            if (hasViolatedItem)
            {
                return new Result(
                    ErrorCode.MemberStakeLevelGroupMustBeSelected,
                    Message.MemberStakeLevelGroupMustBeSelected);
            }

            return Result.Success;
        }
    }
}
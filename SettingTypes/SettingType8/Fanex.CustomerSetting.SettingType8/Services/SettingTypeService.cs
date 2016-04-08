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
    using Models.Requests;
    using Newtonsoft.Json;
    using Resources;

    public class SettingTypeService : SettingTypeServiceBase, ISettingTypeService, IAdditionalRequestHandler
    {
        private const string StakeLevelGroupRefId = "StakeLevel_GG";
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
            IEnumerable<StakeLevelGroupItem> stakeLevelGroups = QueryAndMapStakeLevelGroups(settingContext.Language, query.CustomerId, query.ProductId);
            var customerStakeLevelGroups = new SettingType
            {
                Name = Label.BetLimit,
                SettingItems = stakeLevelGroups
            };

            SetSettingTypeReadOnlyWhenIndirectUplineEditMember(customerStakeLevelGroups, query);

            return SetSportTypeBetTypeName(customerStakeLevelGroups, settingContext.Language);
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

        public object Handle(ISettingContext settingContext, byte actionId, ActionType actionType, string jsonParameters)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(jsonParameters, nameof(jsonParameters));

            StakeLevelGroupDetailsQuery query = JsonConvert.DeserializeObject<StakeLevelGroupDetailsQuery>(jsonParameters);
            IEnumerable<StakeLevelGroupItem> stakeLevelGroupDetails
                = QueryAndMapStakeLevelGroups(settingContext.Language, query.CustomerId, query.ProductId, query.SportTypeId, true);
            var stakeLevelGroup = new SettingType
            {
                Name = Label.BetLimit,
                SettingItems = stakeLevelGroupDetails
            };

            return SetSportTypeBetTypeName(stakeLevelGroup, settingContext.Language);
        }

        private IEnumerable<StakeLevelGroupItem> QueryAndMapStakeLevelGroups(
            string language,
            int customerId,
            byte productId,
            short? sportTypeId = null,
            bool showAll = false)
        {
            var result = Enumerable.Empty<StakeLevelGroupItem>();
            IEnumerable<StakeLevelGroupDto> stakeLevelGroupsRawData
                = _groupStakeLevelDataAccess.GetCustomerStakeLevelGroups(customerId, productId);

            if (stakeLevelGroupsRawData?.Any() == true)
            {
                result = stakeLevelGroupsRawData
                    .GroupBy(rawItem => new { rawItem.SportTypeId, rawItem.BetTypeId })
                    .Where(group => group.Key.SportTypeId == sportTypeId || !sportTypeId.HasValue)
                    .Select(group =>
                    {
                        var stakeLevelGroupItem = new StakeLevelGroupItem();
                        int numberOfDetailedItem = 0;

                        stakeLevelGroupItem.SportTypeId = group.Key.SportTypeId;
                        stakeLevelGroupItem.BetTypeId = group.Key.BetTypeId;
                        stakeLevelGroupItem.Groups = GroupLevelGroups(group.ToList(), language);
                        stakeLevelGroupItem.Details = QueryAndMapStakeLevelGroupDetails(
                            language,
                            customerId,
                            group.Key.SportTypeId,
                            showAll,
                            out numberOfDetailedItem);
                        stakeLevelGroupItem.ShowMore = numberOfDetailedItem > stakeLevelGroupItem.Details.Count();

                        return stakeLevelGroupItem;
                    })
                    .ToList();
            }

            return result;
        }

        private IEnumerable<LevelGroup> GroupLevelGroups(IEnumerable<StakeLevelGroupDto> rawData, string language)
        {
            var result = Enumerable.Empty<LevelGroup>();

            if (rawData?.Any() == true)
            {
                result = rawData.Select(item =>
                {
                    string levelName = ResourceService.GetResourceData(StakeLevelGroupRefId, item.StakeLevelGroupId.ToString(), language);

                    return new LevelGroup(item.StakeLevelId, item.StakeLevelGroupId, levelName, item.Selected);
                });
            }

            return result;
        }

        private IEnumerable<StakeLevel> QueryAndMapStakeLevelGroupDetails(
            string language,
            int customerId,
            short sportTypeId,
            bool showAll,
            out int numberOfDetailedItems)
        {
            var stakeLevelGroupDetails = Enumerable.Empty<StakeLevel>();
            IEnumerable<StakeLevelDetailDto> stakeLevelGroupDetailsRawData
                = _groupStakeLevelDataAccess.GetCustomerStakeLevelGroupsDetails(customerId, sportTypeId, out numberOfDetailedItems, showAll);

            if (stakeLevelGroupDetailsRawData?.Any() == true)
            {
                stakeLevelGroupDetails = stakeLevelGroupDetailsRawData
                    .GroupBy(rawItem => new { rawItem.SportTypeId, rawItem.BetTypeId })
                    .Select(group =>
                    {
                        var stakeLevelItem = new StakeLevel();

                        stakeLevelItem.SportTypeId = group.Key.SportTypeId;
                        stakeLevelItem.SportTypeName = ResourceService.GetSportTypeName(stakeLevelItem.SportTypeId, language);
                        stakeLevelItem.BetTypeId = group.Key.BetTypeId;
                        stakeLevelItem.BetTypeName = ResourceService.GetBetTypeName(stakeLevelItem.BetTypeId, language);
                        stakeLevelItem.StakeRanges = group.Select(item => new StakeRange(item.StakeLevelGroupId, item.StakeLevel));

                        return stakeLevelItem;
                    });
            }

            return stakeLevelGroupDetails;
        }

        private IResult ValidateMemberSelectedGroup(SettingType settingType)
        {
            var result = new SettingResult(Result.Success);

            var hasViolatedItem = settingType.SettingItems.Cast<StakeLevelGroupItem>().Any(item =>
            {
                return item?.Groups?.Count(group => group.Selected) != 1;
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
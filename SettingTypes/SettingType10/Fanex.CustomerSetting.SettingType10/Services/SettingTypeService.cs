namespace Fanex.CustomerSetting.SettingType10.Services
{
    using System;
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

    public class SettingTypeService : SettingTypeServiceBase, ISettingTypeService, IAdditionalRequestHandler
    {
        private const string CommissionGroupRefId = "CommissionGroup";
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

            SettingType customerCommission = query.CustomerLevel.IsMemberLevel()
                ? GetMemberCommissions(query)
                : GetSMACommissions(settingContext, query);

            customerCommission.SettingItems = customerCommission.SettingItems ?? Enumerable.Empty<ISettingItem>();
            customerCommission.Name = Label.Commission;
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
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(settingType, nameof(settingType));

            var result = _commissionDataAccess.UpdateCustomerCommission(
                settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                settingType.ToXml());

            var settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(
                settingResult,
                Message.UpdatedCommissionSuccessfully,
                settingType.SettingLevel,
                settingContext.Language);
        }

        private SettingType GetSMACommissions(ISettingContext settingContext, CustomerSettingQuery query)
        {
            IEnumerable<CommissionDto> rawData = _commissionDataAccess.GetSMAComissions(query);
            var smaCommission = new SettingType();

            if (rawData != null && rawData.Any())
            {
                smaCommission.SettingItems = rawData
                    .GroupBy(dto => new { dto.SportTypeId, dto.BetTypeId })
                    .Select(group =>
                    {
                        ISettingItem commission;

                        if (group.Count() > 1)
                        {
                            commission = new CommissionGroupSettingItem(
                                group.Key.SportTypeId,
                                group.Key.BetTypeId,
                                group,
                                groupId => ResourceService.GetResourceData(
                                    CommissionGroupRefId,
                                    groupId.ToString(),
                                    settingContext.Language));
                        }
                        else
                        {
                            commission = new SingleCommissionSettingItem(group.First());
                        }

                        return commission;
                    });
            }

            return smaCommission;
        }

        private SettingType GetMemberCommissions(CustomerSettingQuery query)
        {
            Tuple<IEnumerable<string>, string, IEnumerable<CommissionDto>> rawData = _commissionDataAccess.GetMemberComissions(query);
            var memberCommission = new MemberCommissionGroupSettingType();

            if (rawData.Item1 != null && rawData.Item1.Any())
            {
                memberCommission.AvailableGroups = rawData.Item1;
            }

            if (!string.IsNullOrEmpty(rawData.Item2))
            {
                memberCommission.UserGroup = rawData.Item2;
            }

            if (rawData.Item1 != null && rawData.Item3.Any())
            {
                memberCommission.SettingItems = rawData.Item3.Select(commissionItem => new MemberCommissionSettingItem(commissionItem));
            }

            return memberCommission;
        }

        public object Handle(ISettingContext settingContext, byte actionId, ActionType actionType, string jsonParameters)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(jsonParameters, nameof(jsonParameters));

            ////StakeLevelGroupDetailsQuery query = JsonConvert.DeserializeObject<StakeLevelGroupDetailsQuery>(jsonParameters);
            IEnumerable<CommissionDto> groupConstraintsRawData = _commissionDataAccess.GetMemberComissionGroupConstraints(1, "a");
            IEnumerable<MemberCommissionSettingItem> groupConstraints = Enumerable.Empty<MemberCommissionSettingItem>();

            if (groupConstraintsRawData?.Any() == true)
            {
                groupConstraints = groupConstraintsRawData.Select(constraint => new MemberCommissionSettingItem(constraint));
            }

            return groupConstraints;
        }
    }
}
namespace Fanex.CustomerSetting.SettingType1.Services
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
    using Logging;
    using Resources;

    public class SettingTypeService : SettingTypeServiceBase, ISettingTypeService
    {
        private readonly IDataAccess _dataAccess;

        public SettingTypeService()
            : this(null, null, null, null)
        {
        }

        internal SettingTypeService(
            IMessageService messageService,
            IResourceService resourceService,
            ILogger logger,
            IDataAccess dataAcess)
            : base(resourceService, logger)
        {
            MessageService = messageService ?? new MessageService(ResourceService);
            _dataAccess = dataAcess ?? new DataAccess();
        }

        public override SettingType GetCustomerSettingType(
            ISettingContext settingContext,
            CustomerSettingQuery query)
        {
            var customerPT = _dataAccess.GetCustomerPT(query);

            return SetSportTypeBetTypeName(customerPT, settingContext.Language);
        }

        public override SettingResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingType)
        {
            Check.Argument.IsNotNull(settingType, "settingType");

            var result = _dataAccess.UpdateCustomerPT(
                settingContext,
                customerId,
                customerLevel,
                uplineLevel,
                settingType.ToXml());

            var settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(settingResult, Message.UpdatePositionTakingSuccessfully, settingType.SettingLevel, settingContext.Language);
        }

        public override IEnumerable<SettingResult> UpdateMultipleCustomerSettingType(
            ISettingContext settingContext,
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingType)
        {
            var results = Enumerable.Empty<SettingResult>();
            var ungroupedCustomerIds = customerIds;

            if (customerLevel.IsMemberLevel())
            {
                var groupedMemberIds = GetGroupedMemberIds(customerIds);

                if (groupedMemberIds.Any())
                {
                    results = UpdateGroupedMembersPT(settingContext, groupedMemberIds, uplineLevel, settingType);

                    ungroupedCustomerIds = customerIds.Except(groupedMemberIds);
                }
            }

            var updateUnGroupedCustomerResults = base.UpdateMultipleCustomerSettingType(
                                                                settingContext,
                                                                ungroupedCustomerIds,
                                                                customerLevel,
                                                                uplineLevel,
                                                                productId,
                                                                settingType);

            return results.Concat(updateUnGroupedCustomerResults);
        }

        private IEnumerable<int> GetGroupedMemberIds(IEnumerable<int> memberIds)
        {
            return _dataAccess.GetGroupedMemberIds(string.Join(",", memberIds));
        }

        public IEnumerable<SettingResult> UpdateGroupedMembersPT(
            ISettingContext settingContext,
            IEnumerable<int> groupedMemberIds,
            Level uplineLevel,
            SettingType settingType)
        {
            Check.Argument.IsNotNull(settingType, "settingType");

            var updatedResult = _dataAccess.UpdateGroupedMembersSetting(
                settingContext.Actor.Id,
                settingContext.Actor.SubAccountId,
                string.Join(",", groupedMemberIds),
                uplineLevel,
                settingType.ToXml(),
                settingContext.IP);

            return SeparateResultForEachMember(updatedResult, groupedMemberIds, settingType, settingContext.Language);
        }

        private IEnumerable<SettingResult> SeparateResultForEachMember(IResult result, IEnumerable<int> groupedMemberIds, SettingType settingType, string language)
        {
            var settingResults = new List<SettingResult>();
            var sharedSettingResult = CreateSettingResult(result, 0, settingType);
            var messageContent = TranslateUpdateResultMessage(sharedSettingResult, Message.UpdatePositionTakingSuccessfully, settingType.SettingLevel, language);

            foreach (int memberId in groupedMemberIds)
            {
                var settingResult = new SettingResult(
                    result.Code,
                    messageContent.Message,
                    result.MsgType,
                    memberId,
                    Constants.SettingTypeId,
                    Label.PositionTaking,
                    messageContent.SubMessages);

                settingResults.Add(settingResult);
            }

            return settingResults;
        }
    }
}
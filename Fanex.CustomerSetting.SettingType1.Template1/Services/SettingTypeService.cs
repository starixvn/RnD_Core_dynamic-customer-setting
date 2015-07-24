namespace Fanex.CustomerSetting.SettingType1.Template1.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType1.Template1.Constants;
    using Fanex.CustomerSetting.SettingType1.Template1.DataAccesses;
    using Fanex.Logging;

    public class SettingTypeService : SettingTypeServiceBase, ISettingTypeService
    {
        private readonly IDataAccess _dataAccess;

        public SettingTypeService()
            : this(null, null, null, null)
        {
        }

        public SettingTypeService(
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
            CustomerSettingQuery query,
            SettingType productSettingType)
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
                productId, 
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

            if (customerLevel.IsMemberLevel)
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

            return SeparateResultForEachMember(updatedResult, groupedMemberIds);
        }

        private IEnumerable<SettingResult> SeparateResultForEachMember(IResult result, IEnumerable<int> groupedMemberIds)
        {
            var settingResults = new List<SettingResult>();

            foreach (int memberId in groupedMemberIds)
            {
                var settingResult = new SettingResult(result.Code, result.Message, result.MsgType, memberId, Constants.SettingTypeId, Label.PositionTaking);

                settingResults.Add(settingResult);
            }

            return settingResults;
        }
    }
}
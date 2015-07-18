namespace Fanex.CustomerSetting.SettingType1.Template1.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.SettingType1.Template1.Constants;
    using Fanex.CustomerSetting.SettingType1.Template1.DataAccesses;
    using Fanex.CustomerSetting.SettingType1.Template1.Dtos;

    public class SettingType1Service : BaseSettingTypeService, ISettingTypeService
    {
        private readonly IPTDataAccess _positionTakingDataAccess;

        public SettingType1Service()
            : this(null, null, null)
        {
        }

        public SettingType1Service(
            IMessageService messageService,
            IResourceService resourceService,
            IPTDataAccess positionTakingDataAccess)
            : base(resourceService)
        {
            MessageService = messageService ?? new PTMessageService(ResourceService);
            _positionTakingDataAccess = positionTakingDataAccess ?? new PTDataAccess();
        }

        public override SettingType GetCustomerSettingType(
            ISettingContext settingContext,
            QuerySettingParam queryParam,
            SettingType productSettingType)
        {
            var customerPT = _positionTakingDataAccess.GetCustomerPT(queryParam);

            return SetSportTypeBetTypeName(customerPT, settingContext.Language);
        }

        public override SettingResult UpdateCustomerSettingType(ISettingContext settingContext, int customerId, Enumerations.Level customerLevel, Enumerations.Level uplineLevel, byte productId, SettingType settingType)
        {
            var result = _positionTakingDataAccess.UpdateCustomerPT(settingContext, customerId, customerLevel, uplineLevel, productId, settingType.ToXml());

            var settingResult = CreateSettingResult(result, customerId, settingType);

            return TranslateUpdateResultMessage(settingResult, Message.UpdatePositionTakingSuccessfully, settingType.SettingLevel, settingContext.Language);
        }

        public override IEnumerable<SettingResult> UpdateMultipleCustomersSettingType(
            ISettingContext settingContext,
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingType)
        {
            var groupedMemberIds = GetGroupedMemberIds(customerIds);

            var ungroupedMemberIds = customerIds.Except(groupedMemberIds);

            var updateGroupedMemberResults = UpdateGroupedMembersPT(settingContext, groupedMemberIds, uplineLevel, settingType);

            var updatedUnGroupedMemberResults = base.UpdateMultipleCustomersSettingType(
                                                                settingContext,
                                                                ungroupedMemberIds,
                                                                customerLevel,
                                                                uplineLevel,
                                                                productId,
                                                                settingType);

            return updateGroupedMemberResults.Concat(updatedUnGroupedMemberResults);
        }

        private IEnumerable<int> GetGroupedMemberIds(IEnumerable<int> memberIds)
        {
            return _positionTakingDataAccess.GetGroupedMemberIds(string.Join(",", memberIds));
        }

        public IEnumerable<SettingResult> UpdateGroupedMembersPT(
            ISettingContext settingContext,
            IEnumerable<int> groupedMemberIds,
            Level uplineLevel,
            SettingType settingType)
        {
            if (groupedMemberIds == null || !groupedMemberIds.Any())
            {
                return Enumerable.Empty<SettingResult>();
            }

            var updatedResult = _positionTakingDataAccess.UpdateGroupedMembersSetting(
                settingContext.Actor.Id,
                settingContext.Actor.SubAccountId,
                string.Join(",", groupedMemberIds),
                uplineLevel,
                settingType.ToXml(),
                settingContext.IP);

            return SeparateResultForEachCustomer(updatedResult, groupedMemberIds);
        }

        private IEnumerable<SettingResult> SeparateResultForEachCustomer(IResult result, IEnumerable<int> groupedMemberIds)
        {
            var settingResults = new List<SettingResult>();

            foreach (int memberId in groupedMemberIds)
            {
                var settingResult = new SettingResult(result.Code, result.Message, result.MsgType, memberId, PTConstants.SettingTypeId, Label.PositionTaking);

                settingResults.Add(settingResult);
            }

            return settingResults;
        }
    }
}
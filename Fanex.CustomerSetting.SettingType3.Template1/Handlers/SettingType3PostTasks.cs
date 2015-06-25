namespace Fanex.CustomerSetting.SettingType3.Template1.Handlers
{
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Resources;
    using Fanex.CustomerSetting.Resource.Services;
    using Fanex.CustomerSetting.Resource.Services.Impl;
    using Fanex.CustomerSetting.SettingType3.Template1.Models;
    using Fanex.CustomerSetting.SettingType3.Template1.Services;

    public class SettingType3PostTasks : SettingPostTasks
    {
        private readonly IMessageService _messageService;
        private readonly IResourceService _resourceService;

        public SettingType3PostTasks() :
            this(null, null, null)
        {
        }

        public SettingType3PostTasks(
            ISettingContext settingContext,
            IMessageService messageService,
            IResourceService resourceService)
            : base(settingContext)
        {
            _messageService = messageService ?? new CommissionMessageService();
            _resourceService = resourceService ?? new ResourceService();
        }

        public override SettingType PostGetCustomerSettingType(QuerySettingParams queryParams, SettingType settingType)
        {
            if (settingType != null && settingType.SettingItems.Any())
            {
                foreach (ISettingItem settingItem in settingType.SettingItems)
                {
                    if (settingItem is CommissionItem)
                    {
                        var concreteSettingItem = settingItem as CommissionItem;
                        concreteSettingItem.BetTypeName = _resourceService.GetBetTypeName(concreteSettingItem.BetTypeId, SettingContext.Language);
                        concreteSettingItem.SportTypeName = _resourceService.GetSportTypeName(concreteSettingItem.SportTypeId, SettingContext.Language);
                    }
                }
            }

            return settingType;
        }

        public override IResult PostUpdateCustomerSettingType(
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult result)
        {
            if (result.IsSuccessful)
            {
                return new Result(Result.SuccessValue, Lang.UpdateCommissionSuccessfully);
            }
            else
            {
                var messageContent = _messageService.TranslateMessage(result.MsgType, result.Message, settingType.SettingLevel);
                return new Result(result.Code, messageContent.MainMessage, messageContent.SubMessages);
            }
        }
    }
}
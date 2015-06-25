namespace Fanex.CustomerSetting.SettingType4.Template1.Handlers
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
    using Fanex.CustomerSetting.SettingType4.Template1.Models;
    using Fanex.CustomerSetting.SettingType4.Template1.Services;

    public class SettingType4PostTasks : SettingPostTasks
    {
        private readonly IMessageService _messageService;
        private readonly IResourceService _resourceService;

        public SettingType4PostTasks() :
            this(null, null, null)
        {
        }

        public SettingType4PostTasks(
            ISettingContext settingContext,
            IMessageService messageService,
            IResourceService resourceService)
            : base(settingContext)
        {
            _messageService = messageService ?? new ProductStatusMessageService();
            _resourceService = resourceService ?? new ResourceService();
        }

        public override SettingType PostGetCustomerSettingType(QuerySettingParams queryParams, SettingType settingType)
        {
            if (settingType != null && settingType.SettingItems.Any())
            {
                foreach (ISettingItem settingItem in settingType.SettingItems)
                {
                    if (settingItem is ProductStatusItem)
                    {
                        var concreteSettingItem = settingItem as ProductStatusItem;
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
                return new Result(Result.SuccessValue, Lang.UpdateProductStatusSuccessfully);
            }
            else
            {
                var messageContent = _messageService.TranslateMessage(result.MsgType, result.Message, settingType.SettingLevel);
                return new Result(result.Code, messageContent.MainMessage, messageContent.SubMessages);
            }
        }
    }
}
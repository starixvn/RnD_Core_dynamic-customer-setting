namespace Fanex.CustomerSetting.SettingType2.Template1.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Resources;
    using Fanex.CustomerSetting.Resource.Services;
    using Fanex.CustomerSetting.Resource.Services.Impl;
    using Fanex.CustomerSetting.SettingType2.Template1.Models;
    using Fanex.CustomerSetting.SettingType2.Template1.Services;

    public class SettingType2PostTasks : SettingPostTasks
    {
        private readonly IMessageService _messageService;
        private readonly IResourceService _resourceService;

        public SettingType2PostTasks() :
            this(null, null, null)
        {
        }

        public SettingType2PostTasks(
            ISettingContext settingContext,
            IMessageService messageService,
            IResourceService resourceService)
            : base(settingContext)
        {
            _messageService = messageService ?? new BetLimitMessageService();
            _resourceService = resourceService ?? new ResourceService();
        }

        public override SettingType PostGetCustomerSettingType(QuerySettingParams queryParams, SettingType settingType)
        {
            if (settingType != null && settingType.SettingItems.Any())
            {
                foreach (ISettingItem settingItem in settingType.SettingItems)
                {
                    if (settingItem is BetLimitItem)
                    {
                        var concreteSettingItem = settingItem as BetLimitItem;
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
                return new Result(Result.SuccessValue, Lang.UpdateBetSettingSuccessfully);
            }
            else
            {
                var messageContent = _messageService.TranslateMessage(result.MsgType, result.Message, settingType.SettingLevel);
                return new Result(result.Code, messageContent.MainMessage, messageContent.SubMessages);
            }
        }
    }
}

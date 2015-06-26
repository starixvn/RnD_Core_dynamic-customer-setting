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

        public SettingType2PostTasks() :
            this(null, null, null)
        {
        }

        public SettingType2PostTasks(
            ISettingContext settingContext,
            IMessageService messageService,
            IResourceService resourceService)
            : base(settingContext, resourceService)
        {
            _messageService = messageService ?? new BetLimitMessageService();
        }

        public override SettingType PostGetCustomerSettingType(QuerySettingParams queryParams, SettingType settingType)
        {
            return SetSportTypeBetTypeName(settingType);
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

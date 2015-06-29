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
        public SettingType2PostTasks() 
            : this(null, null)
        {
        }

        public SettingType2PostTasks(
            IMessageService messageService,
            IResourceService resourceService)
            : base(resourceService)
        {
            MessageService = messageService ?? new BetLimitMessageService(ResourceService);
        }

        public override SettingType PostGetCustomerSettingType(
            ISettingContext settingContext,
            QuerySettingParam queryParams, 
            SettingType settingType)
        {
            return SetSportTypeBetTypeName(settingType, settingContext.Language);
        }

        public override IResult PostUpdateCustomerSettingType(
            ISettingContext settingContext,
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult result)
        {
            return ReadUpdateingCustomerSettingTypeMessage(result, Lang.UpdateBetSettingSuccessfully, settingType.SettingLevel, settingContext.Language);
        }
    }
}

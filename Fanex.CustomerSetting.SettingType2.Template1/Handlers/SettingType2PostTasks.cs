namespace Fanex.CustomerSetting.SettingType2.Template1.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;
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
            QuerySettingParam queryParam,
            SettingType settingType)
        {
            return base.PostGetCustomerSettingType(settingContext, queryParam, settingType);
        }

        public override SettingResult PostUpdateCustomerSettingType(
            ISettingContext settingContext,
            int customerId,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            SettingResult result)
        {
            if (settingType == null)
            {
                throw new ArgumentNullException("settingType");
            }

            if (settingContext == null)
            {
                throw new ArgumentNullException("settingContext");
            }

            return ReadUpdatingCustomerSettingTypeMessage(result, Message.UpdateBetSettingSuccessfully, settingType.SettingLevel, settingContext.Language);
        }
    }
}
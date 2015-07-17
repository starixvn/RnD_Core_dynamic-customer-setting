namespace Fanex.CustomerSetting.SettingType4.Template1.Handlers
{
    using System;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType4.Template1.Services;

    public class SettingType4PostTasks : SettingPostTasks
    {
        public SettingType4PostTasks()
            : this(null, null)
        {
        }

        public SettingType4PostTasks(
            IMessageService messageService,
            IResourceService resourceService)
            : base(resourceService)
        {
            MessageService = messageService ?? new ProductStatusMessageService(ResourceService);
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
            UserLevel customerLevel,
            SettingType settingType,
            UserLevel uplineLevel,
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

            return ReadUpdatingCustomerSettingTypeMessage(result, Message.UpdateProductStatusSuccessfully, settingType.SettingLevel, settingContext.Language);
        }
    }
}
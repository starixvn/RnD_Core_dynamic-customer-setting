namespace Fanex.CustomerSetting.SettingType3.Template1.Handlers
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Resources;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.SettingType3.Template1.Services;

    public class SettingType3PostTasks : SettingPostTasks
    {
        public SettingType3PostTasks()
            : this(null, null)
        {
        }

        public SettingType3PostTasks(
            IMessageService messageService,
            IResourceService resourceService)
            : base(resourceService)
        {
            MessageService = messageService ?? new CommissionMessageService(ResourceService);
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
            return ReadUpdatingCustomerSettingTypeMessage(result, Message.UpdateCommissionSuccessfully, settingType.SettingLevel, settingContext.Language);
        }
    }
}
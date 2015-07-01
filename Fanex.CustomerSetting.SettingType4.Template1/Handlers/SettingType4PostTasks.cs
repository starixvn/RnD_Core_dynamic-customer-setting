namespace Fanex.CustomerSetting.SettingType4.Template1.Handlers
{
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Resources;
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
            return SetSportTypeBetTypeName(settingType, settingContext.Language);
        }

        public override IResult PostUpdateCustomerSettingType(
            ISettingContext settingContext,
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult result)
        {
            var settingResult = new SettingResult(result.Code, result.Message, customer.Id, customer.FirstName, settingType.Id, settingType.Name);
            return ReadUpdatingCustomerSettingTypeMessage(settingResult, Message.UpdateProductStatusSuccessfully, settingType.SettingLevel, settingContext.Language);
        }
    }
}
namespace Fanex.CustomerSetting.SettingType1.Template1.Handlers
{
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Handlers;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Resources;
    using Fanex.CustomerSetting.Resource.Services;
    using Fanex.CustomerSetting.SettingType1.Template1.Services;

    public class SettingType1PostTasks : SettingPostTasks
    {
        private readonly IMessageService _messageService;

        public SettingType1PostTasks() :
            this(null, null)
        {
        }

        public SettingType1PostTasks(
            ISettingContext settingContext,
            IMessageService messageService)
            : base(settingContext)
        {
            _messageService = messageService ?? new PositionTakingMessageService();
        }

        public override IResult PostUpdateCustomerSettingType(
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult result)
        {
            if (result.IsSuccessful)
            {
                return new Result(Result.SuccessValue, Lang.UpdatePositionTakingSuccessfully);
            }
            else
            {
                var messageContent = _messageService.TranslateMessage(result.MsgType, result.Message, settingType.SettingLevel);
                return new Result(result.Code, messageContent.MainMessage, messageContent.SubMessages);
            }
        }
    }
}
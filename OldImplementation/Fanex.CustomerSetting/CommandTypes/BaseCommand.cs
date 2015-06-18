namespace Fanex.CustomerSetting.CommandTypes
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;

    public abstract class BaseCommand
    {
        private readonly IMessageService _messageService;

        protected BaseCommand()
            : this(null)
        {
        }

        protected BaseCommand(IMessageService messageService)
        {
            _messageService = messageService ?? new MessageService();
        }

        protected SettingResult Process(CommandCriteria criteria, CommandInfo commandInfo, SettingType settingType)
        {
            var settingLevel = SettingLevel.BetType;
            switch (settingType)
            {
                case SettingType.Status:
                    settingLevel = commandInfo.StatusLevel;
                    break;

                case SettingType.Discount:
                    settingLevel = commandInfo.DiscountLevel;
                    break;

                case SettingType.BetLimit:
                    settingLevel = commandInfo.BetLimitLevel;
                    break;

                case SettingType.PositionTaking:
                    settingLevel = commandInfo.PositionTakingLevel;
                    break;
            }

            var messageInfo = new MessageInfo
            {
                ErrCode = criteria.ErrCode,
                ErrMsg = criteria.ErrMsg,
                MsgType = (MsgType)criteria.MsgType,
                TargetRoleId = commandInfo.TargetRoleId,
                SettingLevel = settingLevel,
                SettingType = settingType
            };

            var messageContent = _messageService.GetTranslatedMessage(messageInfo);
            return new SettingResult(settingType, new Result(messageInfo.ErrCode, messageContent));
        }
    }
}
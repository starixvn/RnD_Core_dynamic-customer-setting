namespace Fanex.CustomerSetting.CommandTypes
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;
    using Fanex.CustomerSetting.Services;

    public class BaseCommand
    {
        protected ICommandRepository _commandRepository = new CommandRepository();
        protected IXmlGenerator _xmlGenerator = new XmlGenerator();

        protected CommandResult Process(CommandCriteria criteria, CommandInfo model, SettingType settingType)
        {
            var settingLevel = SettingLevel.BetType;
            switch (settingType)
            {
                case SettingType.Status:
                    settingLevel = model.StatusLevel;
                    break;

                case SettingType.Discount:
                    settingLevel = model.DiscountLevel;
                    break;

                case SettingType.BetLimit:
                    settingLevel = model.BetLimitLevel;
                    break;

                case SettingType.PositionTaking:
                    settingLevel = model.PositionTakingLevel;
                    break;
            }

            var messageInfo = new MessageInfo
            {
                ErrCode = criteria.ErrCode,
                ErrMsg = criteria.ErrMsg,
                MsgType = (MsgType)criteria.MsgType,
                TargetRoleId = model.TargetRoleId,
                SettingLevel = settingLevel,
                SettingType = settingType
            };

            var messageService = new MessageService("en-US");
            var message = messageService.GetTranslatedMessage(messageInfo);
            return new CommandResult(model.CommandType, messageInfo.ErrCode, message);
        }
    }
}
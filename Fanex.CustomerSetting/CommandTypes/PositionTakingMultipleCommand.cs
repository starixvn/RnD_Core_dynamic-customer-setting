namespace Fanex.CustomerSetting.CommandTypes
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;
    using Fanex.CustomerSetting.Services;

    public class PositionTakingMultipleCommand : IMultipleCommand
    {
        public CommandResult Do(MultipleCommandInfo multipleCommandInfo)
        {
            var commandCriteria = new PositionTakingCommandOnGroupedMembersCriteria(multipleCommandInfo.FormType);
            var repository = new CommandOnGroupedMembersRepository();
            repository.UpdatePositionTaking(commandCriteria);
            var messageInfo = new MessageInfo
            {
                ErrCode = commandCriteria.ErrCode,
                ErrMsg = commandCriteria.ErrMsg,
                MsgType = (MsgType)commandCriteria.MsgType,
                TargetRoleId = multipleCommandInfo.TargetRoleId,
                SettingLevel = multipleCommandInfo.PositionTakingLevel,
                SettingType = SettingType.PositionTaking
            };

            var messageService = new MessageService("en-US");
            var messageContent = messageService.GetTranslatedMessage(messageInfo);
            var settingResult = new SettingResult(SettingType.PositionTaking, new Result(messageInfo.ErrCode, messageContent));
            return new CommandResult(multipleCommandInfo, settingResult);
        }
    }
}
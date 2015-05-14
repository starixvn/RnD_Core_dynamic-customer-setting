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
            var criteria = new PositionTakingCommandOnGroupedMembersCriteria(multipleCommandInfo.FormType);
            var repository = new CommandOnGroupedMembersRepository();
            repository.UpdatePositionTaking(criteria);
            var messageInfo = new MessageInfo
            {
                ErrCode = criteria.ErrCode,
                ErrMsg = criteria.ErrMsg,
                MsgType = (MsgType)criteria.MsgType,
                TargetRoleId = multipleCommandInfo.TargetRoleId,
                SettingLevel = multipleCommandInfo.PositionTakingLevel,
                SettingType = SettingType.PositionTaking
            };

            var messageService = new MessageService("en-US");
            var positionTakingMessage = messageService.GetTranslatedMessage(messageInfo);
            return new CommandResult(CommandType.PositionTaking, messageInfo.ErrCode, positionTakingMessage);
        }
    }
}
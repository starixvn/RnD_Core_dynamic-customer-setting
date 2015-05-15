namespace Fanex.CustomerSetting.CommandTypes
{
    using System.Linq;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;
    using Fanex.CustomerSetting.Services;

    public class PositionTakingMultipleCommand : IMultipleCommand
    {
        public CommandResult Do(MultipleCommandInfo multipleCommandInfo)
        {
            var settingType = SettingType.PositionTaking;
            if (multipleCommandInfo.PositionTakings == null || multipleCommandInfo.PositionTakings.Count() == 0)
            {
                return new CommandResult(multipleCommandInfo, new NoChangeSettingResult(settingType));
            }

            IXmlGenerator _xmlGenerator = new XmlGenerator();
            var commandCriteria = new PositionTakingCommandOnGroupedMembersCriteria(multipleCommandInfo.FormType)
            {
                UserId = multipleCommandInfo.CustId,
                SubAccId = multipleCommandInfo.SubAccountId,
                CustIds = string.Join(",", multipleCommandInfo.TargetCustIds),
                PositionTaking = _xmlGenerator.GeneratePositionTaking(multipleCommandInfo.PositionTakings),
                Ip = multipleCommandInfo.Ip
            };

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
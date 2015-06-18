namespace Fanex.CustomerSetting.CommandTypes
{
    using System.Linq;
    using Fanex.CustomerSetting.Converters;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;
    using Fanex.CustomerSetting.Services;

    public class PositionTakingMultipleCommand : IMultipleCommand
    {
        private readonly IMessageService _messageService;

        public PositionTakingMultipleCommand()
            : this(null)
        {
        }

        public PositionTakingMultipleCommand(IMessageService messageService)
        {
            _messageService = messageService ?? new MessageService();
        }

        public CommandResult Execute(MultipleCommandInfo multipleCommandInfo)
        {
            var settingType = SettingType.PositionTaking;
            if (multipleCommandInfo.PositionTakings == null || multipleCommandInfo.PositionTakings.Count() == 0)
            {
                return new CommandResult(multipleCommandInfo, new NoChangeSettingResult(settingType));
            }

            var xmlConverter = new PositionTakingXmlConverter();
            var commandCriteria = new PositionTakingCommandOnGroupedMembersCriteria(multipleCommandInfo.FormType)
            {
                UserId = multipleCommandInfo.CustId,
                SubAccId = multipleCommandInfo.SubAccountId,
                CustIds = string.Join(",", multipleCommandInfo.TargetCustIds),
                PositionTaking = xmlConverter.Generate(multipleCommandInfo.PositionTakings),
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

            var messageContent = _messageService.GetTranslatedMessage(messageInfo);
            var settingResult = new SettingResult(SettingType.PositionTaking, new Result(messageInfo.ErrCode, messageContent));
            return new CommandResult(multipleCommandInfo, settingResult);
        }
    }
}
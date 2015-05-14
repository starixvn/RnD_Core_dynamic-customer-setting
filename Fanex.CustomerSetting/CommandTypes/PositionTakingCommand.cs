namespace Fanex.CustomerSetting.CommandTypes
{
    using System.Linq;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;

    public class PositionTakingCommand : BaseCommand, ICommand
    {
        public CommandResult Do(CommandInfo commandInfo)
        {
            var settingType = SettingType.PositionTaking;
            if (commandInfo.PositionTakings == null || commandInfo.PositionTakings.Count() == 0)
            {
                return new CommandResult(commandInfo, new NoChangeSettingResult(settingType));
            }

            var commandCriteria = new PositionTakingCommandCriteria(commandInfo.FormType)
            {
                UserId = commandInfo.CustId,
                SubAccId = commandInfo.SubAccountId,
                CustId = commandInfo.TargetCustId,
                PositionTaking = _xmlGenerator.GeneratePositionTaking(commandInfo.PositionTakings),
                Ip = commandInfo.Ip
            };
            if (!commandCriteria.IsValid())
            {
                throw new CriteriaException(commandCriteria);
            }

            _commandRepository.UpdatePositionTaking(commandCriteria);
            var settingResult = Process(commandCriteria, commandInfo, settingType);
            return new CommandResult(commandInfo, settingResult);
        }
    }
}
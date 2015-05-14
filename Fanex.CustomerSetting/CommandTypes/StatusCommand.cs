namespace Fanex.CustomerSetting.CommandTypes
{
    using System.Linq;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;

    public class StatusCommand : BaseCommand, ICommand
    {
        public CommandResult Do(CommandInfo commandInfo)
        {
            var settingType = SettingType.Status;
            if (commandInfo.Statuses == null || commandInfo.Statuses.Count() == 0)
            {
                return new CommandResult(commandInfo, new NoChangeSettingResult(settingType));
            }

            var commandCriteria = new DisabledStatusCommandCriteria
            {
                UserId = commandInfo.CustId,
                SubAccId = commandInfo.SubAccountId,
                CustId = commandInfo.TargetCustId,
                IsDisabled = _xmlGenerator.GenerateStatus(commandInfo.Statuses),
                Ip = commandInfo.Ip
            };
            if (!commandCriteria.IsValid())
            {
                throw new CriteriaException(commandCriteria);
            }

            _commandRepository.UpdateDisabledStatus(commandCriteria);
            var settingResult = Process(commandCriteria, commandInfo, settingType);
            return new CommandResult(commandInfo, settingResult);
        }
    }
}
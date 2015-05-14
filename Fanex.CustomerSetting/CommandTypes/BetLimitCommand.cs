namespace Fanex.CustomerSetting.CommandTypes
{
    using System.Linq;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;

    public class BetLimitCommand : BaseCommand, ICommand
    {
        public CommandResult Do(CommandInfo commandInfo)
        {
            var settingType = SettingType.BetLimit;
            if (commandInfo.BetLimits == null || commandInfo.BetLimits.Count() == 0)
            {
                return new CommandResult(commandInfo, new NoChangeSettingResult(settingType));
            }

            var commandCriteria = new BetLimitCommandCriteria
            {
                UserId = commandInfo.CustId,
                SubAccId = commandInfo.SubAccountId,
                CustId = commandInfo.TargetCustId,
                BetLimit = _xmlGenerator.GenerateBetLimit(commandInfo.BetLimits),
                Ip = commandInfo.Ip
            };
            if (!commandCriteria.IsValid())
            {
                throw new CriteriaException(commandCriteria);
            }

            _commandRepository.UpdateBetLimit(commandCriteria);
            var settingResult = Process(commandCriteria, commandInfo, settingType);
            return new CommandResult(commandInfo, settingResult);
        }
    }
}
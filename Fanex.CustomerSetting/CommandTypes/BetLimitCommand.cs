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
            if (commandInfo.BetLimits == null || commandInfo.BetLimits.Count() == 0)
            {
                return new NoChangeCommandResult(CommandType.BetLimit);
            }

            var criteria = new BetLimitCommandCriteria
            {
                UserId = commandInfo.CustId,
                SubAccId = commandInfo.SubAccountId,
                CustId = commandInfo.TargetCustId,
                BetLimit = _xmlGenerator.GenerateBetLimit(commandInfo.BetLimits),
                Ip = commandInfo.Ip
            };
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            _commandRepository.UpdateBetLimit(criteria);
            return Process(criteria, commandInfo, SettingType.BetLimit);
        }
    }
}
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
            if (commandInfo.PositionTakings == null || commandInfo.PositionTakings.Count() == 0)
            {
                return new NoChangeCommandResult(CommandType.PositionTaking);
            }

            var criteria = new PositionTakingCommandCriteria(commandInfo.FormType)
            {
                UserId = commandInfo.CustId,
                SubAccId = commandInfo.SubAccountId,
                CustId = commandInfo.TargetCustId,
                PositionTaking = _xmlGenerator.GeneratePositionTaking(commandInfo.PositionTakings),
                Ip = commandInfo.Ip
            };
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            _commandRepository.UpdatePositionTaking(criteria);
            return Process(criteria, commandInfo, SettingType.PositionTaking);
        }
    }
}
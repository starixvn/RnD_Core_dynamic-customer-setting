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
            if (commandInfo.Statuses == null || commandInfo.Statuses.Count() == 0)
            {
                return new NoChangeCommandResult(CommandType.Status);
            }

            var criteria = new DisabledStatusCommandCriteria
            {
                UserId = commandInfo.CustId,
                SubAccId = commandInfo.SubAccountId,
                CustId = commandInfo.TargetCustId,
                IsDisabled = _xmlGenerator.GenerateStatus(commandInfo.Statuses),
                Ip = commandInfo.Ip
            };
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            _commandRepository.UpdateDisabledStatus(criteria);
            return Process(criteria, commandInfo, SettingType.Status);
        }
    }
}
namespace Fanex.CustomerSetting.CommandTypes
{
    using System.Linq;
    using Fanex.CustomerSetting.Converters;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;
    using Fanex.CustomerSetting.Services;

    public class BetLimitCommand : BaseCommand, ICommand
    {
        private readonly ICommandRepository _commandRepository;

        public BetLimitCommand()
            : this(null, null)
        {
        }

        public BetLimitCommand(ICommandRepository commandRepository, IMessageService messageService)
            : base(messageService)
        {
            _commandRepository = commandRepository ?? new CommandRepository();
        }

        public CommandResult Execute(CommandInfo commandInfo)
        {
            var settingType = SettingType.BetLimit;
            if (commandInfo.BetLimits == null || commandInfo.BetLimits.Count() == 0)
            {
                return new CommandResult(commandInfo, new NoChangeSettingResult(settingType));
            }

            var xmlConverter = new BetLimitXmlConverter();
            var commandCriteria = new BetLimitCommandCriteria
            {
                UserId = commandInfo.CustId,
                SubAccId = commandInfo.SubAccountId,
                CustId = commandInfo.TargetCustId,
                BetLimit = xmlConverter.Generate(commandInfo.BetLimits),
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
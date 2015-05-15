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

    public class PositionTakingCommand : BaseCommand, ICommand
    {
        private readonly ICommandRepository _commandRepository;

        public PositionTakingCommand()
            : this(null, null)
        {
        }

        public PositionTakingCommand(ICommandRepository commandRepository, IMessageService messageService)
            : base(messageService)
        {
            _commandRepository = commandRepository ?? new CommandRepository();
        }

        public CommandResult Execute(CommandInfo commandInfo)
        {
            var settingType = SettingType.PositionTaking;
            if (commandInfo.PositionTakings == null || commandInfo.PositionTakings.Count() == 0)
            {
                return new CommandResult(commandInfo, new NoChangeSettingResult(settingType));
            }

            var xmlConverter = new PositionTakingXmlConverter();
            var commandCriteria = new PositionTakingCommandCriteria(commandInfo.FormType)
            {
                UserId = commandInfo.CustId,
                SubAccId = commandInfo.SubAccountId,
                CustId = commandInfo.TargetCustId,
                PositionTaking = xmlConverter.Generate(commandInfo.PositionTakings),
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
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

    public class StatusCommand : BaseCommand, ICommand
    {
        private readonly ICommandRepository _commandRepository;

        public StatusCommand()
            : this(null, null)
        {
        }

        public StatusCommand(ICommandRepository commandRepository, IMessageService messageService)
            : base(messageService)
        {
            _commandRepository = commandRepository ?? new CommandRepository();
        }

        public CommandResult Execute(CommandInfo commandInfo)
        {
            var settingType = SettingType.Status;
            if (commandInfo.Statuses == null || commandInfo.Statuses.Count() == 0)
            {
                return new CommandResult(commandInfo, new NoChangeSettingResult(settingType));
            }

            var xmlConverter = new StatusXmlConverter();
            var commandCriteria = new DisabledStatusCommandCriteria
            {
                UserId = commandInfo.CustId,
                SubAccId = commandInfo.SubAccountId,
                CustId = commandInfo.TargetCustId,
                IsDisabled = xmlConverter.Generate(commandInfo.Statuses),
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
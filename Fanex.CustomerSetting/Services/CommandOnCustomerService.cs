namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.CommandTypes;
    using Fanex.CustomerSetting.Models;

    public class CommandOnCustomerService : ICommandOnCustomerService
    {
        private readonly IValidationService _validationService;
        private readonly ICommandFactory _commandFactory;

        public CommandOnCustomerService()
            : this(null, null)
        {
        }

        public CommandOnCustomerService(
            IValidationService validationService,
            ICommandFactory commandFactory)
        {
            _validationService = validationService ?? new ValidationService();
            _commandFactory = commandFactory ?? new CommandFactory();
        }

        public CommandResult Execute(CommandInfo commandInfo)
        {
            CommandResult commandResult = null;
            Result validationResult = _validationService.Validate(commandInfo);
            if (validationResult.IsSuccess)
            {
                ICommand command = _commandFactory.Create(commandInfo.CommandType);
                commandResult = command.Execute(commandInfo);
            }
            else
            {
                commandResult = new CommandResult(commandInfo, validationResult);
            }

            return commandResult;
        }
    }
}
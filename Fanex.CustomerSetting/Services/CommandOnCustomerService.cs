namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.CommandTypes;
    using Fanex.CustomerSetting.Models;

    public class CommandOnCustomerService : ICommandOnCustomerService
    {
        private IValidationService _validationService;

        public CommandOnCustomerService()
            : this(null)
        {
        }

        public CommandOnCustomerService(IValidationService validationService)
        {
            _validationService = validationService ?? new ValidationService();
        }

        public CommandResult Do(CommandInfo commandInfo)
        {
            CommandResult commandResult = null;
            Result validationResult = _validationService.Validate(commandInfo);
            if (validationResult.IsSuccess)
            {
                ICommand command = CommandFactory.Get(commandInfo.CommandType);
                commandResult = command.Do(commandInfo);
            }
            else
            {
                commandResult = new CommandResult(commandInfo, validationResult);
            }

            return commandResult;
        }
    }
}
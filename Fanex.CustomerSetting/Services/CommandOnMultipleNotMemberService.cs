namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.CommandTypes;
    using Fanex.CustomerSetting.Models;

    public class CommandOnMultipleNotMemberService : ICommandOnMultipleCustomerService
    {
        private IValidationService _validationService;

        public CommandOnMultipleNotMemberService()
            : this(null)
        {
        }

        public CommandOnMultipleNotMemberService(IValidationService validationService)
        {
            _validationService = validationService ?? new ValidationService();
        }

        public IList<CommandResult> Do(MultipleCommandInfo multipleCommandInfo)
        {
            var commandResults = new List<CommandResult>();
            while (true)
            {
                var commandInfo = multipleCommandInfo.Take();
                if (commandInfo == null)
                {
                    break;
                }

                CommandResult commandResult = null;
                var validationResult = _validationService.Validate(commandInfo);
                if (validationResult.IsSuccess)
                {
                    ICommand command = CommandFactory.Get(commandInfo.CommandType);
                    commandResult = command.Do(commandInfo);
                }
                else
                {
                    commandResult = new CommandResult(commandInfo.CommandType, validationResult);
                }

                commandResults.Add(commandResult);
            }

            return commandResults;
        }
    }
}
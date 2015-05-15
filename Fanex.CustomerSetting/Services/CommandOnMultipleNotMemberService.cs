namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;

    public class CommandOnMultipleNotMemberService : ICommandOnMultipleCustomerService
    {
        private readonly ICommandOnCustomerService _commandOnCustomerService;

        public CommandOnMultipleNotMemberService()
            : this(null)
        {
        }

        public CommandOnMultipleNotMemberService(ICommandOnCustomerService commandOnCustomerService)
        {
            _commandOnCustomerService = commandOnCustomerService ?? new CommandOnCustomerService();
        }

        public IList<CommandResult> Execute(MultipleCommandInfo multipleCommandInfo)
        {
            var commandResults = new List<CommandResult>();
            while (true)
            {
                var commandInfo = multipleCommandInfo.Take();
                if (commandInfo == null)
                {
                    break;
                }

                var commandResult = _commandOnCustomerService.Execute(commandInfo);
                commandResults.Add(commandResult);
            }

            return commandResults;
        }
    }
}
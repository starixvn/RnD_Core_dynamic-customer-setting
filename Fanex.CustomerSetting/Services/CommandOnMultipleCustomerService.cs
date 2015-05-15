namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;

    public class CommandOnMultipleCustomerService : ICommandOnMultipleCustomerService
    {
        private readonly ICommandOnMultipleCustomerServiceFactory _commandOnMultipleCustomerServiceFactory;

        public CommandOnMultipleCustomerService()
            : this(null)
        {
        }

        public CommandOnMultipleCustomerService(CommandOnMultipleCustomerServiceFactory commandOnMultipleCustomerServiceFactory)
        {
            _commandOnMultipleCustomerServiceFactory = commandOnMultipleCustomerServiceFactory ?? new CommandOnMultipleCustomerServiceFactory();
        }

        public IList<CommandResult> Execute(MultipleCommandInfo multipleCommandInfo)
        {
            var commandOnMultipleCustomerService = _commandOnMultipleCustomerServiceFactory.Create(multipleCommandInfo);
            return commandOnMultipleCustomerService.Execute(multipleCommandInfo);
        }
    }
}
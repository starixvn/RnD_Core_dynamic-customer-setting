namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.FormTypes;
    using Fanex.CustomerSetting.Models;

    public class CommandOnMultipleCustomerService : ICommandOnMultipleCustomerService
    {
        public IList<CommandResult> Do(MultipleCommandInfo multipleCommandInfo)
        {
            var formTypeHandler = FormHandlerFactory.Create(multipleCommandInfo.FormType);
            ICommandOnMultipleCustomerService commandOnMultipleMemberService = new CommandOnMultipleMemberService();
            ICommandOnMultipleCustomerService commandOnMultipleNotMemberService = new CommandOnMultipleNotMemberService();
            ICommandOnMultipleCustomerService commandService = formTypeHandler.IsCommandOnMember
                ? commandOnMultipleMemberService
                : commandOnMultipleNotMemberService;
            return commandService.Do(multipleCommandInfo);
        }
    }
}
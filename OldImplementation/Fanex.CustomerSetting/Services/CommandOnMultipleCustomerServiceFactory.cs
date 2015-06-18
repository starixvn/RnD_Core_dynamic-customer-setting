namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.FormTypes;
    using Fanex.CustomerSetting.Models;

    public class CommandOnMultipleCustomerServiceFactory : ICommandOnMultipleCustomerServiceFactory
    {
        private readonly FormHandlerFactory _formHanderFactory;
        private readonly CommandOnMultipleMemberService _commandOnMultipleMemberService;
        private readonly CommandOnMultipleNotMemberService _commandOnMultipleNotMemberService;

        public CommandOnMultipleCustomerServiceFactory()
            : this(null, null, null)
        {
        }

        public CommandOnMultipleCustomerServiceFactory(
            FormHandlerFactory formHanderFactory,
            CommandOnMultipleMemberService commandOnMultipleMemberService,
            CommandOnMultipleNotMemberService commandOnMultipleNotMemberService)
        {
            _formHanderFactory = formHanderFactory ?? new FormHandlerFactory();
            _commandOnMultipleMemberService = commandOnMultipleMemberService ?? new CommandOnMultipleMemberService();
            _commandOnMultipleNotMemberService = commandOnMultipleNotMemberService ?? new CommandOnMultipleNotMemberService();
        }

        public ICommandOnMultipleCustomerService Create(MultipleCommandInfo multipleCommandInfo)
        {
            var formTypeHandler = _formHanderFactory.Create(multipleCommandInfo.FormType);
            if (formTypeHandler.IsCommandOnMember)
            {
                return _commandOnMultipleMemberService;
            }
            else
            {
                return _commandOnMultipleNotMemberService;
            }
        }
    }
}
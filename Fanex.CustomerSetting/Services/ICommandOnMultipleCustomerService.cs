namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Models;

    public interface ICommandOnMultipleCustomerService
    {
        IList<CommandResult> Do(MultipleCommandInfo multipleCommandInfo);
    }
}
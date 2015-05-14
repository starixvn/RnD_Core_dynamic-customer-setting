namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    public interface ICommandOnCustomerService
    {
        CommandResult Do(CommandInfo commandInfo);
    }
}
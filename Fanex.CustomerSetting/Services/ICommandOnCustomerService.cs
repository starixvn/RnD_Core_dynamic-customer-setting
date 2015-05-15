namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Models;

    public interface ICommandOnCustomerService
    {
        CommandResult Execute(CommandInfo commandInfo);
    }
}
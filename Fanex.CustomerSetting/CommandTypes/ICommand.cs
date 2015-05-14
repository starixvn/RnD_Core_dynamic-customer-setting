namespace Fanex.CustomerSetting.CommandTypes
{
    using Fanex.CustomerSetting.Models;

    public interface ICommand
    {
        CommandResult Do(CommandInfo commandInfo);
    }
}
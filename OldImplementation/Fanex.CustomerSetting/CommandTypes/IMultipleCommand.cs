namespace Fanex.CustomerSetting.CommandTypes
{
    using Fanex.CustomerSetting.Models;

    public interface IMultipleCommand
    {
        CommandResult Execute(MultipleCommandInfo multipleCommandInfo);
    }
}
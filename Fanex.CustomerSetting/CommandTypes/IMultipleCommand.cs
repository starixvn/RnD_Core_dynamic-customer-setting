namespace Fanex.CustomerSetting.CommandTypes
{
    using Fanex.CustomerSetting.Models;

    public interface IMultipleCommand
    {
        CommandResult Do(MultipleCommandInfo multipleCommandInfo);
    }
}
namespace Fanex.CustomerSetting.CommandTypes
{
    using Fanex.CustomerSetting.Enumerations;

    public interface ICommandFactory
    {
        ICommand Create(CommandType commandType);
    }
}
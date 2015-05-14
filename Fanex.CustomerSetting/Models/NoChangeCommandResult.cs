namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Enumerations;

    public class NoChangeCommandResult : CommandResult
    {
        public NoChangeCommandResult(CommandType commandType)
            : base(commandType, Result.NoChange)
        {
        }
    }
}
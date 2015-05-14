namespace Fanex.CustomerSetting.CommandTypes
{
    using Fanex.CustomerSetting.Models;

    public class AllNotPositionTakingCommand : BaseCommand, ICommand
    {
        public CommandResult Do(CommandInfo commandInfo)
        {
            ICommand statusCommand = new StatusCommand();
            CommandResult statusResult = statusCommand.Do(commandInfo);
            ICommand discountCommand = new DiscountCommand();
            CommandResult discountResult = discountCommand.Do(commandInfo);
            ICommand betLimitCommand = new BetLimitCommand();
            CommandResult betLimitResult = betLimitCommand.Do(commandInfo);
            return new CommandResult(statusResult, discountResult, betLimitResult, null);
        }
    }
}
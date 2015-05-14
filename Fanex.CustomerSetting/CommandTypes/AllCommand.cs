namespace Fanex.CustomerSetting.CommandTypes
{
    using Fanex.CustomerSetting.Models;

    public class AllCommand : BaseCommand, ICommand
    {
        public CommandResult Do(CommandInfo commandInfo)
        {
            ICommand statusCommand = new StatusCommand();
            CommandResult statusResult = statusCommand.Do(commandInfo);
            ICommand discountCommand = new DiscountCommand();
            CommandResult discountResult = discountCommand.Do(commandInfo);
            ICommand betLimitCommand = new BetLimitCommand();
            CommandResult betLimitResult = betLimitCommand.Do(commandInfo);
            ICommand positionTakingCommand = new PositionTakingCommand();
            CommandResult positionTakingResult = positionTakingCommand.Do(commandInfo);
            return new CommandResult(statusResult, discountResult, betLimitResult, positionTakingResult);
        }
    }
}
namespace Fanex.CustomerSetting.CommandTypes
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Models;

    public class AllNotPositionTakingCommand : BaseCommand, ICommand
    {
        public CommandResult Do(CommandInfo commandInfo)
        {
            ICommand statusCommand = new StatusCommand();
            var commandResult = statusCommand.Do(commandInfo);
            var statusResult = commandResult.SettingResults.First().Value;
            ICommand discountCommand = new DiscountCommand();
            commandResult = discountCommand.Do(commandInfo);
            var discountResult = commandResult.SettingResults.First().Value;
            ICommand betLimitCommand = new BetLimitCommand();
            commandResult = betLimitCommand.Do(commandInfo);
            var betLimitResult = commandResult.SettingResults.First().Value;
            var settingResults = new List<SettingResult>
            {
                statusResult,
                discountResult,
                betLimitResult
            };

            return new CommandResult(commandInfo, settingResults);
        }
    }
}
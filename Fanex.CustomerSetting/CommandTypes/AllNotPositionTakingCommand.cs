namespace Fanex.CustomerSetting.CommandTypes
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class AllNotPositionTakingCommand : BaseCommand, ICommand
    {
        public CommandResult Do(CommandInfo commandInfo)
        {
            ICommand statusCommand = new StatusCommand();
            var commandResult = statusCommand.Do(commandInfo);
            var statusResult = commandResult[SettingType.Status];
            ICommand discountCommand = new DiscountCommand();
            commandResult = discountCommand.Do(commandInfo);
            var discountResult = commandResult[SettingType.Discount];
            ICommand betLimitCommand = new BetLimitCommand();
            commandResult = betLimitCommand.Do(commandInfo);
            var betLimitResult = commandResult[SettingType.BetLimit];
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
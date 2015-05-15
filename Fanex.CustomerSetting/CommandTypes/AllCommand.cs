namespace Fanex.CustomerSetting.CommandTypes
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class AllCommand : BaseCommand, ICommand
    {
        public CommandResult Execute(CommandInfo commandInfo)
        {
            ICommand statusCommand = new StatusCommand();
            var commandResult = statusCommand.Execute(commandInfo);
            var statusResult = commandResult[SettingType.Status];
            ICommand discountCommand = new DiscountCommand();
            commandResult = discountCommand.Execute(commandInfo);
            var discountResult = commandResult[SettingType.Discount];
            ICommand betLimitCommand = new BetLimitCommand();
            commandResult = betLimitCommand.Execute(commandInfo);
            var betLimitResult = commandResult[SettingType.BetLimit];
            ICommand positionTakingCommand = new PositionTakingCommand();
            commandResult = positionTakingCommand.Execute(commandInfo);
            var positionTakingResult = commandResult[SettingType.PositionTaking];
            var settingResults = new List<SettingResult>
            {
                statusResult,
                discountResult,
                betLimitResult,
                positionTakingResult
            };

            return new CommandResult(commandInfo, settingResults);
        }
    }
}
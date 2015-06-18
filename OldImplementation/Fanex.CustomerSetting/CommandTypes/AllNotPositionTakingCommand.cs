namespace Fanex.CustomerSetting.CommandTypes
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;
    using Fanex.CustomerSetting.Services;

    public class AllNotPositionTakingCommand : BaseCommand, ICommand
    {
        private readonly ICommandRepository _commandRepository;
        private readonly IMessageService _messageService;

        public AllNotPositionTakingCommand()
            : this(null, null)
        {
        }

        public AllNotPositionTakingCommand(ICommandRepository commandRepository, IMessageService messageService)
            : base(messageService)
        {
            _commandRepository = commandRepository ?? new CommandRepository();
            _messageService = messageService ?? new MessageService();
        }

        public CommandResult Execute(CommandInfo commandInfo)
        {
            ICommand statusCommand = new StatusCommand(_commandRepository, _messageService);
            var commandResult = statusCommand.Execute(commandInfo);
            var statusResult = commandResult[SettingType.Status];
            ICommand discountCommand = new DiscountCommand(_commandRepository, _messageService);
            commandResult = discountCommand.Execute(commandInfo);
            var discountResult = commandResult[SettingType.Discount];
            ICommand betLimitCommand = new BetLimitCommand(_commandRepository, _messageService);
            commandResult = betLimitCommand.Execute(commandInfo);
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
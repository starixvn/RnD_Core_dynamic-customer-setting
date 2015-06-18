namespace Fanex.CustomerSetting.CommandTypes
{
    using System.Linq;
    using Fanex.CustomerSetting.Converters;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;
    using Fanex.CustomerSetting.Services;

    public class DiscountCommand : BaseCommand, ICommand
    {
        private readonly ICommandRepository _commandRepository;

        public DiscountCommand()
            : this(null, null)
        {
        }

        public DiscountCommand(ICommandRepository commandRepository, IMessageService messageService)
            : base(messageService)
        {
            _commandRepository = commandRepository ?? new CommandRepository();
        }

        public CommandResult Execute(CommandInfo commandInfo)
        {
            var settingType = SettingType.Discount;
            if (commandInfo.Discounts == null || commandInfo.Discounts.Count() == 0)
            {
                return new CommandResult(commandInfo, new NoChangeSettingResult(settingType));
            }

            var xmlConverter = new DiscountXmlConverter();
            var commandCriteria = new DiscountCommandCriteria
            {
                UserId = commandInfo.CustId,
                SubAccId = commandInfo.SubAccountId,
                CustId = commandInfo.TargetCustId,
                Discount = xmlConverter.Generate(commandInfo.Discounts),
                Ip = commandInfo.Ip
            };
            if (!commandCriteria.IsValid())
            {
                throw new CriteriaException(commandCriteria);
            }

            _commandRepository.UpdateDiscount(commandCriteria);
            var settingResult = Process(commandCriteria, commandInfo, settingType);
            return new CommandResult(commandInfo, settingResult);
        }
    }
}
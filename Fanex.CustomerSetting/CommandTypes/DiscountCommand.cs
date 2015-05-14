namespace Fanex.CustomerSetting.CommandTypes
{
    using System.Linq;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;

    public class DiscountCommand : BaseCommand, ICommand
    {
        public CommandResult Do(CommandInfo commandInfo)
        {
            var settingType = SettingType.Discount;
            if (commandInfo.Discounts == null || commandInfo.Discounts.Count() == 0)
            {
                return new CommandResult(commandInfo, new NoChangeSettingResult(settingType));
            }

            var commandCriteria = new DiscountCommandCriteria
            {
                UserId = commandInfo.CustId,
                SubAccId = commandInfo.SubAccountId,
                CustId = commandInfo.TargetCustId,
                Discount = _xmlGenerator.GenerateDiscount(commandInfo.Discounts),
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
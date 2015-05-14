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
            if (commandInfo.Discounts == null || commandInfo.Discounts.Count() == 0)
            {
                return new NoChangeCommandResult(CommandType.Discount);
            }

            var criteria = new DiscountCommandCriteria
            {
                UserId = commandInfo.CustId,
                SubAccId = commandInfo.SubAccountId,
                CustId = commandInfo.TargetCustId,
                Discount = _xmlGenerator.GenerateDiscount(commandInfo.Discounts),
                Ip = commandInfo.Ip
            };
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            _commandRepository.UpdateDiscount(criteria);
            return Process(criteria, commandInfo, SettingType.Discount);
        }
    }
}
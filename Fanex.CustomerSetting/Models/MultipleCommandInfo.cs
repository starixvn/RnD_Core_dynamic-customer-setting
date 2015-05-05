namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// MultipleCommandInfo class.
    /// </summary>
    public class MultipleCommandInfo : CommandInfo
    {
        public IEnumerable<int> TargetCustIds { get; set; }

        /// <summary>
        /// Take a single command information.
        /// </summary>
        /// <returns>Returns CommandInfo object.</returns>
        public CommandInfo Take()
        {
            if (TargetCustIds.Count() == 0)
            {
                return null;
            }

            var targetCustId = TargetCustIds.First();
            TargetCustIds = TargetCustIds.Skip(1);
            return new CommandInfo
            {
                CustId = this.CustId,
                RoleId = this.RoleId,
                TargetCustId = targetCustId,
                TargetRoleId = TargetRoleId,
                ProductId = this.ProductId,
                FormType = this.FormType,
                Language = this.Language,
                SubAccountId = this.SubAccountId,
                Ip = this.Ip,
                CommandType = this.CommandType,
                StatusLevel = this.StatusLevel,
                Statuses = this.Statuses,
                DiscountLevel = this.DiscountLevel,
                Discounts = this.Discounts,
                BetLimitLevel = this.BetLimitLevel,
                BetLimits = this.BetLimits,
                PositionTakingLevel = this.PositionTakingLevel,
                PositionTakings = this.PositionTakings
            };
        }
    }
}
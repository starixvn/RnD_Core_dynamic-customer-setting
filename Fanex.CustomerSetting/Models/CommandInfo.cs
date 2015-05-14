namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;

    public class CommandInfo : QueryInfo
    {
        public int SubAccountId { get; set; }

        public CommandType CommandType { get; set; }

        public string Ip { get; set; }

        public SettingLevel StatusLevel { get; set; }

        public IEnumerable<ProductStatusEntity> Statuses { get; set; }

        public SettingLevel DiscountLevel { get; set; }

        public IEnumerable<DiscountEntity> Discounts { get; set; }

        public SettingLevel BetLimitLevel { get; set; }

        public IEnumerable<BetLimitEntity> BetLimits { get; set; }

        public SettingLevel PositionTakingLevel { get; set; }

        public IEnumerable<PositionTakingEntity> PositionTakings { get; set; }
    }
}
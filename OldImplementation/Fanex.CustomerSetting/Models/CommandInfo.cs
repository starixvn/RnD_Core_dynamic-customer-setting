namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;

    public class CommandInfo : QueryInfo
    {
        public CommandInfo()
        {
        }

        public CommandInfo(CommandInfo commandInfo)
            : base(commandInfo)
        {
            this.SubAccountId = commandInfo.SubAccountId;
            this.CommandType = commandInfo.CommandType;
            this.Ip = commandInfo.Ip;
            this.StatusLevel = commandInfo.StatusLevel;
            this.Statuses = commandInfo.Statuses;
            this.DiscountLevel = commandInfo.DiscountLevel;
            this.Discounts = commandInfo.Discounts;
            this.BetLimitLevel = commandInfo.BetLimitLevel;
            this.BetLimits = commandInfo.BetLimits;
            this.PositionTakingLevel = commandInfo.PositionTakingLevel;
            this.PositionTakings = commandInfo.PositionTakings;
        }

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
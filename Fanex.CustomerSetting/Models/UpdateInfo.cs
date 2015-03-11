namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;

    /// <summary>
    /// UpdateInfo class.
    /// </summary>
    public class UpdateInfo : SelectInfo
    {
        public int SubAccountId { get; set; }

        public SettingType SettingType { get; set; }

        public string Ip { get; set; }

        public string UserName { get; set; }

        public bool IsMultiple { get; set; }

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
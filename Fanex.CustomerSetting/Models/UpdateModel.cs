namespace Fanex.CustomerSetting.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;

    /// <summary>
    /// UpdateModel class.
    /// </summary>
    public class UpdateModel : BaseModel
    {
        public int SubAccountId { get; set; }

        public SettingType SettingType { get; set; }

        public string Ip { get; set; }

        public string StatusString { get; set; }

        public string DiscountString { get; set; }

        public string BetLimitString { get; set; }

        public string PositionTakingString { get; set; }

        public IEnumerable<ProductStatusEntity> Statuses { get; set; }

        public IEnumerable<DiscountEntity> Discounts { get; set; }

        public IEnumerable<BetLimitEntity> BetLimits { get; set; }

        public IEnumerable<PositionTakingEntity> PositionTakings { get; set; }
    }
}
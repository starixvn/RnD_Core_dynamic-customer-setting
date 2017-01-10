namespace Fanex.CustomerSetting.SettingType12.Models
{
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Results;
    using Resources;
    using Utilities;

    public class LossLimitItem : SettingItemDefault
    {
        private const string LossLimitXmlFormat = "<Limit {0} MaxLossLimit=\"{1}\" IsAutoReset=\"{2}\" />";

        public decimal LossLimit { get; set; }

        public decimal MaxLossRange { get; set; }

        public decimal MinLossRange { get; set; }

        public bool IsAutoReset { get; set; }

        public bool IsAutoResetUpline { get; set; }

        public override IResult Validate()
        {
            if ((MaxLossRange > 0 && (LossLimit < MinLossRange || MaxLossRange < LossLimit))
                || LossLimit < 0)
            {
                var errorMessage = string.Format(
                        Message.MaximumLossValueShouldBeInRange,
                        NumberFormatter.FormatDecimal(MinLossRange),
                        NumberFormatter.FormatDecimal(MaxLossRange));

                return new Result(ErrorCode.MaximumLossValueShouldBeInRange, errorMessage);
            }

            if (!IsAutoResetUpline && IsAutoReset)
            {
                return new Result(ErrorCode.AutoResetIsViolatedWithUpline, Message.AutoResetIsViolatedWithUpline);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(LossLimitXmlFormat, base.ToXml(), LossLimit, IsAutoReset ? 1 : 0);
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as LossLimitItem;

            if (otherObj == null)
            {
                return false;
            }
            else
            {
                return Equals(SportTypeId, otherObj.SportTypeId)
                    && Equals(BetTypeId, otherObj.BetTypeId)
                    && Equals(LossLimit, otherObj.LossLimit)
                    && Equals(MaxLossRange, otherObj.MaxLossRange)
                    && Equals(MinLossRange, otherObj.MinLossRange)
                    && Equals(IsAutoReset, otherObj.IsAutoReset)
                    && Equals(IsAutoReset, otherObj.IsAutoResetUpline);
            }
        }

        public override int GetHashCode()
        {
            return SportTypeId.GetHashCode()
                ^ BetTypeId.GetHashCode()
                ^ LossLimit.GetHashCode()
                ^ MaxLossRange.GetHashCode()
                ^ MinLossRange.GetHashCode()
                ^ IsAutoReset.GetHashCode()
                ^ IsAutoResetUpline.GetHashCode();
        }
    }
}
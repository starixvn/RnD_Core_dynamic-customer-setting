namespace Fanex.CustomerSetting.SettingType6.Models
{
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType6.Constants;
    using Fanex.CustomerSetting.SettingType6.Resources;
    using Fanex.CustomerSetting.SettingType6.Utilities;

    public class WinLossLimitItem : SettingItemDefault
    {
        public decimal WinLimit { get; set; }

        public decimal MaxWinRange { get; set; }

        public decimal LossLimit { get; set; }

        public decimal MaxLossRange { get; set; }

        public decimal MinLossRange { get; set; }

        public bool IsAutoReset { get; set; }

        public override IResult Validate()
        {
            if (WinLimit < 0)
            {
                return new Result(ErrorCode.MaximumWinMustNotBeLessThanZero, Message.MaximumWinShouldNotBeLessThanZero);
            }

            if (0 < MaxWinRange && MaxWinRange < WinLimit)
            {
                return new Result(ErrorCode.MaximumWinMustBotExceedThanUplineMaximumWin, Message.MaximumWinShouldBotExceedThanUplineMaximumWin);
            }

            if (LossLimit < MinLossRange || (0 < MaxLossRange && MaxLossRange < LossLimit))
            {
                return new Result(
                    ErrorCode.MaximumLossValueMustBeInRange,
                    string.Format(
                        Message.MaximumLossValueShouldBeInRange,
                        NumberFormatter.FormatDecimal(MinLossRange),
                        NumberFormatter.FormatDecimal(MaxLossRange)));
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return $"<Limit {base.ToXml()} MaxWinLimit=\"{WinLimit}\" MaxLossLimit=\"{LossLimit}\" IsAutoReset=\"{(IsAutoReset ? 1 : 0)}\" />";
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as WinLossLimitItem;

            if (otherObj == null)
            {
                return false;
            }
            else
            {
                return Equals(SportTypeId, otherObj.SportTypeId)
                    && Equals(BetTypeId, otherObj.BetTypeId)
                    && Equals(WinLimit, otherObj.WinLimit)
                    && Equals(MaxWinRange, otherObj.MaxWinRange)
                    && Equals(LossLimit, otherObj.LossLimit)
                    && Equals(MaxLossRange, otherObj.MaxLossRange)
                    && Equals(MinLossRange, otherObj.MinLossRange)
                    && Equals(IsAutoReset, otherObj.IsAutoReset);
            }
        }

        public override int GetHashCode()
        {
            return SportTypeId.GetHashCode()
                ^ BetTypeId.GetHashCode()
                ^ WinLimit.GetHashCode()
                ^ MaxWinRange.GetHashCode()
                ^ LossLimit.GetHashCode()
                ^ MaxLossRange.GetHashCode()
                ^ MinLossRange.GetHashCode()
                ^ IsAutoReset.GetHashCode();
        }
    }
}
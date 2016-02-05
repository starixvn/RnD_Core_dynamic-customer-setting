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

        public bool IsAutoResetUpline { get; set; }

        public override IResult Validate()
        {
            if (WinLimit < 0)
            {
                return new Result(ErrorCode.MaximumWinShouldNotBeLessThanZero, Message.MaximumWinShouldNotBeLessThanZero);
            }

            if (0 < MaxWinRange && MaxWinRange < WinLimit)
            {
                return new Result(ErrorCode.MaximumWinShouldNotExceedThanUplineMaximumWin, Message.MaximumWinShouldBotExceedThanUplineMaximumWin);
            }

            if ((0 < MaxLossRange && (LossLimit < MinLossRange || MaxLossRange < LossLimit))
                || LossLimit < 0)
            {
                return new Result(
                    ErrorCode.MaximumLossValueShouldBeInRange,
                    string.Format(
                        Message.MaximumLossValueShouldBeInRange,
                        NumberFormatter.FormatDecimal(MinLossRange),
                        NumberFormatter.FormatDecimal(MaxLossRange)));
            }

            if (!IsAutoResetUpline && IsAutoReset)
            {
                return new Result(ErrorCode.AutoResetIsViolatedWithUpline, Message.AutoResetIsViolatedWithUpline);
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
                    && Equals(IsAutoReset, otherObj.IsAutoReset)
                    && Equals(IsAutoReset, otherObj.IsAutoResetUpline);
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
                ^ IsAutoReset.GetHashCode()
                ^ IsAutoResetUpline.GetHashCode();
        }
    }
}
namespace Fanex.CustomerSetting.SettingType22.Models
{
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Results;
    using Resources;
    using Utilities;

    public class WinLossLimitItem : SettingItemDefault
    {
        private const string WinLossLimitXmlFormat = "<Limit {0} MaxWinLimit=\"{1}\" MaxLossLimit=\"{2}\" IsAutoReset=\"{3}\" />";

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
                return new Result(ErrorCodes.MaximumWinShouldNotBeLessThanZero, Message.MaximumWinShouldNotBeLessThanZero);
            }

            if (MaxWinRange > 0 && MaxWinRange < WinLimit)
            {
                return new Result(ErrorCodes.MaximumWinShouldNotExceedThanUplineMaximumWin, Message.MaximumWinShouldBotExceedThanUplineMaximumWin);
            }

            if ((MaxLossRange > 0 && (LossLimit < MinLossRange || MaxLossRange < LossLimit))
                || LossLimit < 0)
            {
                var errorMessage = string.Format(
                        Message.MaximumLossValueShouldBeInRange,
                        NumberFormatter.FormatDecimal(MinLossRange),
                        NumberFormatter.FormatDecimal(MaxLossRange));

                return new Result(ErrorCodes.MaximumLossValueShouldBeInRange, errorMessage);
            }

            if (!IsAutoResetUpline && IsAutoReset)
            {
                return new Result(ErrorCodes.AutoResetIsViolatedWithUpline, Message.AutoResetIsViolatedWithUpline);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(WinLossLimitXmlFormat, base.ToXml(), WinLimit, LossLimit, IsAutoReset ? 1 : 0);
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as WinLossLimitItem;

            if (otherObj == null)
            {
                return false;
            }

#pragma warning disable S1067 // Expressions should not be too complex
            return Equals(SportTypeId, otherObj.SportTypeId)
                && Equals(BetTypeId, otherObj.BetTypeId)
                && Equals(WinLimit, otherObj.WinLimit)
                && Equals(MaxWinRange, otherObj.MaxWinRange)
                && Equals(LossLimit, otherObj.LossLimit)
                && Equals(MaxLossRange, otherObj.MaxLossRange)
                && Equals(MinLossRange, otherObj.MinLossRange)
                && Equals(IsAutoReset, otherObj.IsAutoReset)
                && Equals(IsAutoReset, otherObj.IsAutoResetUpline);
#pragma warning restore S1067 // Expressions should not be too complex
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
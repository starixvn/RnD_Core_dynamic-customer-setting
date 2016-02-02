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
                return new Result(ErrorCode.MaximumWinMustNotBeLessThanZero, Message.MaximumWinMustNotBeLessThanZero);
            }

            if (WinLimit > MaxWinRange)
            {
                return new Result(ErrorCode.MaximumWinMustBotExceedThanUplineMaximumWin, Message.MaximumWinMustBotExceedThanUplineMaximumWin);
            }

            if (MinLossRange > LossLimit || LossLimit > MaxLossRange)
            {
                return new Result(
                    ErrorCode.MaximumLossValueMustBeInRange,
                    string.Format(
                        Message.MaximumLossValueMustBeInRange,
                        NumberFormatter.FormatDecimal(MinLossRange),
                        NumberFormatter.FormatDecimal(MaxLossRange)));
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return $"<Limit {base.ToXml()} MaxWinLimit=\"{WinLimit}\" MaxLossLimit=\"{LossLimit}\" IsAutoReset=\"{(IsAutoReset ? 1 : 0)}\" />";
        }
    }
}
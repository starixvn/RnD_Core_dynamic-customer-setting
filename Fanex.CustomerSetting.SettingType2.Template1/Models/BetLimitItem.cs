namespace Fanex.CustomerSetting.SettingType2.Template1.Models
{
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType2.Template1.Constants;
    using Fanex.CustomerSetting.SettingType2.Template1.Resources;

    public class BetLimitItem : DefaultSettingItem
    {
        private const string BetLimitXmlFormat = "<BetLimit {0} MinBet=\"{1}\" MaxBet=\"{2}\" MaxPerMatch=\"{3}\"/>";

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public BetLimitItem Constraint { get; set; }

        public override IResult Validate()
        {
            if (MinBet == 0
                && MaxBet == 0
                && MaxPerMatch == 0)
            {
                return Result.Success;
            }

            if (MinBet > SettingType2Constants.MaxBetLimitValue
                || MaxBet > SettingType2Constants.MaxBetLimitValue
                || MaxPerMatch > SettingType2Constants.MaxBetLimitValue)
            {
                return new Result(
                    SettingType2Constants.BetLimitExceedMaxValueErrorCode,
                    BetLimitError.BetLimitCannotBeGreaterThanMaxValue);
            }

            if (MinBet < 0)
            {
                return new Result(
                    SettingType2Constants.MinBetIsNegativeErrorCode,
                    BetLimitError.MinBetCannotBeNegative);
            }

            if (MaxBet < 0)
            {
                return new Result(
                    SettingType2Constants.MaxBetIsNegativeErrorCode,
                    BetLimitError.MaxBetCannotBeNegative);
            }

            if (MaxPerMatch < 0)
            {
                return new Result(
                    SettingType2Constants.MaxPerMatchIsNegativeErrorCode,
                    BetLimitError.MaxPerMatchCannotBeNegative);
            }

            if (MinBet > MaxBet)
            {
                return new Result(
                    SettingType2Constants.MinBetIsGreaterThanMaxBetErrorCode,
                    BetLimitError.MinBetCannotBeGreaterThanMaxBet);
            }

            if (MaxBet > MaxPerMatch)
            {
                return new Result(
                    SettingType2Constants.MaxBetIsGreaterThanMaxPerMatchErrorCode,
                    BetLimitError.MaxBetCannotBeGreaterThanMaxPerMatch);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(
                BetLimitXmlFormat,
                base.ToXml(),
                MinBet,
                MaxBet,
                MaxPerMatch);
        }
    }
}
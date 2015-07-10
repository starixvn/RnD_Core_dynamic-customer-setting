namespace Fanex.CustomerSetting.SettingType2.Template1.Models
{
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType2.Template1.Constants;
    using Fanex.CustomerSetting.SettingType2.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType2.Template1.Resources;

    public class BetLimitItem : DefaultSettingItem
    {
        private const string BetLimitXmlFormat = "<BetLimit {0} MinBet=\"{1}\" MaxBet=\"{2}\" MaxPerMatch=\"{3}\"/>";

        public BetLimitItem()
        {
        }

        public BetLimitItem(BetLimitItemDto betLimitItemDto)
        {
            SportTypeId = betLimitItemDto.SportTypeId;
            BetTypeId = betLimitItemDto.BetTypeId;
            MaxBet = betLimitItemDto.MaxBet;
            MinBet = betLimitItemDto.MinBet;
            MaxPerMatch = betLimitItemDto.MaxPerMatch;
            Constraint = new BetLimitItem
            {
                MinBet = betLimitItemDto.MinBetMin,
                MaxBet = betLimitItemDto.MaxBetMax,
                MaxPerMatch = betLimitItemDto.MaxPerMatchMax
            };
        }

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

            if (MinBet > BetLimitConstants.MaxBetLimitValue
                || MaxBet > BetLimitConstants.MaxBetLimitValue
                || MaxPerMatch > BetLimitConstants.MaxBetLimitValue)
            {
                return new Result(
                    BetLimitErrorCode.BetLimitExceedMaxValue,
                    BetLimitMessage.BetLimitCannotBeGreaterThanMaxValue);
            }

            if (MinBet < 0)
            {
                return new Result(
                    BetLimitErrorCode.MinBetIsNegative,
                    BetLimitMessage.MinBetCannotBeNegative);
            }

            if (MaxBet < 0)
            {
                return new Result(
                    BetLimitErrorCode.MaxBetIsNegative,
                    BetLimitMessage.MaxBetCannotBeNegative);
            }

            if (MaxPerMatch < 0)
            {
                return new Result(
                    BetLimitErrorCode.MaxPerMatchIsNegative,
                    BetLimitMessage.MaxPerMatchCannotBeNegative);
            }

            if (MinBet > MaxBet)
            {
                return new Result(
                    BetLimitErrorCode.MinBetIsGreaterThanMaxBet,
                    BetLimitMessage.MinBetCannotBeGreaterThanMaxBet);
            }

            if (MaxBet > MaxPerMatch)
            {
                return new Result(
                    BetLimitErrorCode.MaxBetIsGreaterThanMaxPerMatch,
                    BetLimitMessage.MaxBetCannotBeGreaterThanMaxPerMatch);
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
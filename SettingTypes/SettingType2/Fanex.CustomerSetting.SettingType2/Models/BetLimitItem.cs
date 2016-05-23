namespace Fanex.CustomerSetting.SettingType2.Models
{
    using Common.Helper;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Results;
    using Dtos;
    using Resources;

    public class BetLimitItem : SettingItemDefault
    {
        private const string BetLimitXmlFormat = "<BetLimit {0} MinBet=\"{1}\" MaxBet=\"{2}\" MaxPerMatch=\"{3}\"/>";

        public BetLimitItem()
        {
        }

        internal BetLimitItem(BetLimitItemDto betLimitItemDto)
        {
            Check.Argument.IsNotNull(betLimitItemDto, "betLimitItemDto");

            SportTypeId = betLimitItemDto.SportTypeId;
            BetTypeId = betLimitItemDto.BetTypeId;
            MaxBet = betLimitItemDto.MaxBet;
            MinBet = betLimitItemDto.MinBet;
            MaxPerMatch = betLimitItemDto.MaxPerMatch;
            Upline = new BetLimitItem
            {
                MinBet = betLimitItemDto.MinBetMin,
                MaxBet = betLimitItemDto.MaxBetMax,
                MaxPerMatch = betLimitItemDto.MaxPerMatchMax
            };
        }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public BetLimitItem Upline { get; set; }

        public override IResult Validate()
        {
            if (MinBet > Constants.MaxBetLimitValue
                || MaxBet > Constants.MaxBetLimitValue
                || MaxPerMatch > Constants.MaxBetLimitValue)
            {
                return new Result(
                    ErrorCode.BetLimitExceedMaxValue,
                    Message.BetLimitCannotBeGreaterThanMaxValue);
            }

            if (MinBet < 0)
            {
                return new Result(
                    ErrorCode.MinBetIsNegative,
                    Message.MinBetCannotBeNegative);
            }

            if (MaxBet < 0)
            {
                return new Result(
                    ErrorCode.MaxBetIsNegative,
                    Message.MaxBetCannotBeNegative);
            }

            if (MaxPerMatch < 0)
            {
                return new Result(
                    ErrorCode.MaxPerMatchIsNegative,
                    Message.MaxPerMatchCannotBeNegative);
            }

            if (MinBet > MaxBet)
            {
                return new Result(
                    ErrorCode.MinBetIsGreaterThanMaxBet,
                    Message.MinBetCannotBeGreaterThanMaxBet);
            }

            if (MaxBet > MaxPerMatch)
            {
                return new Result(
                    ErrorCode.MaxBetIsGreaterThanMaxPerMatch,
                    Message.MaxBetCannotBeGreaterThanMaxPerMatch);
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
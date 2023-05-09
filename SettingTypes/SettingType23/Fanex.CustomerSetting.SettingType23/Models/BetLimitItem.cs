namespace Fanex.CustomerSetting.SettingType23.Models
{
    using Common.Helper;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Results;
    using Dtos;
    using Resources;

    public class BetLimitItem : SettingItemDefault
    {
        private const string BetLimitXmlFormat = "<BetLimit {0} MinBet=\"{1}\" MaxBet=\"{2}\" MaxPerMatch=\"{3}\" {4}/>";
        private const string MaxPerBallNullableXmlFormat = " MaxPerNumber=\"{0}\"";

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
            MaxPerBall = betLimitItemDto.MaxPerBall;
            SubType = betLimitItemDto.SubType;
            Upline = new BetLimitItem
            {
                SubType = betLimitItemDto.SubType,
                MinBet = betLimitItemDto.MinBetMin,
                MaxBet = betLimitItemDto.MaxBetMax,
                MaxPerMatch = betLimitItemDto.MaxPerMatchMax,
                MaxPerBall = betLimitItemDto.MaxPerBallMax
            };
        }

        public short SubType { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public decimal? MaxPerBall { get; set; }

        public BetLimitItem Upline { get; set; }

        public override IResult Validate()
        {
            if (MinBet > Constants.MaxBetLimitValue
                || MaxBet > Constants.MaxBetLimitValue
                || MaxPerMatch > Constants.MaxBetLimitValue)
            {
                return new Result(
                    ErrorCodes.BetLimitExceedMaxValue,
                    Message.BetLimitCannotBeGreaterThanMaxValue);
            }

            if (MinBet < 0)
            {
                return new Result(
                    ErrorCodes.MinBetIsNegative,
                    Message.MinBetCannotBeNegative);
            }

            if (MaxBet < 0)
            {
                return new Result(
                    ErrorCodes.MaxBetIsNegative,
                    Message.MaxBetCannotBeNegative);
            }

            if (MaxPerMatch < 0)
            {
                return new Result(
                    ErrorCodes.MaxPerMatchIsNegative,
                    Message.MaxPerMatchCannotBeNegative);
            }

            if (MaxPerBall < 0)
            {
                return new Result(
                    ErrorCodes.MaxPerBallIsNegative,
                    Message.MaxPerBallCannotBeNegative);
            }

            if (MinBet > MaxBet)
            {
                return new Result(
                    ErrorCodes.MinBetIsGreaterThanMaxBet,
                    Message.MinBetCannotBeGreaterThanMaxBet);
            }

            if (MaxBet > MaxPerMatch)
            {
                return new Result(
                    ErrorCodes.MaxBetIsGreaterThanMaxPerMatch,
                    Message.MaxBetCannotBeGreaterThanMaxPerMatch);
            }

            if (MaxBet > MaxPerBall)
            {
                return new Result(
                    ErrorCodes.MaxBetIsGreaterThanMaxPerBall,
                    Message.MaxBetCannotBeGreaterThanMaxPerBall);
            }

            if (MaxPerBall > MaxPerMatch)
            {
                return new Result(
                    ErrorCodes.MaxPerBallIsGreaterThanMaxPerMatch,
                    Message.MaxPerBallCannotBeGreaterThanMaxPerMatch);
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
                MaxPerMatch,
                MaxPerBall == null ? string.Empty : string.Format(MaxPerBallNullableXmlFormat, MaxPerBall));
        }
    }
}
namespace Fanex.CustomerSetting.SettingType16.Models
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

        internal BetLimitItem(BetLimitDto betLimitDto)
        {
            Check.Argument.IsNotNull(betLimitDto, nameof(betLimitDto));

            SportTypeId = betLimitDto.SportTypeId;
            BetTypeId = betLimitDto.BetTypeId;
            MaxBet = betLimitDto.MaxBet;
            MinBet = betLimitDto.MinBet;
            MaxPerMatch = betLimitDto.MaxPerMatch;
            Upline = new BetLimitItem
            {
                MinBet = betLimitDto.MinBetMin,
                MaxBet = betLimitDto.MaxBetMax,
                MaxPerMatch = betLimitDto.MaxPerMatchMax
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

        public override bool Equals(object obj)
        {
            var otherObj = obj as BetLimitItem;

            if (otherObj == null)
            {
                return false;
            }

            if (ReferenceEquals(otherObj, this))
            {
                return true;
            }

            if (SportTypeId == otherObj.SportTypeId
                && BetTypeId == otherObj.BetTypeId
                && MinBet == otherObj.MinBet
                && MaxBet == otherObj.MaxBet
                && MaxPerMatch == otherObj.MaxPerMatch)
            {
                return Equals(Upline, otherObj.Upline);
            }

            return false;
        }

        public override int GetHashCode()
        {
            var uplineHashCode = Upline != null ? Upline.GetHashCode() : 0;

            return SportTypeId.GetHashCode()
                ^ BetTypeId.GetHashCode()
                ^ MinBet.GetHashCode()
                ^ MaxBet.GetHashCode()
                ^ MaxPerMatch.GetHashCode()
                ^ uplineHashCode;
        }
    }
}
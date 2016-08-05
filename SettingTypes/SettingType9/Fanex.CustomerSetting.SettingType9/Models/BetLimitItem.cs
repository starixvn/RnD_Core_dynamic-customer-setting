namespace Fanex.CustomerSetting.SettingType9.Models
{
    using Common.Helper;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Results;
    using Dtos;
    using Resources;

    public class BetLimitItem : SettingItemDefault
    {
        private const string BetLimitXmlFormat = "<BetLimit {0} MinBet=\"{1}\" MaxBet=\"{2}\" MaxPerNumber=\"{3}\"/>";

        public BetLimitItem()
        {
        }

        internal BetLimitItem(BetLimitDto dto)
        {
            Check.Argument.IsNotNull(dto, nameof(dto));

            SportTypeId = dto.SportTypeId;
            BetTypeId = dto.BetTypeId;
            MaxBet = dto.MaxBet;
            MinBet = dto.MinBet;
            MaxPerNumber = dto.MaxPerNumber;
            Upline = new BetLimitItem
            {
                MinBet = dto.MinBetMin,
                MaxBet = dto.MaxBetMax,
                MaxPerNumber = dto.MaxPerNumber
            };
        }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerNumber { get; set; }

        public BetLimitItem Upline { get; set; }

        public override IResult Validate()
        {
            if (MinBet > Constants.MaxBetLimitValue
                || MaxBet > Constants.MaxBetLimitValue
                || MaxPerNumber > Constants.MaxBetLimitValue)
            {
                return new Result(ErrorCode.BetLimitExceedMaxValue, Message.BetLimitCannotBeGreaterThanMaxValue);
            }

            if (MinBet < 0)
            {
                return new Result(ErrorCode.MinBetIsNegative, Message.MinBetCannotBeNegative);
            }

            if (MaxBet < 0)
            {
                return new Result(ErrorCode.MaxBetIsNegative, Message.MaxBetCannotBeNegative);
            }

            if (MaxPerNumber < 0)
            {
                return new Result(ErrorCode.MaxPerNumberIsNegative, Message.MaxPerNumberCannotBeNegative);
            }

            if (MinBet > MaxBet)
            {
                return new Result(ErrorCode.MinBetIsGreaterThanMaxBet, Message.MinBetCannotBeGreaterThanMaxBet);
            }

            if (MaxBet > MaxPerNumber)
            {
                return new Result(ErrorCode.MaxBetIsGreaterThanMaxPerNumber, Message.MaxBetCannotBeGreaterThanMaxPerNumber);
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
                MaxPerNumber);
        }
    }
}
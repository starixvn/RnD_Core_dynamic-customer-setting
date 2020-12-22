namespace Fanex.CustomerSetting.SettingType20.Models
{
    using Common.Helper;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Results;
    using Dtos;
    using Resources;

    public class BetLimitItem : SettingItemDefault
    {
        private const string BetLimitXmlFormat = "<BetLimit {0} MinBet=\"{1}\" MaxBet=\"{2}\"/>";

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
            Upline = new BetLimitItem
            {
                MinBet = dto.MinBetMin,
                MaxBet = dto.MaxBetMax,
            };
        }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public BetLimitItem Upline { get; set; }

        public override IResult Validate()
        {
            if (MinBet > Constants.MaxBetLimitValue
                || MaxBet > Constants.MaxBetLimitValue)
            {
                return new Result(ErrorCodes.BetLimitExceedMaxValue, Message.BetLimitCannotBeGreaterThanMaxValue);
            }

            if (MinBet < 0)
            {
                return new Result(ErrorCodes.MinBetIsNegative, Message.MinBetCannotBeNegative);
            }

            if (MaxBet < 0)
            {
                return new Result(ErrorCodes.MaxBetIsNegative, Message.MaxBetCannotBeNegative);
            }

            if (MaxBet > 0 && MinBet > MaxBet)
            {
                return new Result(ErrorCodes.MinBetIsGreaterThanMaxBet, Message.MinBetCannotBeGreaterThanMaxBet);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(
                BetLimitXmlFormat,
                base.ToXml(),
                MinBet,
                MaxBet);
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
                && MaxBet == otherObj.MaxBet)
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
                ^ uplineHashCode;
        }
    }
}
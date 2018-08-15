namespace Fanex.CustomerSetting.SettingType5.Models
{
    using Common.Helper;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Results;
    using Dtos;
    using Resources;

    public class ChargeCommissionItem : SettingItemDefault
    {
        private const string ChargeCommissionXmlFormat = "<Discount {0} Discount=\"{1}\"/>";

        public ChargeCommissionItem()
        {
        }

        internal ChargeCommissionItem(ChargeCommissionDto chargeCommissionDto)
        {
            Check.Argument.IsNotNull(chargeCommissionDto, nameof(chargeCommissionDto));

            SportTypeId = chargeCommissionDto.SportTypeId;
            BetTypeId = chargeCommissionDto.BetTypeId;
            ChargeCommission = chargeCommissionDto.ChargeCommission;
            IntervalStep = chargeCommissionDto.IntervalStep;
            Upline = new ChargeCommissionItem
            {
                ChargeCommission = chargeCommissionDto.UplineChargeCommission
            };

            if (chargeCommissionDto.UplineOfUplineChargeCommission.HasValue)
            {
                Upline.Upline = new ChargeCommissionItem
                {
                    ChargeCommission = chargeCommissionDto.UplineOfUplineChargeCommission.Value
                };
            }
        }

        public decimal ChargeCommission { get; set; }

        public decimal IntervalStep { get; set; }

        public ChargeCommissionItem Upline { get; set; }

        public override IResult Validate()
        {
            if (ChargeCommission > Constants.MaxChargeCommissionRateValue)
            {
                return new Result(
                    ErrorCodes.ShareCannotExceedMaxValue,
                    Message.ShareCannotBeExceed100Percent);
            }

            if (ChargeCommission < 0)
            {
                return new Result(
                    ErrorCodes.ShareMustBePositive,
                    Message.ShareMustBePositive);
            }

            if (Upline == null || ChargeCommission > Upline.ChargeCommission)
            {
                return new Result(
                    ErrorCodes.ShareCannotBeGreaterThanGivenCommissionError,
                    Message.ShareMustBeLessThanOrEqualToGivenCommissionError);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(ChargeCommissionXmlFormat, base.ToXml(), ChargeCommission);
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as ChargeCommissionItem;

            if (otherObj == null)
            {
                return false;
            }
            else
            {
                return Equals(SportTypeId, otherObj.SportTypeId)
                    && Equals(BetTypeId, otherObj.BetTypeId)
                    && Equals(ChargeCommission, otherObj.ChargeCommission);
            }
        }

        public override int GetHashCode()
        {
            return SportTypeId.GetHashCode() ^ BetTypeId.GetHashCode() ^ ChargeCommission.GetHashCode();
        }
    }
}
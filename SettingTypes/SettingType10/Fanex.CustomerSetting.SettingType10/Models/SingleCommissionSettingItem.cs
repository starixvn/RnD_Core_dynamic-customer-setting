namespace Fanex.CustomerSetting.SettingType10.Models
{
    using Common.Helper;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Results;
    using Dtos;
    using Resources;

    public class SingleCommissionSettingItem : SettingItemDefault
    {
        private const string CommissionXmlFormat = "<Discount {0} Discount=\"{1}\"/>";

        public SingleCommissionSettingItem()
        {
        }

        internal SingleCommissionSettingItem(decimal rate)
        {
            Rate = rate;
        }

        internal SingleCommissionSettingItem(CommissionDto dto)
        {
            Check.Argument.IsNotNull(dto, nameof(dto));

            SportTypeId = dto.SportTypeId;
            BetTypeId = dto.BetTypeId;
            Rate = dto.Rate;
            Upline = new SingleCommissionSettingItem(dto.UplineRate);
        }

        public decimal Rate { get; set; }

        public SingleCommissionSettingItem Upline { get; set; }

        public override IResult Validate()
        {
            if (Rate > Constants.MaxCommissionRate)
            {
                return new Result(ErrorCode.CommissionExceedsMaxValue, Message.CommissionCannotExceedMaxValue);
            }

            if (Rate < 0)
            {
                return new Result(ErrorCode.CommissionIsNegative, Message.CommissionCannotBeNegative);
            }

            if (Upline != null && Rate > Upline.Rate)
            {
                return new Result(ErrorCode.CommissionMustBeLessThanValue, string.Format(Message.CommissionMustBeLessThan, Upline.Rate));
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(
                CommissionXmlFormat,
                base.ToXml(),
                Rate);
        }
    }
}
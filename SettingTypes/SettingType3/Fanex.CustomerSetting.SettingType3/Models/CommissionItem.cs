namespace Fanex.CustomerSetting.SettingType3.Models
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.SettingType3.Constants;
    using Fanex.CustomerSetting.SettingType3.Dtos;
    using Fanex.CustomerSetting.SettingType3.Resources;

    public class CommissionItem : SettingItemDefault
    {
        private const string CommissionXmlFormat = "<Discount {0} Discount=\"{1}\"/>";

        public CommissionItem()
        {
        }

        internal CommissionItem(CommissionItemDto commItemDto)
        {
            Check.Argument.IsNotNull(commItemDto, "commItemDto");

            SportTypeId = commItemDto.SportTypeId;
            BetTypeId = commItemDto.BetTypeId;
            CommRate = commItemDto.CommRate;
            IntervalStep = commItemDto.IntervalStep;
            Upline = new CommissionItem
            {
                CommRate = commItemDto.UplineCommRate,
                Upline = commItemDto.UplineOfUplineCommRate.HasValue ? new CommissionItem
                {
                    CommRate = commItemDto.UplineOfUplineCommRate.Value
                }
                : null
            };
        }

        public decimal CommRate { get; set; }

        public decimal IntervalStep { get; set; }

        public CommissionItem Upline { get; set; }

        public override IResult Validate()
        {
            if (CommRate > Constants.MaxCommissionRateValue)
            {
                return new Result(
                    ErrorCodes.CommissionRateExceedMaxValue,
                    Message.CommRateCannotBeExceedMaxValue);
            }

            if (CommRate < 0)
            {
                return new Result(
                    ErrorCodes.CommRateIsNegative,
                    Message.CommRateCannotBeNegative);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(
                CommissionXmlFormat,
                base.ToXml(),
                CommRate);
        }
    }
}
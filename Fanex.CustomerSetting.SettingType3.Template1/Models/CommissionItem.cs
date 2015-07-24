namespace Fanex.CustomerSetting.SettingType3.Template1.Models
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType3.Template1.Constants;
    using Fanex.CustomerSetting.SettingType3.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType3.Template1.Resources;

    public class CommissionItem : SettingItemDefault
    {
        private const string CommissionXmlFormat = "<Discount {0} Discount=\"{1}\"/>";

        public CommissionItem()
        {
        }

        public CommissionItem(CommissionItemDto commItemDto)
        {
            Check.Argument.IsNotNull(commItemDto, "commItemDto");

            SportTypeId = commItemDto.SportTypeId;
            BetTypeId = commItemDto.BetTypeId;
            CommRate = commItemDto.CommRate;
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

        public CommissionItem Upline { get; set; }

        public override IResult Validate()
        {
            if (CommRate > Constants.MaxCommissionRateValue)
            {
                return new Result(
                    ErrorCode.CommissionRateExceedMaxValue,
                    CommissionMessage.CommRateCannotBeExceedMaxValue);
            }

            if (CommRate < 0)
            {
                return new Result(
                    ErrorCode.CommRateIsNegative,
                    CommissionMessage.CommRateCannotBeNegative);
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
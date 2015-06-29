namespace Fanex.CustomerSetting.SettingType3.Template1.Models
{
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType3.Template1.Constants;
    using Fanex.CustomerSetting.SettingType3.Template1.Resources;

    public class CommissionItem : DefaultSettingItem
    {
        private const string CommissionXmlFormat = "<Discount {0} Discount=\"{1}\"/>";

        public decimal CommRate { get; set; }

        public CommissionItem Upline { get; set; }

        public override IResult Validate()
        {
            if (CommRate > SettingType3Constants.MaxCommissionRateValue)
            {
                return new Result(
                    SettingType3Constants.CommissionRateExceedMaxValueErrorCode,
                    CommissionError.CommRateCannotBeExceedMaxValue);
            }

            if (CommRate < 0)
            {
                return new Result(
                    SettingType3Constants.CommRateIsNegativeErrorCode,
                    CommissionError.CommRateCannotBeNegative);
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
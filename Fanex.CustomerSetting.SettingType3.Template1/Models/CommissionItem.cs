namespace Fanex.CustomerSetting.SettingType3.Template1.Models
{
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType3.Template1.Constants;

    public class CommissionItem : DefaultSettingItem
    {
        private const string CommissionXmlFormat = "<Discount {0} Discount=\"{1}\"/>";

        public decimal CommRate { get; set; }

        public CommissionItem Upline { get; set; }

        public override IResult Validate()
        {
            if (CommRate < 0)
            {
                return new Result(SettingType3Constants.CommRateIsNegativeErrorCode);
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
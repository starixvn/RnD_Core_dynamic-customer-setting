namespace Fanex.CustomerSetting.SettingType3.Template1.Models
{
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType3.Template1.Constants;

    public class CommissionItem : SportTypeBetTypeSettingItem
    {
        private const string CommissionXmlFormat = "<Discount {0} Discount=\"{1}\" />";

        public decimal CommRate { get; set; }

        public CommissionItem Constraint { get; set; }

        public override IResult Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.IsSuccessful)
            {
                return baseResult;
            }

            if (CommRate < 0)
            {
                return new Result(SettingType3ErrorCodes.CommRateIsNegative);
            }

            return Result.Success;
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
namespace Fanex.CustomerSetting.SettingType4.Template1.Models
{
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType4.Template1.Constants;

    public class ProductStatusItem : DefaultSettingItem
    {
        private const string ProductXmlFormat = "<ProductStatus {0} IsDisabled=\"{1}\" IsDisabledDownline=\"{2}\"/>";

        public bool IsDisabled { get; set; }

        public ProductStatusItem Upline { get; set; }

        public ProductStatusItem Downline { get; set; }

        public override IResult Validate()
        {
            var baseResult = base.Validate();
            if (!baseResult.IsSuccessful)
            {
                return baseResult;
            }

            if (Downline == null)
            {
                return new Result(SettingType4ErrorCodes.DownlineProductStatusIsNull);
            }

            if (IsDisabled && !Downline.IsDisabled)
            {
                return new Result(SettingType4ErrorCodes.DownlineIsEnabledWhenUplineIsDisabled);
            }

            return Result.Success;
        }

        public override string ToXml()
        {
            return string.Format(
                ProductXmlFormat,
                base.ToXml(),
                IsDisabled ? 1 : 0,
                Downline.IsDisabled ? 1 : 0);
        }
    }
}
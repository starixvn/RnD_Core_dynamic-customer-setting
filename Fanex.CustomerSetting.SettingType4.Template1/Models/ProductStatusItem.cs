namespace Fanex.CustomerSetting.SettingType4.Template1.Models
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType4.Template1.Constants;
    using Fanex.CustomerSetting.SettingType4.Template1.Dtos;
    using Fanex.CustomerSetting.SettingType4.Template1.Resources;

    public class ProductStatusItem : DefaultSettingItem
    {
        private const string ProductXmlFormat = "<ProductStatus {0} IsDisabled=\"{1}\" IsDisabledDownline=\"{2}\"/>";

        public ProductStatusItem()
        {
        }

        public ProductStatusItem(ProductStatusItemDto productStatusItemDto)
        {
            Check.Argument.IsNotNull(productStatusItemDto, "productStatusItemDto");

            SportTypeId = productStatusItemDto.SportTypeId;
            BetTypeId = productStatusItemDto.BetTypeId;
            IsDisabled = productStatusItemDto.IsDisabled;
            Downline = productStatusItemDto.IsDisabledDownline.HasValue ? new ProductStatusItem
            {
                IsDisabled = productStatusItemDto.IsDisabledDownline.Value
            }
            : null;
            Upline = productStatusItemDto.IsDisabledUpline.HasValue ? new ProductStatusItem
            {
                IsDisabled = productStatusItemDto.IsDisabledUpline.Value
            }
            : null;
        }

        public bool IsDisabled { get; set; }

        public ProductStatusItem Upline { get; set; }

        public ProductStatusItem Downline { get; set; }

        public override IResult Validate()
        {
            if (Downline != null && IsDisabled && !Downline.IsDisabled)
            {
                return new Result(
                    ErrorCode.DownlineIsEnabledWhenUplineIsDisabled,
                    ProductStatusMessage.DownlineCannotBeEnabledWhenUplineIsDisabled);
            }

            return base.Validate();
        }

        public override string ToXml()
        {
            return string.Format(
                ProductXmlFormat,
                base.ToXml(),
                IsDisabled ? 1 : 0,
                (Downline != null && Downline.IsDisabled) ? 1 : 0);
        }
    }
}
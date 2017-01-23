namespace Fanex.CustomerSetting.SettingType13.Models
{
    using Common.Helper;
    using Constants;
    using CustomerSetting.Models;
    using CustomerSetting.Models.Results;
    using Dtos;
    using Resources;

    public class ProductStatusItem : SettingItemDefault
    {
        private const string ProductXmlFormat = "<ProductStatus {0} IsDisabled=\"{1}\" IsDisabledDownline=\"{2}\"/>";

        public ProductStatusItem()
        {
        }

        internal ProductStatusItem(bool isDisabled)
        {
            IsDisabled = isDisabled;
        }

        internal ProductStatusItem(ProductStatusDto productStatusItemDto)
        {
            Check.Argument.IsNotNull(productStatusItemDto, nameof(productStatusItemDto));

            SportTypeId = productStatusItemDto.SportTypeId;
            BetTypeId = productStatusItemDto.BetTypeId;
            IsDisabled = productStatusItemDto.IsDisabled;

            if (productStatusItemDto.HasDownline && productStatusItemDto.IsDisabledDownline.HasValue)
            {
                Downline = new ProductStatusItem(productStatusItemDto.IsDisabledDownline.Value);
            }

            if (productStatusItemDto.IsDisabledUpline.HasValue)
            {
                Upline = new ProductStatusItem(productStatusItemDto.IsDisabledUpline.Value);
            }
        }

        public bool IsDisabled { get; set; }

        public ProductStatusItem Upline { get; set; }

        public ProductStatusItem Downline { get; set; }

        public override IResult Validate()
        {
            if (IsDisabled && Downline?.IsDisabled == false)
            {
                return new Result(
                    ErrorCode.DownlineIsEnabledWhenUplineIsDisabled,
                    Message.DownlineCannotBeEnabledWhenUplineIsDisabled);
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
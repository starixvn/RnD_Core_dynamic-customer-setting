namespace Fanex.CustomerSetting.SettingType11.Models
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

        internal ProductStatusItem(ProductStatusItemDto productStatusItemDto)
        {
            Check.Argument.IsNotNull(productStatusItemDto, nameof(productStatusItemDto));

            SportTypeId = productStatusItemDto.SportTypeId;
            BetTypeId = productStatusItemDto.BetTypeId;
            IsDisabled = productStatusItemDto.IsDisabled;

            if (productStatusItemDto.HasDownline)
            {
                Downline =
                    productStatusItemDto.IsDisabledDownline.HasValue
                    ? new ProductStatusItem
                    {
                        IsDisabled = productStatusItemDto.IsDisabledDownline.Value
                    }
                    : null;
            }

            Upline =
                productStatusItemDto.IsDisabledUpline.HasValue
                ? new ProductStatusItem
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
                Downline?.IsDisabled == true ? 1 : 0);
        }
    }
}
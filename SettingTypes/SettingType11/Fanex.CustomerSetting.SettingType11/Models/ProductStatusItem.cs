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
        private const string ProductStatusXmlFormat = "<ProductStatus {0} IsDisabled=\"{1}\" IsDisabledDownline=\"{2}\"/>";
        private const string ProductStatusWithoutDownlineXmlFormat = "<ProductStatus {0} IsDisabled=\"{1}\"/>";

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

            if (productStatusItemDto.HasDownline
                && productStatusItemDto.IsDisabledDownline.HasValue)
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
            if (Downline == null)
            {
                return string.Format(ProductStatusWithoutDownlineXmlFormat, base.ToXml(), IsDisabled ? 1 : 0);
            }

            return string.Format(ProductStatusXmlFormat, base.ToXml(), IsDisabled ? 1 : 0, Downline.IsDisabled ? 1 : 0);
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as ProductStatusItem;

            if (otherObj == null)
            {
                return false;
            }

            if (ReferenceEquals(otherObj, this))
            {
                return true;
            }

            if (SportTypeId == otherObj.SportTypeId
                && BetTypeId == otherObj.BetTypeId
                && IsDisabled == otherObj.IsDisabled)
            {
                return Equals(Upline, otherObj.Upline) && Equals(Downline, otherObj.Downline);
            }

            return false;
        }

        public override int GetHashCode()
        {
            var uplineHashCode = Upline != null ? Upline.GetHashCode() : 0;
            var downlineHashCode = Downline != null ? Downline.GetHashCode() : 0;

            return SportTypeId.GetHashCode()
                ^ BetTypeId.GetHashCode()
                ^ IsDisabled.GetHashCode()
                ^ uplineHashCode ^ downlineHashCode;
        }
    }
}
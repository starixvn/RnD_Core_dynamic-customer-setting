namespace Fanex.CustomerSetting.SettingType21.Models
{
    using Common.Helper;
    using CustomerSetting.Models;
    using Dtos;

    public class ProductStatusItem : SettingItemDefault
    {
        private const string ProductStatusXmlFormat = "<ProductStatus {0} IsDisabled=\"{1}\"/>";
        
        public short ParentSportTypeId { get; set; }

        public short ParentBetTypeId { get; set; }
        
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
            ParentSportTypeId = productStatusItemDto.ParentSportType;
            ParentBetTypeId = productStatusItemDto.ParentBetType;
            
            if (productStatusItemDto.IsDisabledUpline.HasValue)
            {
                Upline = new ProductStatusItem(productStatusItemDto.IsDisabledUpline.Value);
            }
        }

        public bool IsDisabled { get; set; }
        
        public bool IsReadOnly { get; set; }

        public ProductStatusItem Upline { get; set; }

        public override string ToXml()
            => string.Format(ProductStatusXmlFormat, base.ToXml(), IsDisabled ? 1 : 0);

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
                return Equals(Upline, otherObj.Upline);
            }

            return false;
        }

        public override int GetHashCode()
        {
            var uplineHashCode = Upline != null ? Upline.GetHashCode() : 0;

            return SportTypeId.GetHashCode()
                ^ BetTypeId.GetHashCode()
                ^ IsDisabled.GetHashCode()
                ^ uplineHashCode;
        }
    }
}
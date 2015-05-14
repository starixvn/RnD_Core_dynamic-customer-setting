namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    public class ProductStatusEntity : BaseEntity
    {
        [OutputElement]
        public bool IsDisabled { get; set; }

        public bool IsHidden { get; set; }

        public bool IsSync { get; set; }
    }
}
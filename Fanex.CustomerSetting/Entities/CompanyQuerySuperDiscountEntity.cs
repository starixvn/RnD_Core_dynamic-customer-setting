namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    public class CompanyQuerySuperDiscountEntity : DiscountEntity
    {
        [OutputElement("Discount")]
        public decimal SuperDiscount { get; set; }

        public decimal DefaultMaxDiscount { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && IsDiscountValue(SuperDiscount);
        }
    }
}
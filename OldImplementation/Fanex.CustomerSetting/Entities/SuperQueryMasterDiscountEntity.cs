namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    public class SuperQueryMasterDiscountEntity : DiscountEntity
    {
        [OutputElement("Discount")]
        public decimal MasterDiscount { get; set; }

        public decimal SuperDiscount { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && IsDiscountValue(MasterDiscount);
        }
    }
}
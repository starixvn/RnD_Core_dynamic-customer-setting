namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    public class AgentQueryMemberDiscountEntity : DiscountEntity
    {
        [OutputElement("Discount")]
        public decimal PlayerDiscount { get; set; }

        public decimal AgentDiscount { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && IsDiscountValue(PlayerDiscount);
        }
    }
}
namespace Fanex.CustomerSetting.Entities
{
    using Fanex.CustomerSetting.Attributes;

    public class MasterQueryAgentDiscountEntity : DiscountEntity
    {
        [OutputElement("Discount")]
        public decimal AgentDiscount { get; set; }

        public decimal MasterDiscount { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && IsDiscountValue(AgentDiscount);
        }
    }
}
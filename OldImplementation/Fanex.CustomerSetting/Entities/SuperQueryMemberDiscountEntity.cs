namespace Fanex.CustomerSetting.Entities
{
    public class SuperQueryMemberDiscountEntity : DiscountEntity
    {
        public decimal PlayerDiscount { get; set; }

        public decimal AgentDiscount { get; set; }

        public decimal MasterDiscount { get; set; }

        public override bool IsValid()
        {
            return base.IsValid() && PlayerDiscount >= 0;
        }
    }
}
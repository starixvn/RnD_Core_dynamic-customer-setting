namespace Fanex.CustomerSetting.Criteria
{
    /// <summary>
    /// CommandDiscountCriteria class.
    /// </summary>
    public class CommandDiscountCriteria : CommandCriteria
    {
        public string Discount { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_DiscountUpd";
        }

        public override bool IsValid()
        {
            return base.IsValid() && !string.IsNullOrEmpty(Discount);
        }
    }
}
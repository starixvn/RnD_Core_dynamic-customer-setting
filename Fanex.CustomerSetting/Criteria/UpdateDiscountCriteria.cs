namespace Fanex.CustomerSetting.Criteria
{
    /// <summary>
    /// UpdateStatusCriteria class.
    /// </summary>
    public class UpdateDiscountCriteria : UpdateCriteria
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
namespace Fanex.CustomerSetting.Criteria
{
    /// <summary>
    /// UpdateStatusCriteria class.
    /// </summary>
    public class UpdateBetLimitCriteria : UpdateCriteria
    {
        public string BetLimit { get; set; }

        public override string GetSettingKey()
        {
            return "Acc_CustSetting_BetLimitUpd";
        }

        public override bool IsValid()
        {
            return base.IsValid() && !string.IsNullOrEmpty(BetLimit);
        }
    }
}
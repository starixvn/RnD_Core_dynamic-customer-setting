namespace Fanex.CustomerSetting.Criteria
{
    public class BetLimitCommandCriteria : CommandCriteria
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
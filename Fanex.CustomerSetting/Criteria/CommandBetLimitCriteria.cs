namespace Fanex.CustomerSetting.Criteria
{
    /// <summary>
    /// CommandBetLimitCriteria class.
    /// </summary>
    public class CommandBetLimitCriteria : CommandCriteria
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